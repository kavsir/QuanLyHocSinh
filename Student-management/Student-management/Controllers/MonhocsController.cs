using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers;

[Authorize(Roles = "Admin")]
public class MonhocsController(QuanLyHocSinhContext context) : Controller
{
    // GET: Monhocs
    public async Task<IActionResult> Index(string? searchString)
    {
        ViewData["CurrentFilter"] = searchString;
        var monhocsQuery = from m in context.Monhocs select m;

        if (!String.IsNullOrEmpty(searchString))
        {
            monhocsQuery = monhocsQuery.Where(s => s.TenMonHoc != null && s.TenMonHoc.Contains(searchString));
        }

        return View(await monhocsQuery.OrderBy(m => m.TenMonHoc).ToListAsync());
    }

    // GET: Monhocs/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var monhoc = await context.Monhocs.FirstOrDefaultAsync(m => m.MaMonHoc == id);
        if (monhoc == null) return NotFound();
        return View(monhoc);
    }

    // GET: Monhocs/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Monhocs/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TenMonHoc,SoTiet,HeSo")] Monhoc monhoc)
    {
        if (ModelState.IsValid)
        {
            var normalizedTenMonHoc = monhoc.TenMonHoc?.Trim().ToLower();
            var existing = await context.Monhocs.FirstOrDefaultAsync(m => m.TenMonHoc != null && m.TenMonHoc.ToLower() == normalizedTenMonHoc);

            if (existing != null)
            {
                ModelState.AddModelError("TenMonHoc", "Tên môn học này đã tồn tại.");
            }
            else
            {
                monhoc.TenMonHoc = monhoc.TenMonHoc?.Trim();
                context.Add(monhoc);
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tạo môn học mới thành công!";
                return RedirectToAction(nameof(Index));
            }
        }
        return View(monhoc);
    }

    // GET: Monhocs/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var monhoc = await context.Monhocs.FindAsync(id);
        if (monhoc == null) return NotFound();
        return View(monhoc);
    }

    // POST: Monhocs/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MaMonHoc,TenMonHoc,SoTiet,HeSo")] Monhoc monhoc)
    {
        if (id != monhoc.MaMonHoc) return NotFound();

        if (ModelState.IsValid)
        {
            var normalizedTenMonHoc = monhoc.TenMonHoc?.Trim().ToLower();
            var existing = await context.Monhocs.AsNoTracking().FirstOrDefaultAsync(m => m.TenMonHoc != null && m.TenMonHoc.ToLower() == normalizedTenMonHoc && m.MaMonHoc != id);

            if (existing != null)
            {
                ModelState.AddModelError("TenMonHoc", "Tên môn học này đã tồn tại.");
            }
            else
            {
                try
                {
                    monhoc.TenMonHoc = monhoc.TenMonHoc?.Trim();
                    context.Update(monhoc);
                    await context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật môn học thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonhocExists(monhoc.MaMonHoc)) return NotFound();
                    else throw;
                }
            }
        }
        return View(monhoc);
    }

    // GET: Monhocs/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var monhoc = await context.Monhocs.FirstOrDefaultAsync(m => m.MaMonHoc == id);
        if (monhoc == null) return NotFound();
        return View(monhoc);
    }

    // POST: Monhocs/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var monhoc = await context.Monhocs.FindAsync(id);
        if (monhoc != null)
        {
            context.Monhocs.Remove(monhoc);
            await context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool MonhocExists(int id)
    {
        return context.Monhocs.Any(e => e.MaMonHoc == id);
    }
}