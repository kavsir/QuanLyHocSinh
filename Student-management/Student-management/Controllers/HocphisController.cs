using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers;

[Authorize(Roles = "Admin")]
public class HocphisController(QuanLyHocSinhContext context) : Controller
{
    public async Task<IActionResult> Index(string? searchString, int? hocKyFilter, int? lopFilter, string? statusFilter)
    {
        ViewBag.HocKyList = new SelectList(await context.Hockies.OrderByDescending(h => h.TenHk).ToListAsync(), "MaHk", "TenHk", hocKyFilter);
        ViewBag.LopList = new SelectList(await context.Lops.OrderBy(l => l.TenLop).ToListAsync(), "MaLop", "TenLop", lopFilter);
        ViewBag.StatusList = new SelectList(new List<string> { "Đã đóng", "Chưa đóng" }, statusFilter);
        ViewData["CurrentSearch"] = searchString;

        var hocphisQuery = context.Hocphis
            .Include(h => h.MaHsNavigation).ThenInclude(hs => hs!.MaLopNavigation)
            .Include(h => h.MaHkNavigation)
            .AsQueryable();

        if (!String.IsNullOrEmpty(searchString))
        {
            hocphisQuery = hocphisQuery.Where(h => h.MaHsNavigation != null && h.MaHsNavigation.HoTen!.Contains(searchString));
        }
        if (hocKyFilter.HasValue) { hocphisQuery = hocphisQuery.Where(h => h.MaHk == hocKyFilter); }
        if (lopFilter.HasValue) { hocphisQuery = hocphisQuery.Where(h => h.MaHsNavigation != null && h.MaHsNavigation.MaLop == lopFilter); }
        if (!string.IsNullOrEmpty(statusFilter)) { hocphisQuery = hocphisQuery.Where(h => h.TrangThai == statusFilter); }

        return View(await hocphisQuery.OrderByDescending(h => h.MaHp).ToListAsync());
    }

    private async Task PopulateDropdownsAsync(object? selectedHs = null, object? selectedHk = null)
    {
        ViewData["MaHs"] = new SelectList(await context.Hocsinhs.OrderBy(h => h.HoTen).ToListAsync(), "MaHs", "HoTen", selectedHs);
        ViewData["MaHk"] = new SelectList(await context.Hockies.OrderByDescending(h => h.TenHk).ToListAsync(), "MaHk", "TenHk", selectedHk);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateDropdownsAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Hocphi hocphi)
    {
        // Kiểm tra xem học sinh đã có học phí cho học kỳ này chưa
        bool isExisting = await context.Hocphis.AnyAsync(hp =>
            hp.MaHs == hocphi.MaHs &&
            hp.MaHk == hocphi.MaHk);

        if (isExisting)
        {
            ModelState.AddModelError("", "Học sinh này đã được tạo học phí cho học kỳ đã chọn.");
        }

        if (ModelState.IsValid)
        {
            context.Add(hocphi);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        await PopulateDropdownsAsync(hocphi.MaHs, hocphi.MaHk);
        return View(hocphi);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var hocphi = await context.Hocphis.FindAsync(id);
        if (hocphi == null) return NotFound();
        await PopulateDropdownsAsync(hocphi.MaHs, hocphi.MaHk);
        return View(hocphi);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MaHp,MaHs,SoTien,NgayDong,TrangThai,MaHk")] Hocphi hocphi)
    {
        if (id != hocphi.MaHp) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(hocphi);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HocphiExists(hocphi.MaHp)) return NotFound();
                else throw;
            }
        }
        await PopulateDropdownsAsync(hocphi.MaHs, hocphi.MaHk);
        return View(hocphi);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var hocphi = await context.Hocphis
            .Include(h => h.MaHkNavigation)
            .Include(h => h.MaHsNavigation)
            .FirstOrDefaultAsync(m => m.MaHp == id);
        if (hocphi == null) return NotFound();
        return View(hocphi);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var hocphi = await context.Hocphis.FindAsync(id);
        if (hocphi != null)
        {
            context.Hocphis.Remove(hocphi);
            await context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool HocphiExists(int id)
    {
        return context.Hocphis.Any(e => e.MaHp == id);
    }
}