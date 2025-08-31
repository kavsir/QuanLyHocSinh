using Microsoft.AspNetCore.Mvc;
using Student_management.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Student_management.Controllers
{
    [Authorize(Roles = "Admin")]   
    public class NamhocController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public NamhocController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

       // GET: Namhoc
        public async Task<IActionResult> Index()
        {
            var namhocs = await _context.Namhocs.ToListAsync();
            return View(namhocs);
        }

        // GET: Namhoc/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Namhoc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Namhoc namhoc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(namhoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(namhoc);
        }

        // GET: Namhoc/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var namhoc = await _context.Namhocs.FindAsync(id);
            if (namhoc == null) return NotFound();
            return View(namhoc);
        }

        // POST: Namhoc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Namhoc namhoc)
        {
            if (id != namhoc.MaNamHoc) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(namhoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Namhocs.Any(e => e.MaNamHoc == namhoc.MaNamHoc))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(namhoc);
        }

        // GET: Namhoc/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var namhoc = await _context.Namhocs.FirstOrDefaultAsync(m => m.MaNamHoc == id);
            if (namhoc == null) return NotFound();
            return View(namhoc);
        }

        // POST: Namhoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var namhoc = await _context.Namhocs.FindAsync(id);
            if (namhoc != null)
            {
                _context.Namhocs.Remove(namhoc);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Namhoc/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var namhoc = await _context.Namhocs.FirstOrDefaultAsync(m => m.MaNamHoc == id);
            if (namhoc == null) return NotFound();
            return View(namhoc);
        }
    }
}
