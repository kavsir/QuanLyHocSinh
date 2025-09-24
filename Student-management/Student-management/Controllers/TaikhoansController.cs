using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers;

[Authorize(Roles = "Admin")]
public class TaikhoansController(QuanLyHocSinhContext context) : Controller
{
    // GET: Taikhoans
    public async Task<IActionResult> Index(string? searchString, string? roleFilter)
    {
        ViewBag.RoleList = new SelectList(new List<string> { "Admin", "GiaoVien", "HocSinh" }, roleFilter);
        ViewData["CurrentSearch"] = searchString;
        ViewData["CurrentRoleFilter"] = roleFilter;

        var taikhoansQuery = context.Taikhoans
            .Include(t => t.MaHsNavigation)
            .Include(t => t.MaGvNavigation)
            .AsQueryable();

        if (!String.IsNullOrEmpty(searchString))
        {
            taikhoansQuery = taikhoansQuery.Where(t =>
                (t.TenDangNhap != null && t.TenDangNhap.Contains(searchString)) ||
                (t.MaHsNavigation != null && t.MaHsNavigation.HoTen!.Contains(searchString)) ||
                (t.MaGvNavigation != null && t.MaGvNavigation.HoTen!.Contains(searchString))
            );
        }

        if (!string.IsNullOrEmpty(roleFilter))
        {
            taikhoansQuery = taikhoansQuery.Where(t => t.VaiTro == roleFilter);
        }

        return View(await taikhoansQuery.OrderBy(t => t.VaiTro).ThenBy(t => t.TenDangNhap).ToListAsync());
    }

    // GET: Taikhoans/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var taikhoan = await context.Taikhoans
            .Include(t => t.MaGvNavigation)
            .Include(t => t.MaHsNavigation)
            .FirstOrDefaultAsync(m => m.MaTk == id);
        if (taikhoan == null) return NotFound();
        return View(taikhoan);
    }

    // GET: Taikhoans/CreateUserAndAccount
    public async Task<IActionResult> CreateUserAndAccount()
    {
        ViewBag.MonHocList = new SelectList(await context.Monhocs.OrderBy(m => m.TenMonHoc).ToListAsync(), "MaMonHoc", "TenMonHoc");
        return View(new CreateUserAndAccountViewModel());
    }

    // POST: Taikhoans/CreateUserAndAccount
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUserAndAccount(CreateUserAndAccountViewModel model)
    {
        // Kiểm tra các quy tắc nghiệp vụ
        if (await context.Taikhoans.AnyAsync(t => t.TenDangNhap == model.TenDangNhap))
            ModelState.AddModelError("TenDangNhap", "Tên đăng nhập này đã tồn tại.");

        if (await context.Hocsinhs.AnyAsync(h => h.Email == model.Email) || await context.Giaoviens.AnyAsync(g => g.Email == model.Email))
            ModelState.AddModelError("Email", "Email này đã được sử dụng.");

        if (model.VaiTro == "GiaoVien" && model.MaMonHoc == null)
            ModelState.AddModelError("MaMonHoc", "Vui lòng chọn môn chuyên môn cho giáo viên.");

        if (!ModelState.IsValid)
        {
            ViewBag.MonHocList = new SelectList(await context.Monhocs.OrderBy(m => m.TenMonHoc).ToListAsync(), "MaMonHoc", "TenMonHoc", model.MaMonHoc);
            return View(model);
        }

        // <<< SỬ DỤNG TRANSACTION ĐỂ ĐẢM BẢO TOÀN VẸN DỮ LIỆU >>>
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            // Bước 1: Tạo người dùng (Hocsinh hoặc Giaovien)
            int? newHsId = null;
            int? newGvId = null;

            if (model.VaiTro == "HocSinh")
            {
                var newStudent = new Hocsinh { /* ... gán các thuộc tính từ model ... */ };
                context.Hocsinhs.Add(newStudent);
                await context.SaveChangesAsync();
                newHsId = newStudent.MaHs;
            }
            else if (model.VaiTro == "GiaoVien")
            {
                var newTeacher = new Giaovien { /* ... gán các thuộc tính từ model ... */ };
                context.Giaoviens.Add(newTeacher);
                await context.SaveChangesAsync();
                newGvId = newTeacher.MaGv;
            }

            // Bước 2: Tạo tài khoản và liên kết
            var newAccount = new Taikhoan
            {
                TenDangNhap = model.TenDangNhap,
                // <<< BĂM MẬT KHẨU TRƯỚC KHI LƯU >>>
                MatKhau = BCrypt.Net.BCrypt.HashPassword(model.MatKhau),
                VaiTro = model.VaiTro,
                MaHs = newHsId,
                MaGv = newGvId
            };
            context.Taikhoans.Add(newAccount);
            await context.SaveChangesAsync();

            // Nếu mọi thứ thành công, commit transaction
            await transaction.CommitAsync();

            TempData["SuccessMessage"] = $"Đã tạo thành công người dùng và tài khoản '{model.TenDangNhap}'.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            // Nếu có bất kỳ lỗi nào, hủy bỏ tất cả thay đổi
            await transaction.RollbackAsync();
            ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình tạo người dùng. Vui lòng thử lại.");
            ViewBag.MonHocList = new SelectList(await context.Monhocs.OrderBy(m => m.TenMonHoc).ToListAsync(), "MaMonHoc", "TenMonHoc", model.MaMonHoc);
            return View(model);
        }
    }

    // GET: Taikhoans/Edit/5 (Đổi mật khẩu)
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var taikhoan = await context.Taikhoans
            .Include(t => t.MaHsNavigation)
            .Include(t => t.MaGvNavigation)
            .FirstOrDefaultAsync(t => t.MaTk == id);

        if (taikhoan == null) return NotFound();

        var viewModel = new ResetPasswordViewModel
        {
            MaTk = taikhoan.MaTk,
            TenDangNhap = taikhoan.TenDangNhap,
            // <<< GÁN DỮ LIỆU CHO CÁC THUỘC TÍNH MỚI >>>
            VaiTro = taikhoan.VaiTro,
            NguoiDungLienKet = taikhoan.MaHsNavigation != null
                ? $"{taikhoan.MaHsNavigation.HoTen} (Học sinh)"
                : taikhoan.MaGvNavigation != null
                    ? $"{taikhoan.MaGvNavigation.HoTen} (Giáo viên)"
                    : "Không có"
        };
        return View(viewModel);
    }

    // POST: Taikhoans/Edit/5 (Đổi mật khẩu)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ResetPasswordViewModel model)
    {
        if (id != model.MaTk) return NotFound();

        if (ModelState.IsValid)
        {
            var accountToUpdate = await context.Taikhoans.FindAsync(id);
            if (accountToUpdate == null) return NotFound();

            // <<< BĂM MẬT KHẨU MỚI TRƯỚC KHI LƯU >>>
            accountToUpdate.MatKhau = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            await context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Đã cập nhật mật khẩu cho tài khoản '{accountToUpdate.TenDangNhap}' thành công!";
            return RedirectToAction(nameof(Index));
        }

        // Nếu model không hợp lệ, trả về view với lỗi
        return View(model);
    }

    // GET: Taikhoans/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var taikhoan = await context.Taikhoans
            .Include(t => t.MaGvNavigation)
            .Include(t => t.MaHsNavigation)
            .FirstOrDefaultAsync(m => m.MaTk == id);
        if (taikhoan == null) return NotFound();
        return View(taikhoan);
    }

    // POST: Taikhoans/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var taikhoan = await context.Taikhoans.FindAsync(id);
        if (taikhoan != null)
        {
            context.Taikhoans.Remove(taikhoan);
            await context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}