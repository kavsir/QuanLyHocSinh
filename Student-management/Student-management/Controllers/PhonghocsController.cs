using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers;

[Authorize(Roles = "Admin")]
public class PhonghocsController(QuanLyHocSinhContext context) : Controller
{
    // GET: Phonghocs
    public async Task<IActionResult> Index(string? searchString)
    {
        // Lưu lại từ khóa tìm kiếm để hiển thị lại trên ô tìm kiếm
        ViewData["CurrentFilter"] = searchString;

        // Bắt đầu truy vấn
        var phonghocsQuery = from p in context.Phonghocs
                             select p;

        // Nếu có từ khóa, lọc kết quả theo Tên phòng hoặc Vị trí
        if (!String.IsNullOrEmpty(searchString))
        {
            phonghocsQuery = phonghocsQuery.Where(p =>
                (p.TenPhong != null && p.TenPhong.Contains(searchString)) ||
                (p.ViTri != null && p.ViTri.Contains(searchString))
            );
        }

        // Trả về danh sách đã được lọc và sắp xếp
        return View(await phonghocsQuery.OrderBy(p => p.ViTri).ThenBy(p => p.TenPhong).ToListAsync());
    }

    // GET: Phonghocs/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var phonghoc = await context.Phonghocs
            .Include(p => p.Lichhocs) // Tải lịch học của phòng này
                .ThenInclude(l => l.MaLopNavigation) // Kèm thông tin Lớp
            .Include(p => p.Lichhocs)
                .ThenInclude(l => l.MaGvNavigation) // Kèm thông tin Giáo viên
            .Include(p => p.Lichhocs)
                .ThenInclude(l => l.MaMonHocNavigation) // Kèm thông tin Môn học
            .Include(p => p.Lichhocs)
                .ThenInclude(l => l.MaHkNavigation) // Kèm thông tin Học kỳ
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.MaPhong == id);

        if (phonghoc == null)
        {
            return NotFound();
        }

        return View(phonghoc);
    }

    // GET: Phonghocs/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Phonghocs/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TenPhong,SucChua,ViTri")] Phonghoc phonghoc)
    {
        // --- BỔ SUNG LOGIC KIỂM TRA TRÙNG LẶP ---
        if (!string.IsNullOrEmpty(phonghoc.TenPhong))
        {
            var existingPhong = await context.Phonghocs
                .FirstOrDefaultAsync(p => p.TenPhong != null && p.TenPhong.ToLower() == phonghoc.TenPhong.ToLower());

            if (existingPhong != null)
            {
                // Nếu tìm thấy phòng đã tồn tại, thêm lỗi vào ModelState
                ModelState.AddModelError("TenPhong", "Tên phòng học này đã tồn tại. Vui lòng chọn một tên khác.");
            }
        }

        if (ModelState.IsValid)
        {
            context.Add(phonghoc);
            await context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Tạo phòng học mới thành công!";
            return RedirectToAction(nameof(Index));
        }

        // Nếu có lỗi, trả về view Create với dữ liệu người dùng đã nhập
        return View(phonghoc);
    }


    // GET: Phonghocs/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var phonghoc = await context.Phonghocs.FindAsync(id);
        if (phonghoc == null) return NotFound();
        return View(phonghoc);
    }

    // POST: Phonghocs/Edit/5 - ĐÃ THÊM VALIDATION
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MaPhong,TenPhong,SucChua,ViTri")] Phonghoc phonghoc)
    {
        if (id != phonghoc.MaPhong) return NotFound();

        if (ModelState.IsValid)
        {
            var existingPhong = await context.Phonghocs
                .FirstOrDefaultAsync(p => p.TenPhong != null && p.TenPhong.ToLower() == phonghoc.TenPhong!.ToLower() && p.MaPhong != id);

            if (existingPhong != null)
            {
                ModelState.AddModelError("TenPhong", "Tên phòng học này đã tồn tại.");
            }
            else
            {
                try
                {
                    context.Update(phonghoc);
                    await context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật thông tin phòng học thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhonghocExists(phonghoc.MaPhong)) return NotFound();
                    else throw;
                }
            }
        }
        return View(phonghoc);
    }

    // GET: Phonghocs/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var phonghoc = await context.Phonghocs
            .FirstOrDefaultAsync(m => m.MaPhong == id);
        if (phonghoc == null)
        {
            return NotFound();
        }

        // Kiểm tra xem phòng học có đang được sử dụng không
        bool isInUse = await context.Lichhocs.AnyAsync(l => l.MaPhong == id);
        if (isInUse)
        {
            ViewBag.ErrorMessage = "Không thể xóa phòng học này vì nó đang được sử dụng trong thời khóa biểu. Vui lòng xóa các lịch học liên quan trước.";
        }

        return View(phonghoc);
    }

    // POST: Phonghocs/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // Kiểm tra lại một lần nữa trước khi xóa
        bool isInUse = await context.Lichhocs.AnyAsync(l => l.MaPhong == id);
        if (isInUse)
        {
            // Nếu vẫn còn sử dụng, không xóa và báo lỗi
            TempData["ErrorMessage"] = "Không thể xóa phòng học đang được sử dụng.";
            return RedirectToAction(nameof(Index));
        }

        var phonghoc = await context.Phonghocs.FindAsync(id);
        if (phonghoc != null)
        {
            context.Phonghocs.Remove(phonghoc);
            await context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Xóa phòng học thành công!";
        }

        return RedirectToAction(nameof(Index));
    }
    private bool PhonghocExists(int id)
    {
        return context.Phonghocs.Any(e => e.MaPhong == id);
    }
}