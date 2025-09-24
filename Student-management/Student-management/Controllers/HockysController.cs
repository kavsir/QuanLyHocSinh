using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;
using System.Text.RegularExpressions;

namespace Student_management.Controllers;

[Authorize(Roles = "Admin")]
public class HockysController(QuanLyHocSinhContext context) : Controller
{
    // GET: Hockys
    public async Task<IActionResult> Index(int? namHocFilter)
    {
        // Lấy danh sách năm học để đưa vào dropdown bộ lọc
        ViewBag.NamHocList = new SelectList(await context.Namhocs.OrderByDescending(n => n.TenNamHoc).ToListAsync(), "MaNamHoc", "TenNamHoc", namHocFilter);
        ViewData["CurrentNamHocFilter"] = namHocFilter;

        // Bắt đầu truy vấn, luôn Include thông tin Năm học
        var hockys = context.Hockies
            .Include(h => h.MaNamHocNavigation)
            .AsQueryable();

        // Nếu người dùng chọn một năm học từ bộ lọc, áp dụng điều kiện lọc
        if (namHocFilter.HasValue)
        {
            hockys = hockys.Where(h => h.MaNamHoc == namHocFilter);
        }

        // Trả về danh sách đã được lọc và sắp xếp
        return View(await hockys.OrderBy(h => h.NgayBatDau).ToListAsync());
    }

    // GET: Hockies/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var hocky = await context.Hockies
            .Include(h => h.MaNamHocNavigation) // Thông tin năm học
            .Include(h => h.PhancongGiangdays) // Danh sách phân công
                .ThenInclude(pc => pc.MaLopNavigation)
            .Include(h => h.PhancongGiangdays)
                .ThenInclude(pc => pc.MaMonHocNavigation)
            .Include(h => h.PhancongGiangdays)
                .ThenInclude(pc => pc.MaGvNavigation)
            .Include(h => h.Hocphis) // Danh sách học phí
                .ThenInclude(hp => hp.MaHsNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.MaHk == id);

        if (hocky == null)
        {
            return NotFound();
        }

        return View(hocky);
    }

    // GET: Hockys/Create
    public IActionResult Create()
    {
        ViewData["MaNamHoc"] = new SelectList(context.Namhocs, "MaNamHoc", "TenNamHoc");
        return View();
    }

    // POST: Hockys/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TenHk,NgayBatDau,NgayKetThuc,MaNamHoc")] Hocky hocky)
    {
        if (ModelState.IsValid)
        {
            // --- BƯỚC 1: TỰ ĐỘNG TẠO TÊN HỌC KỲ ĐẦY ĐỦ ---
            var namHoc = await context.Namhocs.FindAsync(hocky.MaNamHoc);
            string fullTenHk = hocky.TenHk; // Giữ tên gốc nếu không tìm thấy năm học

            if (namHoc != null && !string.IsNullOrEmpty(namHoc.TenNamHoc) && !string.IsNullOrEmpty(hocky.TenHk))
            {
                // Trích xuất phần năm (ví dụ: "24-25" từ "2024 - 2025")
                var yearPartMatch = Regex.Match(namHoc.TenNamHoc, @"(\d{2,4})\s*-\s*(\d{2,4})");
                if (yearPartMatch.Success)
                {
                    var startYear = yearPartMatch.Groups[1].Value.Substring(2);
                    var endYear = yearPartMatch.Groups[2].Value.Substring(2);
                    fullTenHk = $"{hocky.TenHk.Trim()} ({startYear}-{endYear})";
                }
            }

            // --- BƯỚC 2: KIỂM TRA TRÙNG LẶP DỰA TRÊN TÊN ĐẦY ĐỦ ---
            var existing = await context.Hockies.FirstOrDefaultAsync(h => h.TenHk == fullTenHk);
            if (existing != null)
            {
                ModelState.AddModelError("TenHk", $"Học kỳ '{fullTenHk}' đã tồn tại.");
            }
            else
            {
                // --- BƯỚC 3: NẾU KHÔNG TRÙNG, GÁN TÊN ĐẦY ĐỦ VÀ LƯU ---
                hocky.TenHk = fullTenHk;
                context.Add(hocky);
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tạo học kỳ mới thành công!";
                return RedirectToAction(nameof(Index));
            }
        }

        // Nếu có lỗi, tải lại dropdown và trả về view
        ViewData["MaNamHoc"] = new SelectList(await context.Namhocs.ToListAsync(), "MaNamHoc", "TenNamHoc", hocky.MaNamHoc);
        return View(hocky);
    }

    // GET: Hockys/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var hocky = await context.Hockies.FindAsync(id);
        if (hocky == null)
        {
            return NotFound();
        }
        ViewData["MaNamHoc"] = new SelectList(context.Namhocs, "MaNamHoc", "TenNamHoc", hocky.MaNamHoc);
        return View(hocky);
    }

    // POST: Hockys/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MaHk,TenHk,NgayBatDau,NgayKetThuc,MaNamHoc")] Hocky hocky)
    {
        if (id != hocky.MaHk) return NotFound();

        if (ModelState.IsValid)
        {
            // Tương tự, tự động tạo tên học kỳ đầy đủ khi Edit
            var namHoc = await context.Namhocs.FindAsync(hocky.MaNamHoc);
            string fullTenHk = hocky.TenHk;

            if (namHoc != null && !string.IsNullOrEmpty(namHoc.TenNamHoc) && !string.IsNullOrEmpty(hocky.TenHk))
            {
                var yearPartMatch = Regex.Match(namHoc.TenNamHoc, @"(\d{2,4})\s*-\s*(\d{2,4})");
                if (yearPartMatch.Success && !hocky.TenHk.Contains("(")) // Chỉ ghép nếu người dùng chọn lại tên gốc
                {
                    var startYear = yearPartMatch.Groups[1].Value.Substring(2);
                    var endYear = yearPartMatch.Groups[2].Value.Substring(2);
                    fullTenHk = $"{hocky.TenHk.Trim()} ({startYear}-{endYear})";
                }
            }

            var existing = await context.Hockies.AsNoTracking().FirstOrDefaultAsync(h => h.TenHk == fullTenHk && h.MaHk != id);
            if (existing != null)
            {
                ModelState.AddModelError("TenHk", $"Học kỳ '{fullTenHk}' đã tồn tại.");
            }
            else
            {
                try
                {
                    hocky.TenHk = fullTenHk;
                    context.Update(hocky);
                    await context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật học kỳ thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HockyExists(hocky.MaHk)) return NotFound();
                    else throw;
                }
            }
        }
        ViewData["MaNamHoc"] = new SelectList(await context.Namhocs.ToListAsync(), "MaNamHoc", "TenNamHoc", hocky.MaNamHoc);
        return View(hocky);
    }

    

    private bool HockyExists(int id)
    {
        return context.Hockies.Any(e => e.MaHk == id);
    }
}