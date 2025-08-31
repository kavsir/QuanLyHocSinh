using Microsoft.AspNetCore.Mvc;
using Student_management.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Student_management.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MonhocController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public MonhocController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // GET: Monhoc
        public async Task<IActionResult> Index()
        {
            var monhocs = await _context.Monhocs.ToListAsync();
            return View(monhocs);
        }

        // GET: Monhoc/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var monhoc = await _context.Monhocs
                .FirstOrDefaultAsync(m => m.MaMonHoc == id);
            if (monhoc == null) return NotFound();
            return View(monhoc);
        }

        // GET: Monhoc/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Monhoc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Monhoc monhoc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(monhoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(monhoc);
        }

        // GET: Monhoc/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var monhoc = await _context.Monhocs.FindAsync(id);
            if (monhoc == null) return NotFound();
            return View(monhoc);
        }

        // POST: Monhoc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Monhoc monhoc)
        {
            if (id != monhoc.MaMonHoc) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(monhoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Monhocs.Any(e => e.MaMonHoc == monhoc.MaMonHoc))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(monhoc);
        }

        // GET: Monhoc/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var monhoc = await _context.Monhocs
                .FirstOrDefaultAsync(m => m.MaMonHoc == id);
            if (monhoc == null) return NotFound();
            return View(monhoc);
        }

        // POST: Monhoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var monhoc = await _context.Monhocs.FindAsync(id);
            if (monhoc != null)
            {
                _context.Monhocs.Remove(monhoc);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
