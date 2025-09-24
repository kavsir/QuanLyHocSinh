using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers;

[Authorize(Roles = "Admin")]
public class NamhocsController(QuanLyHocSinhContext context) : Controller
{
    // GET: Namhocs
    public async Task<IActionResult> Index()
    {
        return View(await context.Namhocs.ToListAsync());
    }

    // GET: Namhocs/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var namhoc = await context.Namhocs
            .FirstOrDefaultAsync(m => m.MaNamHoc == id);
        if (namhoc == null)
        {
            return NotFound();
        }

        return View(namhoc);
    }

    // GET: Namhocs/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Namhocs/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TenNamHoc,NgayBatDau,NgayKetThuc")] Namhoc namhoc)
    {
        if (ModelState.IsValid)
        {
            // Chuẩn hóa chuỗi: chuyển thành chữ thường và xóa tất cả khoảng trắng
            var normalizedTenNamHoc = new string(namhoc.TenNamHoc!.Where(c => !Char.IsWhiteSpace(c)).ToArray()).ToLower();

            var existing = await context.Namhocs
                .ToListAsync(); // Lấy về để xử lý trong bộ nhớ

            if (existing.Any(n => new string(n.TenNamHoc!.Where(c => !Char.IsWhiteSpace(c)).ToArray()).ToLower() == normalizedTenNamHoc))
            {
                ModelState.AddModelError("TenNamHoc", "Tên năm học này đã tồn tại (có thể khác về khoảng trắng).");
            }
            else
            {
                context.Add(namhoc);
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tạo năm học mới thành công!";
                return RedirectToAction(nameof(Index));
            }
        }
        return View(namhoc);
    }

    // GET: Namhocs/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var namhoc = await context.Namhocs.FindAsync(id);
        if (namhoc == null)
        {
            return NotFound();
        }
        return View(namhoc);
    }

    // POST: Namhocs/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MaNamHoc,TenNamHoc,NgayBatDau,NgayKetThuc")] Namhoc namhoc)
    {
        if (id != namhoc.MaNamHoc)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            // --- LOGIC KIỂM TRA TRÙNG LẶP ĐÃ SỬA LỖI ---
            var normalizedTenNamHoc = new string(namhoc.TenNamHoc!.Where(c => !Char.IsWhiteSpace(c)).ToArray()).ToLower();

            // Dùng AsNoTracking() để EF không "theo dõi" các đối tượng này, chỉ dùng để đọc
            var existing = await context.Namhocs.AsNoTracking().ToListAsync();

            if (existing.Any(n => new string(n.TenNamHoc!.Where(c => !Char.IsWhiteSpace(c)).ToArray()).ToLower() == normalizedTenNamHoc && n.MaNamHoc != id))
            {
                ModelState.AddModelError("TenNamHoc", "Tên năm học này đã được sử dụng bởi một bản ghi khác.");
            }
            else
            {
                // --- LOGIC CẬP NHẬT AN TOÀN ---
                try
                {
                    // Không dùng context.Update() trực tiếp nữa.
                    // Thay vào đó, chúng ta tìm bản ghi gốc, cập nhật các trường của nó, rồi lưu.
                    var namhocToUpdate = await context.Namhocs.FindAsync(id);
                    if (namhocToUpdate != null)
                    {
                        namhocToUpdate.TenNamHoc = namhoc.TenNamHoc;
                        namhocToUpdate.NgayBatDau = namhoc.NgayBatDau;
                        namhocToUpdate.NgayKetThuc = namhoc.NgayKetThuc;

                        await context.SaveChangesAsync();
                        TempData["SuccessMessage"] = "Cập nhật năm học thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NamhocExists(namhoc.MaNamHoc))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
        return View(namhoc);
    }

    

    private bool NamhocExists(int id)
    {
        return context.Namhocs.Any(e => e.MaNamHoc == id);
    }
}