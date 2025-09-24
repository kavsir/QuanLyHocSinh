using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers;

[Authorize(Roles = "Admin")]
public class PhancongGiangdaysController : Controller
{
    private readonly QuanLyHocSinhContext _context;

    public PhancongGiangdaysController(QuanLyHocSinhContext context)
    {
        _context = context;
    }

    // GET: PhancongGiangdays
    public async Task<IActionResult> Index(int? hocKyFilter, int? lopFilter)
    {
        // Populate dropdowns with null checks and fallback for null properties
        var hockyList = await _context.Hockies.OrderByDescending(h => h.TenHk).ToListAsync() ?? new List<Hocky>();
        var lopList = await _context.Lops.OrderBy(l => l.TenLop).ToListAsync() ?? new List<Lop>();

        // Ensure TenHk and TenLop are not null
        ViewBag.HocKyList = new SelectList(
            hockyList.Select(h => new { h.MaHk, TenHk = h.TenHk ?? "Không xác định" }),
            "MaHk",
            "TenHk",
            hocKyFilter
        );
        ViewBag.LopList = new SelectList(
            lopList.Select(l => new { l.MaLop, TenLop = l.TenLop ?? "Không xác định" }),
            "MaLop",
            "TenLop",
            lopFilter
        );

        var phancongsQuery = _context.PhancongGiangdays
            .Include(p => p.MaGvNavigation)
            .Include(p => p.MaHkNavigation)
            .Include(p => p.MaLopNavigation)
            .Include(p => p.MaMonHocNavigation)
            .AsQueryable();

        if (hocKyFilter.HasValue)
        {
            phancongsQuery = phancongsQuery.Where(p => p.MaHk == hocKyFilter);
        }
        if (lopFilter.HasValue)
        {
            phancongsQuery = phancongsQuery.Where(p => p.MaLop == lopFilter);
        }

        return View(await phancongsQuery.OrderBy(p => p.MaLopNavigation!.TenLop).ToListAsync());
    }

    private async Task PopulateDropdownsAsync(object? selectedGv = null, object? selectedMonHoc = null, object? selectedLop = null, object? selectedHk = null)
    {
        var giaovienList = await _context.Giaoviens.OrderBy(g => g.HoTen).ToListAsync() ?? new List<Giaovien>();
        var monhocList = await _context.Monhocs.OrderBy(m => m.TenMonHoc).ToListAsync() ?? new List<Monhoc>();
        var lopList = await _context.Lops.OrderBy(l => l.TenLop).ToListAsync() ?? new List<Lop>();
        var hockyList = await _context.Hockies.OrderByDescending(h => h.TenHk).ToListAsync() ?? new List<Hocky>();

        ViewData["MaGv"] = new SelectList(
            giaovienList.Select(g => new { g.MaGv, HoTen = g.HoTen ?? "Không xác định" }),
            "MaGv",
            "HoTen",
            selectedGv
        );
        ViewData["MaMonHoc"] = new SelectList(
            monhocList.Select(m => new { m.MaMonHoc, TenMonHoc = m.TenMonHoc ?? "Không xác định" }),
            "MaMonHoc",
            "TenMonHoc",
            selectedMonHoc
        );
        ViewData["MaLop"] = new SelectList(
            lopList.Select(l => new { l.MaLop, TenLop = l.TenLop ?? "Không xác định" }),
            "MaLop",
            "TenLop",
            selectedLop
        );
        ViewData["MaHk"] = new SelectList(
            hockyList.Select(h => new { h.MaHk, TenHk = h.TenHk ?? "Không xác định" }),
            "MaHk",
            "TenHk",
            selectedHk
        );
    }

    // GET: PhancongGiangdays/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var phancongGiangday = await _context.PhancongGiangdays
            .Include(p => p.MaGvNavigation)
            .Include(p => p.MaHkNavigation)
            .Include(p => p.MaLopNavigation)
            .Include(p => p.MaMonHocNavigation)
            .FirstOrDefaultAsync(m => m.MaPc == id);

        if (phancongGiangday == null)
        {
            return NotFound();
        }

        return View(phancongGiangday);
    }

    // GET: PhancongGiangdays/Create
    public async Task<IActionResult> Create()
    {
        await PopulateDropdownsAsync();
        return View();
    }

    // POST: PhancongGiangdays/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaGv,MaMonHoc,MaLop,MaHk")] PhancongGiangday phancong)
    {
        if (ModelState.IsValid)
        {
            // Check for existing assignment
            var existing = await _context.PhancongGiangdays.FirstOrDefaultAsync(p =>
                p.MaLop == phancong.MaLop &&
                p.MaHk == phancong.MaHk &&
                p.MaMonHoc == phancong.MaMonHoc);

            if (existing != null)
            {
                ModelState.AddModelError("", "Môn học này đã được phân công cho một giáo viên khác trong lớp và học kỳ này.");
            }
            else
            {
                _context.Add(phancong);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tạo phân công mới thành công!";
                return RedirectToAction(nameof(Index));
            }
        }
        await PopulateDropdownsAsync(phancong.MaGv, phancong.MaMonHoc, phancong.MaLop, phancong.MaHk);
        return View(phancong);
    }

    // GET: PhancongGiangdays/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var phancongGiangday = await _context.PhancongGiangdays.FindAsync(id);
        if (phancongGiangday == null)
        {
            return NotFound();
        }
        await PopulateDropdownsAsync(phancongGiangday.MaGv, phancongGiangday.MaMonHoc, phancongGiangday.MaLop, phancongGiangday.MaHk);
        return View(phancongGiangday);
    }

    // POST: PhancongGiangdays/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MaPc,MaGv,MaMonHoc,MaLop,MaHk")] PhancongGiangday phancongGiangday)
    {
        if (id != phancongGiangday.MaPc)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(phancongGiangday);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhancongGiangdayExists(phancongGiangday.MaPc))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        await PopulateDropdownsAsync(phancongGiangday.MaGv, phancongGiangday.MaMonHoc, phancongGiangday.MaLop, phancongGiangday.MaHk);
        return View(phancongGiangday);
    }

    // GET: PhancongGiangdays/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var phancongGiangday = await _context.PhancongGiangdays
            .Include(p => p.MaGvNavigation)
            .Include(p => p.MaHkNavigation)
            .Include(p => p.MaLopNavigation)
            .Include(p => p.MaMonHocNavigation)
            .FirstOrDefaultAsync(m => m.MaPc == id);

        if (phancongGiangday == null)
        {
            return NotFound();
        }

        return View(phancongGiangday);
    }

    // POST: PhancongGiangdays/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var phancongGiangday = await _context.PhancongGiangdays.FindAsync(id);
        if (phancongGiangday != null)
        {
            _context.PhancongGiangdays.Remove(phancongGiangday);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool PhancongGiangdayExists(int id)
    {
        return _context.PhancongGiangdays.Any(e => e.MaPc == id);
    }
}