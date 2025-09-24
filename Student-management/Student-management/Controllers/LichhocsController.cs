using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers;

[Authorize]
public class LichhocsController(QuanLyHocSinhContext context) : Controller
{
    [Authorize(Roles = "Admin")]

    // GET: Lichhocs - Phiên bản ổn định, không có bộ lọc
    public async Task<IActionResult> Index()
    {
        var lichhocs = await context.Lichhocs
            .Include(l => l.MaGvNavigation)
            .Include(l => l.MaHkNavigation)
            .Include(l => l.MaLopNavigation)
            .Include(l => l.MaMonHocNavigation)
            .Include(l => l.MaPhongNavigation)
            .ToListAsync();

        var sortedLichhocs = lichhocs
            .OrderBy(l => l.MaLopNavigation?.TenLop)
            .ThenBy(l => l.ThuTrongTuan)
            .ThenBy(l => l.TietHoc);

        return View(sortedLichhocs);
    }
    // ADMIN & GIÁO VIÊN: Xem TKB cá nhân
    [Authorize(Roles = "Admin,GiaoVien")]
    public async Task<IActionResult> MySchedule()
    {
        var username = User.Identity?.Name;
        var taikhoan = await context.Taikhoans.FirstOrDefaultAsync(tk => tk.TenDangNhap == username);
        if (taikhoan?.MaGv == null)
        {
            return RedirectToAction("Index", "Home");
        }
        var gvId = taikhoan.MaGv.Value;

        var teacher = await context.Giaoviens.FindAsync(gvId);
        var currentHk = await context.Hockies.Where(h => h.NgayBatDau.Date <= DateTime.Now.Date).OrderByDescending(h => h.NgayBatDau).FirstOrDefaultAsync();

        var viewModel = new MyScheduleViewModel
        {
            TenGiaoVien = teacher?.HoTen ?? "N/A",
            TenHocKy = currentHk?.TenHk ?? "N/A"
        };

        if (currentHk != null)
        {
            var scheduleData = await context.Lichhocs
                .Where(l => l.MaGv == gvId && l.MaHk == currentHk.MaHk)
                .Include(l => l.MaLopNavigation)
                .Include(l => l.MaMonHocNavigation)
                .Include(l => l.MaPhongNavigation)
                .ToListAsync();

            foreach (var lich in scheduleData)
            {
                if (lich.ThuTrongTuan != null && lich.TietHoc.HasValue)
                {
                    if (!viewModel.TimetableGrid.ContainsKey(lich.ThuTrongTuan))
                    {
                        viewModel.TimetableGrid[lich.ThuTrongTuan] = new Dictionary<int, Lichhoc>();
                    }
                    viewModel.TimetableGrid[lich.ThuTrongTuan][lich.TietHoc.Value] = lich;
                }
            }
        }

        return View(viewModel);
    }
    // GET: Lichhocs/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var lichhoc = await context.Lichhocs
            .Include(l => l.MaGvNavigation)
            .Include(l => l.MaHkNavigation)
            .Include(l => l.MaLopNavigation)
            .Include(l => l.MaMonHocNavigation)
            .Include(l => l.MaPhongNavigation)
            .FirstOrDefaultAsync(m => m.MaLichHoc == id);

        if (lichhoc == null) return NotFound();

        return View(lichhoc);
    }

    // Phương thức hỗ trợ để chuẩn bị dữ liệu cho 5 dropdowns
    private async Task PopulateDropdownsAsync(object? selectedLop = null, object? selectedMonHoc = null, object? selectedGv = null, object? selectedPhong = null, object? selectedHk = null)
    {
        ViewData["MaLop"] = new SelectList(await context.Lops.OrderBy(l => l.TenLop).ToListAsync(), "MaLop", "TenLop", selectedLop);
        ViewData["MaMonHoc"] = new SelectList(await context.Monhocs.OrderBy(m => m.TenMonHoc).ToListAsync(), "MaMonHoc", "TenMonHoc", selectedMonHoc);
        ViewData["MaGv"] = new SelectList(await context.Giaoviens.OrderBy(g => g.HoTen).ToListAsync(), "MaGv", "HoTen", selectedGv);
        ViewData["MaPhong"] = new SelectList(await context.Phonghocs.OrderBy(p => p.TenPhong).ToListAsync(), "MaPhong", "TenPhong", selectedPhong);
        ViewData["MaHk"] = new SelectList(await context.Hockies.OrderByDescending(h => h.TenHk).ToListAsync(), "MaHk", "TenHk", selectedHk);
    }

    // GET: Lichhocs/Create
    public async Task<IActionResult> Create()
    {
        // Quan trọng: Gọi hàm để chuẩn bị dữ liệu cho View
        await PopulateDropdownsAsync();
        return View();
    }

    // POST: Lichhocs/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaLop,MaMonHoc,MaGv,MaPhong,MaHk,ThuTrongTuan,TietHoc")] Lichhoc lichhoc)
    {
        if (ModelState.IsValid)
        {
            // === BẮT ĐẦU LOGIC KIỂM TRA XUNG ĐỘT NÂNG CAO ===

            // 1. Kiểm tra LỚP HỌC có bị trùng lịch không
            var classConflict = await context.Lichhocs
                .Include(l => l.MaMonHocNavigation) // Lấy thông tin môn học đã được xếp
                .FirstOrDefaultAsync(l =>
                    l.MaHk == lichhoc.MaHk &&
                    l.ThuTrongTuan == lichhoc.ThuTrongTuan &&
                    l.TietHoc == lichhoc.TietHoc &&
                    l.MaLop == lichhoc.MaLop);

            if (classConflict != null)
            {
                var monHocDaXep = classConflict.MaMonHocNavigation?.TenMonHoc ?? "Không rõ";
                ModelState.AddModelError("", $"Xung đột: Lớp này đã có lịch học môn '{monHocDaXep}' vào thời điểm này.");
            }

            // 2. Kiểm tra GIÁO VIÊN có bị trùng lịch không
            var teacherConflict = await context.Lichhocs
                .Include(l => l.MaLopNavigation) // Lấy thông tin lớp mà giáo viên đó đang dạy
                .FirstOrDefaultAsync(l =>
                    l.MaHk == lichhoc.MaHk &&
                    l.ThuTrongTuan == lichhoc.ThuTrongTuan &&
                    l.TietHoc == lichhoc.TietHoc &&
                    l.MaGv == lichhoc.MaGv);

            if (teacherConflict != null)
            {
                var lopDaDay = teacherConflict.MaLopNavigation?.TenLop ?? "Không rõ";
                ModelState.AddModelError("", $"Xung đột: Giáo viên này đã có lịch dạy ở lớp '{lopDaDay}' vào thời điểm này.");
            }

            // 3. Kiểm tra PHÒNG HỌC có bị trùng lịch không
            var roomConflict = await context.Lichhocs
                .Include(l => l.MaLopNavigation) // Lấy thông tin lớp đang sử dụng phòng
                .FirstOrDefaultAsync(l =>
                    l.MaHk == lichhoc.MaHk &&
                    l.ThuTrongTuan == lichhoc.ThuTrongTuan &&
                    l.TietHoc == lichhoc.TietHoc &&
                    l.MaPhong == lichhoc.MaPhong);

            if (roomConflict != null)
            {
                var lopDangHoc = roomConflict.MaLopNavigation?.TenLop ?? "Không rõ";
                ModelState.AddModelError("", $"Xung đột: Phòng học này đang được lớp '{lopDangHoc}' sử dụng. Vui lòng chọn phòng khác hoặc tiết khác.");
            }

            // === KẾT THÚC LOGIC KIỂM TRA XUNG ĐỘT ===

            if (ModelState.ErrorCount == 0)
            {
                context.Add(lichhoc);
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tạo lịch học mới thành công!";
                return RedirectToAction(nameof(Index));
            }
        }
        await PopulateDropdownsAsync(lichhoc.MaLop, lichhoc.MaMonHoc, lichhoc.MaGv, lichhoc.MaPhong, lichhoc.MaHk);
        return View(lichhoc);
    }

    // GET: Lichhocs/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var lichhoc = await context.Lichhocs.FindAsync(id);
        if (lichhoc == null) return NotFound();
        await PopulateDropdownsAsync(lichhoc.MaLop, lichhoc.MaMonHoc, lichhoc.MaGv, lichhoc.MaPhong, lichhoc.MaHk);
        return View(lichhoc);
    }

    // POST: Lichhocs/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MaLichHoc,MaLop,MaMonHoc,MaGv,MaPhong,MaHk,ThuTrongTuan,TietHoc")] Lichhoc lichhoc)
    {
        if (id != lichhoc.MaLichHoc) return NotFound();

        if (ModelState.IsValid)
        {
            // Logic kiểm tra xung đột cho action Edit
            var conflict = await context.Lichhocs.FirstOrDefaultAsync(l =>
               // Phải khác với bản ghi đang sửa
               l.MaLichHoc != lichhoc.MaLichHoc &&
               l.MaHk == lichhoc.MaHk &&
               l.ThuTrongTuan == lichhoc.ThuTrongTuan &&
               l.TietHoc == lichhoc.TietHoc &&
               (l.MaLop == lichhoc.MaLop || l.MaGv == lichhoc.MaGv || l.MaPhong == lichhoc.MaPhong)
            );

            if (conflict != null)
            {
                ModelState.AddModelError("", "Thay đổi này gây ra xung đột với một lịch học khác đã có.");
            }
            else
            {
                try
                {
                    context.Update(lichhoc);
                    await context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật lịch học thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LichhocExists(lichhoc.MaLichHoc)) return NotFound();
                    else throw;
                }
            }
        }
        await PopulateDropdownsAsync(lichhoc.MaLop, lichhoc.MaMonHoc, lichhoc.MaGv, lichhoc.MaPhong, lichhoc.MaHk);
        return View(lichhoc);
    }

    // GET: Lichhocs/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var lichhoc = await context.Lichhocs
           .Include(l => l.MaGvNavigation).Include(l => l.MaHkNavigation)
           .Include(l => l.MaLopNavigation).Include(l => l.MaMonHocNavigation)
           .Include(l => l.MaPhongNavigation)
           .FirstOrDefaultAsync(m => m.MaLichHoc == id);
        if (lichhoc == null) return NotFound();
        return View(lichhoc);
    }

    // POST: Lichhocs/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var lichhoc = await context.Lichhocs.FindAsync(id);
        if (lichhoc != null)
        {
            context.Lichhocs.Remove(lichhoc);
            await context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool LichhocExists(int id)
    {
        return context.Lichhocs.Any(e => e.MaLichHoc == id);
    }
}