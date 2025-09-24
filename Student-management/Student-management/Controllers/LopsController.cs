using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers;

// Yêu cầu người dùng phải đăng nhập để truy cập controller này
[Authorize]
public class LopsController(QuanLyHocSinhContext context) : Controller
{
    // CHỈ ADMIN: Mới được xem trang danh sách tổng hợp
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index(string? searchString, int? namHocFilter, int? gvcnFilter)
    {
        ViewBag.NamHocList = new SelectList(await context.Namhocs.OrderByDescending(n => n.TenNamHoc).ToListAsync(), "MaNamHoc", "TenNamHoc", namHocFilter);
        ViewBag.GvcnList = new SelectList(await context.Giaoviens.OrderBy(g => g.HoTen).ToListAsync(), "MaGv", "HoTen", gvcnFilter);
        ViewData["CurrentSearch"] = searchString;

        var lopsQuery = context.Lops
            .Include(l => l.MaGvcnNavigation)
            .Include(l => l.MaNamHocNavigation)
            .AsQueryable();

        if (!String.IsNullOrEmpty(searchString)) { lopsQuery = lopsQuery.Where(l => l.TenLop != null && l.TenLop.Contains(searchString)); }
        if (namHocFilter.HasValue) { lopsQuery = lopsQuery.Where(l => l.MaNamHoc == namHocFilter); }
        if (gvcnFilter.HasValue) { lopsQuery = lopsQuery.Where(l => l.MaGvcn == gvcnFilter); }

        return View(await lopsQuery.OrderBy(l => l.TenLop).ToListAsync());
    }

    // ADMIN & GIÁO VIÊN: Cả hai đều có quyền xem trang này
    [Authorize(Roles = "Admin,GiaoVien")]
    public async Task<IActionResult> ClassDetailsForTeacher(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var lopDetails = await context.Lops
            .Include(l => l.MaGvcnNavigation)
            .Include(l => l.MaNamHocNavigation)
            .Include(l => l.Hocsinhs)
            .FirstOrDefaultAsync(l => l.MaLop == id);

        if (lopDetails == null)
        {
            return NotFound();
        }
        lopDetails.Hocsinhs = lopDetails.Hocsinhs.OrderBy(hs => hs.HoTen).ToList();

        return View(lopDetails);
    }

    // CHỈ ADMIN: Mới có quyền xem chi tiết (dành cho admin)
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // Tải thông tin lớp học cùng với TẤT CẢ các dữ liệu liên quan
        var lop = await context.Lops
            .Include(l => l.MaNamHocNavigation) // Thông tin năm học
            .Include(l => l.MaGvcnNavigation)   // Thông tin GVCN
            .Include(l => l.Hocsinhs)           // DANH SÁCH HỌC SINH trong lớp
            .Include(l => l.PhancongGiangdays)  // Danh sách phân công giảng dạy
                .ThenInclude(pc => pc.MaGvNavigation) // Tên giáo viên bộ môn
            .Include(l => l.PhancongGiangdays)
                .ThenInclude(pc => pc.MaMonHocNavigation) // Tên môn học
            .Include(l => l.Lichhocs)           // Lịch học hàng tuần
                .ThenInclude(lh => lh.MaMonHocNavigation)
            .AsNoTracking() // Dùng AsNoTracking() để tăng hiệu suất cho trang chỉ xem
            .FirstOrDefaultAsync(m => m.MaLop == id);

        if (lop == null)
        {
            return NotFound();
        }

        return View(lop);
    }

    private async Task PopulateDropdownsAsync(object? selectedGvcn = null, object? selectedNamHoc = null)
    {
        ViewData["MaGvcn"] = new SelectList(await context.Giaoviens.OrderBy(g => g.HoTen).ToListAsync(), "MaGv", "HoTen", selectedGvcn);
        ViewData["MaNamHoc"] = new SelectList(await context.Namhocs.OrderByDescending(n => n.TenNamHoc).ToListAsync(), "MaNamHoc", "TenNamHoc", selectedNamHoc);
    }

    // CHỈ ADMIN: Mới có quyền tạo, sửa, xóa
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create()
    {
        await PopulateDropdownsAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([Bind("TenLop,MaNamHoc,MaGvcn")] Lop lop)
    {
        if (ModelState.IsValid)
        {
            var normalizedTenLop = lop.TenLop?.Trim().ToLower();
            var existing = await context.Lops.FirstOrDefaultAsync(l => l.MaNamHoc == lop.MaNamHoc && l.TenLop != null && l.TenLop.ToLower() == normalizedTenLop);
            if (existing != null)
            {
                ModelState.AddModelError("TenLop", "Tên lớp này đã tồn tại trong năm học đã chọn.");
            }
            else
            {
                lop.TenLop = lop.TenLop?.Trim();
                lop.SiSo = 0;
                context.Add(lop);
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tạo lớp học mới thành công!";
                return RedirectToAction(nameof(Index));
            }
        }
        await PopulateDropdownsAsync(lop.MaGvcn, lop.MaNamHoc);
        return View(lop);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var lop = await context.Lops.FindAsync(id);
        if (lop == null) return NotFound();
        await PopulateDropdownsAsync(lop.MaGvcn, lop.MaNamHoc);
        return View(lop);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, [Bind("MaLop,TenLop,SiSo,MaNamHoc,MaGvcn")] Lop lop)
    {
        if (id != lop.MaLop) return NotFound();

        if (ModelState.IsValid)
        {
            var normalizedTenLop = lop.TenLop?.Trim().ToLower();
            var existing = await context.Lops.AsNoTracking().FirstOrDefaultAsync(l => l.MaNamHoc == lop.MaNamHoc && l.TenLop != null && l.TenLop.ToLower() == normalizedTenLop && l.MaLop != id);
            if (existing != null)
            {
                ModelState.AddModelError("TenLop", "Tên lớp này đã tồn tại trong năm học đã chọn.");
            }
            else
            {
                try
                {
                    lop.TenLop = lop.TenLop?.Trim();
                    context.Update(lop);
                    await context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật lớp học thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LopExists(lop.MaLop)) return NotFound();
                    else throw;
                }
            }
        }
        await PopulateDropdownsAsync(lop.MaGvcn, lop.MaNamHoc);
        return View(lop);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var lop = await context.Lops
            .Include(l => l.MaGvcnNavigation)
            .Include(l => l.MaNamHocNavigation)
            .FirstOrDefaultAsync(m => m.MaLop == id);
        if (lop == null) return NotFound();
        return View(lop);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var lop = await context.Lops.FindAsync(id);
        if (lop != null)
        {
            context.Lops.Remove(lop);
            await context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool LopExists(int id)
    {
        return context.Lops.Any(e => e.MaLop == id);
    }
}