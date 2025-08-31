using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers
{
    public class HocphiController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public HocphiController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // GET: Hocphi
        public async Task<IActionResult> Index()
        {
            var hocphis = _context.Hocphis
                .Include(h => h.MaHsNavigation)
                .Include(h => h.MaHkNavigation);

            return View(await hocphis.ToListAsync());
        }

        // GET: Hocphi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var hocphi = await _context.Hocphis
                .Include(h => h.MaHsNavigation)
                .Include(h => h.MaHkNavigation)
                .FirstOrDefaultAsync(m => m.MaHp == id);

            if (hocphi == null) return NotFound();

            return View(hocphi);
        }

        // GET: Hocphi/Create
        public IActionResult Create()
        {
            ViewData["MaHs"] = new SelectList(_context.Hocsinhs, "MaHs", "HoTen");
            ViewData["MaHk"] = new SelectList(_context.Hockies, "MaHk", "TenHk");
            return View();
        }

        // POST: Hocphi/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHp,MaHs,SoTien,NgayDong,TrangThai,MaHk")] Hocphi hocphi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hocphi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaHs"] = new SelectList(_context.Hocsinhs, "MaHs", "HoTen", hocphi.MaHs);
            ViewData["MaHk"] = new SelectList(_context.Hockies, "MaHk", "TenHk", hocphi.MaHk);
            return View(hocphi);
        }

        // GET: Hocphi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var hocphi = await _context.Hocphis.FindAsync(id);
            if (hocphi == null) return NotFound();

            ViewData["MaHs"] = new SelectList(_context.Hocsinhs, "MaHs", "HoTen", hocphi.MaHs);
            ViewData["MaHk"] = new SelectList(_context.Hockies, "MaHk", "TenHk", hocphi.MaHk);
            return View(hocphi);
        }

        // POST: Hocphi/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaHp,MaHs,SoTien,NgayDong,TrangThai,MaHk")] Hocphi hocphi)
        {
            if (id != hocphi.MaHp) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hocphi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HocphiExists(hocphi.MaHp)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaHs"] = new SelectList(_context.Hocsinhs, "MaHs", "HoTen", hocphi.MaHs);
            ViewData["MaHk"] = new SelectList(_context.Hockies, "MaHk", "TenHk", hocphi.MaHk);
            return View(hocphi);
        }

        // GET: Hocphi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var hocphi = await _context.Hocphis
                .Include(h => h.MaHsNavigation)
                .Include(h => h.MaHkNavigation)
                .FirstOrDefaultAsync(m => m.MaHp == id);

            if (hocphi == null) return NotFound();

            return View(hocphi);
        }

        // POST: Hocphi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hocphi = await _context.Hocphis.FindAsync(id);
            if (hocphi != null)
            {
                _context.Hocphis.Remove(hocphi);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool HocphiExists(int id)
        {
            return _context.Hocphis.Any(e => e.MaHp == id);
        }
    }
}
