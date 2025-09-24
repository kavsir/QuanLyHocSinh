using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers;

[Authorize(Roles = "Admin")]
public class GiaoviensController(QuanLyHocSinhContext context) : Controller
{
    // GET: Giaoviens
    // GET: Giaoviens
    public async Task<IActionResult> Index(string searchString, int? subjectFilter)
    {
        ViewBag.SubjectList = new SelectList(await context.Monhocs.OrderBy(m => m.TenMonHoc).ToListAsync(), "MaMonHoc", "TenMonHoc", subjectFilter);
        ViewData["CurrentSearch"] = searchString;
        ViewData["CurrentSubjectFilter"] = subjectFilter;

        var giaoviens = context.Giaoviens
            .Include(g => g.MaMonHocNavigation)
            .AsQueryable();

        if (!String.IsNullOrEmpty(searchString))
        {
            giaoviens = giaoviens.Where(s => s.HoTen != null && s.HoTen.Contains(searchString));
        }

        if (subjectFilter.HasValue)
        {
            giaoviens = giaoviens.Where(x => x.MaMonHoc == subjectFilter);
        }

        return View(await giaoviens.OrderBy(g => g.HoTen).ToListAsync());
    }

    // GET: Giaoviens/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // Tải thông tin giáo viên cùng với TẤT CẢ các dữ liệu liên quan
        var giaovien = await context.Giaoviens
            .Include(g => g.MaMonHocNavigation) // Môn chuyên môn
            .Include(g => g.Lops) // Các lớp đang chủ nhiệm
                .ThenInclude(l => l.MaNamHocNavigation) // Năm học của lớp chủ nhiệm
            .Include(g => g.PhancongGiangdays) // Các phân công giảng dạy
                .ThenInclude(pc => pc.MaLopNavigation)
            .Include(g => g.PhancongGiangdays)
                .ThenInclude(pc => pc.MaMonHocNavigation)
            .Include(g => g.PhancongGiangdays)
                .ThenInclude(pc => pc.MaHkNavigation)
            .Include(g => g.Lichhocs) // Lịch dạy hàng tuần
                .ThenInclude(lh => lh.MaLopNavigation)
            .Include(g => g.Lichhocs)
                .ThenInclude(lh => lh.MaMonHocNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.MaGv == id);

        if (giaovien == null)
        {
            return NotFound();
        }

        return View(giaovien);
    }

    // GET: Giaoviens/Create
    public IActionResult Create()
    {
        ViewData["MaMonHoc"] = new SelectList(context.Monhocs, "MaMonHoc", "TenMonHoc");
        return View();
    }

    // POST: Giaoviens/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("HoTen,NgaySinh,GioiTinh,DiaChi,Sdt,Email,MaMonHoc")] Giaovien giaovien)
    {
        if (ModelState.IsValid)
        {
            context.Add(giaovien);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["MaMonHoc"] = new SelectList(context.Monhocs, "MaMonHoc", "TenMonHoc", giaovien.MaMonHoc);
        return View(giaovien);
    }

    // GET: Giaoviens/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var giaovien = await context.Giaoviens.FindAsync(id);
        if (giaovien == null)
        {
            return NotFound();
        }

        // Chuẩn bị dropdown cho Môn học, chọn sẵn môn hiện tại của giáo viên
        ViewData["MaMonHoc"] = new SelectList(context.Monhocs, "MaMonHoc", "TenMonHoc", giaovien.MaMonHoc);
        return View(giaovien);
    }

    // POST: Giaoviens/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MaGv,HoTen,NgaySinh,GioiTinh,DiaChi,Sdt,Email,MaMonHoc")] Giaovien giaovien)
    {
        if (id != giaovien.MaGv)
        {
            return NotFound();
        }

        // Ràng buộc nâng cao: Kiểm tra xem Email có bị trùng với một giáo viên khác không
        bool emailExists = await context.Giaoviens.AnyAsync(g => g.Email == giaovien.Email && g.MaGv != giaovien.MaGv);
        if (emailExists)
        {
            ModelState.AddModelError("Email", "Email này đã được sử dụng bởi một giáo viên khác.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(giaovien);
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật thông tin giáo viên thành công!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GiaovienExists(giaovien.MaGv))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Details), new { id = giaovien.MaGv });
        }

        // Nếu có lỗi, tải lại dropdown và hiển thị lại form
        ViewData["MaMonHoc"] = new SelectList(context.Monhocs, "MaMonHoc", "TenMonHoc", giaovien.MaMonHoc);
        return View(giaovien);
    }

    private bool GiaovienExists(int maGv)
    {
        throw new NotImplementedException();
    }

    // GET: Giaoviens/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var giaovien = await context.Giaoviens
            .Include(g => g.MaMonHocNavigation)
            .FirstOrDefaultAsync(m => m.MaGv == id);
        if (giaovien == null)
        {
            return NotFound();
        }

        // <<< KIỂM TRA CÁC RÀNG BUỘC TRƯỚC KHI XÓA >>>
        bool isHomeroomTeacher = await context.Lops.AnyAsync(l => l.MaGvcn == id);
        bool hasAssignments = await context.PhancongGiangdays.AnyAsync(p => p.MaGv == id);
        bool hasSchedule = await context.Lichhocs.AnyAsync(l => l.MaGv == id);
        bool hasAccount = await context.Taikhoans.AnyAsync(t => t.MaGv == id);

        if (isHomeroomTeacher || hasAssignments || hasSchedule || hasAccount)
        {
            string errorMessage = "Không thể xóa giáo viên này vì các lý do sau: ";
            if (isHomeroomTeacher) errorMessage += "Đang là GVCN của một lớp. ";
            if (hasAssignments) errorMessage += "Đang được phân công giảng dạy. ";
            if (hasSchedule) errorMessage += "Vẫn còn lịch dạy trong thời khóa biểu. ";
            if (hasAccount) errorMessage += "Vẫn còn tài khoản đăng nhập liên kết. ";

            ViewBag.ErrorMessage = errorMessage;
        }

        return View(giaovien);
    }

    // POST: Giaoviens/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var giaovien = await context.Giaoviens.FindAsync(id);
        if (giaovien == null)
        {
            return NotFound();
        }

        // <<< KIỂM TRA LẠI CÁC RÀNG BUỘC TRƯỚC KHI XÓA >>>
        bool canDelete = !(await context.Lops.AnyAsync(l => l.MaGvcn == id) ||
                           await context.PhancongGiangdays.AnyAsync(p => p.MaGv == id) ||
                           await context.Lichhocs.AnyAsync(l => l.MaGv == id) ||
                           await context.Taikhoans.AnyAsync(t => t.MaGv == id));

        if (canDelete)
        {
            context.Giaoviens.Remove(giaovien);
            await context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã xóa giáo viên '{giaovien.HoTen}' thành công!";
        }
        else
        {
            TempData["ErrorMessage"] = "Không thể xóa giáo viên do vẫn còn các dữ liệu liên quan.";
        }

        return RedirectToAction(nameof(Index));
    }
}