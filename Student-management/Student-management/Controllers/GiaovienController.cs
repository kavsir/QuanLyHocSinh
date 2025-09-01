using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers
{
    public class GiaovienController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public GiaovienController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // GET: Giaovien
        public async Task<IActionResult> Index()
        {
            var giaovien = _context.Giaoviens.Include(g => g.MaMonHocNavigation);
            return View(await giaovien.ToListAsync());
        }

        // GET: Giaovien/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var gv = await _context.Giaoviens
                        .Include(g => g.MaMonHocNavigation)
                        .FirstOrDefaultAsync(g => g.MaGv == id);
            if (gv == null) return NotFound();
            return View(gv);
        }

        // GET: Giaovien/Create
        public IActionResult Create()
        {
            ViewData["MaMonHoc"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Monhocs, "MaMonHoc", "TenMonHoc");
            return View();
        }

        // POST: Giaovien/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Giaovien giaovien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giaovien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaMonHoc"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Monhocs, "MaMonHoc", "TenMonHoc", giaovien.MaMonHoc);
            return View(giaovien);
        }

        // GET: Giaovien/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var gv = await _context.Giaoviens.FindAsync(id);
            if (gv == null) return NotFound();
            ViewData["MaMonHoc"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Monhocs, "MaMonHoc", "TenMonHoc", gv.MaMonHoc);
            return View(gv);
        }

        // POST: Giaovien/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Giaovien giaovien)
        {
            if (id != giaovien.MaGv) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giaovien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Giaoviens.Any(e => e.MaGv == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaMonHoc"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Monhocs, "MaMonHoc", "TenMonHoc", giaovien.MaMonHoc);
            return View(giaovien);
        }

        // GET: Giaovien/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var gv = await _context.Giaoviens
                        .Include(g => g.MaMonHocNavigation)
                        .FirstOrDefaultAsync(g => g.MaGv == id);
            if (gv == null) return NotFound();
            return View(gv);
        }

        // POST: Giaovien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gv = await _context.Giaoviens.FindAsync(id);
            if (gv != null)
            {
                _context.Giaoviens.Remove(gv);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
