using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;
using OfficeOpenXml;

namespace Student_management.Controllers;

[Authorize(Roles = "Admin")]
public class HocsinhsController(QuanLyHocSinhContext context) : Controller
{
    // GET: Hocsinhs
    public async Task<IActionResult> Index(string searchString, int? classFilter)
    {
        // --- Lấy dữ liệu cho bộ lọc Lớp học ---
        var lopHocList = await context.Lops.OrderBy(l => l.TenLop).ToListAsync();
        ViewBag.ClassList = new SelectList(lopHocList, "MaLop", "TenLop", classFilter);

        // --- Lưu lại các giá trị lọc để hiển thị lại trên View ---
        ViewData["CurrentSearch"] = searchString;
        ViewData["CurrentClassFilter"] = classFilter;

        // --- Bắt đầu truy vấn ---
        var hocsinhs = context.Hocsinhs
            .Include(h => h.MaLopNavigation)
            .AsQueryable();

        // --- Áp dụng bộ lọc tìm kiếm theo Tên ---
        if (!String.IsNullOrEmpty(searchString))
        {
            hocsinhs = hocsinhs.Where(s => s.HoTen != null && s.HoTen.Contains(searchString));
        }

        // --- Áp dụng bộ lọc theo Lớp học ---
        if (classFilter.HasValue)
        {
            hocsinhs = hocsinhs.Where(x => x.MaLop == classFilter);
        }

        // Sắp xếp và trả về kết quả
        return View(await hocsinhs.OrderBy(h => h.HoTen).ToListAsync());
    }

    // GET: Hocsinhs/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var hocsinh = await context.Hocsinhs
            .Include(h => h.MaLopNavigation)
                .ThenInclude(l => l.MaNamHocNavigation)
            .Include(h => h.MaLopNavigation)
                .ThenInclude(l => l.MaGvcnNavigation)
            .Include(h => h.Diems)
                .ThenInclude(d => d.MaMonHocNavigation)
            .Include(h => h.Diems)
                .ThenInclude(d => d.MaHkNavigation)
            .Include(h => h.Hocphis)
                 .ThenInclude(hp => hp.MaHkNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.MaHs == id);

        if (hocsinh == null)
        {
            return NotFound();
        }

        return View(hocsinh);
    }

    // GET: Hocsinhs/Create
    public IActionResult Create()
    {
        ViewData["MaLop"] = new SelectList(context.Lops, "MaLop", "TenLop");
        return View();
    }

    // POST: Hocsinhs/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("HoTen,NgaySinh,GioiTinh,DiaChi,Sdt,Email,MaLop,TrangThai")] Hocsinh hocsinh)
    {
        if (ModelState.IsValid)
        {
            context.Add(hocsinh);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["MaLop"] = new SelectList(context.Lops, "MaLop", "TenLop", hocsinh.MaLop);
        return View(hocsinh);
    }

    // GET: Hocsinhs/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var hocsinh = await context.Hocsinhs.FindAsync(id);
        if (hocsinh == null)
        {
            return NotFound();
        }
        ViewData["MaLop"] = new SelectList(context.Lops, "MaLop", "TenLop", hocsinh.MaLop);
        return View(hocsinh);
    }

    // POST: Hocsinhs/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MaHs,HoTen,NgaySinh,GioiTinh,DiaChi,Sdt,Email,MaLop,TrangThai")] Hocsinh hocsinh)
    {
        if (id != hocsinh.MaHs)
        {
            return NotFound();
        }

        // Kiểm tra email có bị trùng với người dùng khác không
        bool emailExists = await context.Hocsinhs.AnyAsync(h => h.Email == hocsinh.Email && h.MaHs != hocsinh.MaHs) ||
                           await context.Giaoviens.AnyAsync(g => g.Email == hocsinh.Email);
        if (emailExists)
        {
            ModelState.AddModelError("Email", "Email này đã được sử dụng bởi một người dùng khác.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(hocsinh);
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật thông tin học sinh thành công!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HocsinhExists(hocsinh.MaHs))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Details), new { id = hocsinh.MaHs });
        }

        // Nếu có lỗi, tải lại dropdown và trả về view
        ViewData["MaLop"] = new SelectList(context.Lops, "MaLop", "TenLop", hocsinh.MaLop);
        return View(hocsinh);
    }

    private bool HocsinhExists(int maHs)
    {
        throw new NotImplementedException();
    }

    // GET: Hocsinhs/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var hocsinh = await context.Hocsinhs
            .Include(h => h.MaLopNavigation)
            .FirstOrDefaultAsync(m => m.MaHs == id);

        if (hocsinh == null)
        {
            return NotFound();
        }

        return View(hocsinh);
    }

    // POST: Hocsinhs/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var hocsinh = await context.Hocsinhs.FindAsync(id);

        if (hocsinh == null)
        {
            return NotFound();
        }

        // Sử dụng một transaction để đảm bảo tất cả các thao tác xóa đều thành công,
        // hoặc không có gì được xóa cả nếu có lỗi xảy ra.
        using (var transaction = await context.Database.BeginTransactionAsync())
        {
            try
            {
                // Bước 1: Xóa các bản ghi điểm liên quan
                var diems = context.Diems.Where(d => d.MaHs == id);
                if (diems.Any())
                {
                    context.Diems.RemoveRange(diems);
                }

                // Bước 2: Xóa các bản ghi học phí liên quan
                var hocphis = context.Hocphis.Where(hp => hp.MaHs == id);
                if (hocphis.Any())
                {
                    context.Hocphis.RemoveRange(hocphis);
                }

                // Bước 3: Xóa các bản ghi điểm danh liên quan (nếu đã có)
                if (context.Model.FindEntityType(typeof(Diemdanh)) != null)
                {
                    var diemdanhs = context.Diemdanhs.Where(dd => dd.MaHs == id);
                    if (diemdanhs.Any())
                    {
                        context.Diemdanhs.RemoveRange(diemdanhs);
                    }
                }

                // Bước 4: Xóa các tài khoản liên quan
                var taikhoans = context.Taikhoans.Where(tk => tk.MaHs == id);
                if (taikhoans.Any())
                {
                    context.Taikhoans.RemoveRange(taikhoans);
                }

                // Bước 5: Cuối cùng, xóa học sinh
                context.Hocsinhs.Remove(hocsinh);

                // Lưu tất cả các thay đổi
                await context.SaveChangesAsync();

                // Hoàn tất transaction thành công
                await transaction.CommitAsync();

                TempData["SuccessMessage"] = $"Đã xóa thành công học sinh '{hocsinh.HoTen}' và tất cả dữ liệu liên quan.";
            }
            catch (Exception)
            {
                // Nếu có bất kỳ lỗi nào, hủy bỏ tất cả các thay đổi
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = "Đã xảy ra lỗi trong quá trình xóa học sinh.";
                // Có thể thêm log lỗi ở đây
                return RedirectToAction(nameof(Index));
            }
        }

        return RedirectToAction(nameof(Index));
    }

}