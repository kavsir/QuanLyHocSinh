using Microsoft.AspNetCore.Mvc;
using Student_management.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Student_management.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HockyController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public HockyController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // GET: Hocky
        public async Task<IActionResult> Index()
        {
            var hockies = await _context.Hockies.Include(h => h.MaNamHocNavigation).ToListAsync();
            return View(hockies);
        }

        // GET: Hocky/Create
        public IActionResult Create()
        {
            ViewBag.NamHocs = _context.Namhocs.ToList();
            return View();
        }

        // POST: Hocky/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hocky hocky)
        {
            if (!ModelState.IsValid)
            {
                // Xem lỗi
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var e in errors)
                    Console.WriteLine(e.ErrorMessage);

                ViewBag.NamHocs = _context.Namhocs.ToList();
                return View(hocky);
            }

            _context.Hockies.Add(hocky);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Hocky/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var hocky = await _context.Hockies.FindAsync(id);
            if (hocky == null) return NotFound();
            ViewBag.NamHocs = _context.Namhocs.ToList();
            return View(hocky);
        }

        // POST: Hocky/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Hocky hocky)
        {
            if (id != hocky.MaHk) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hocky);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Hockies.Any(e => e.MaHk == hocky.MaHk))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.NamHocs = _context.Namhocs.ToList();
            return View(hocky);
        }

        // GET: Hocky/Delete/5
        // GET: Hocky/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var hocky = await _context.Hockies
                .Include(h => h.MaNamHocNavigation)
                .FirstOrDefaultAsync(m => m.MaHk == id);

            if (hocky == null) return NotFound();
            return View(hocky);
        }

        // POST: Hocky/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hocky = await _context.Hockies.FindAsync(id);
            if (hocky != null)
            {
                _context.Hockies.Remove(hocky);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        // GET: Hocky/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var hocky = await _context.Hockies.Include(h => h.MaNamHocNavigation)
                .FirstOrDefaultAsync(m => m.MaHk == id);
            if (hocky == null) return NotFound();
            return View(hocky);
        }
    }
}
