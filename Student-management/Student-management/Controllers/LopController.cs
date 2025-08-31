using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers
{
    public class LopController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public LopController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // GET: Lop
        public async Task<IActionResult> Index()
        {
            var lops = _context.Lops
                .Include(l => l.MaNamHocNavigation)
                .Include(l => l.MaGvcnNavigation);
            return View(await lops.ToListAsync());
        }

        // GET: Lop/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var lop = await _context.Lops
                .Include(l => l.MaNamHocNavigation)
                .Include(l => l.MaGvcnNavigation)
                .FirstOrDefaultAsync(m => m.MaLop == id);

            if (lop == null) return NotFound();
            return View(lop);
        }

        // GET: Lop/Create
        public IActionResult Create()
        {
            ViewData["MaNamHoc"] = new SelectList(_context.Namhocs, "MaNamHoc", "TenNamHoc");
            ViewData["MaGvcn"] = new SelectList(_context.Giaoviens, "MaGv", "HoTen");
            return View();
        }

        // POST: Lop/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLop,TenLop,SiSo,MaNamHoc,MaGvcn")] Lop lop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNamHoc"] = new SelectList(_context.Namhocs, "MaNamHoc", "TenNamHoc", lop.MaNamHoc);
            ViewData["MaGvcn"] = new SelectList(_context.Giaoviens, "MaGv", "HoTen", lop.MaGvcn);
            return View(lop);
        }

        // GET: Lop/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var lop = await _context.Lops.FindAsync(id);
            if (lop == null) return NotFound();

            ViewData["MaNamHoc"] = new SelectList(_context.Namhocs, "MaNamHoc", "TenNamHoc", lop.MaNamHoc);
            ViewData["MaGvcn"] = new SelectList(_context.Giaoviens, "MaGv", "HoTen", lop.MaGvcn);
            return View(lop);
        }

        // POST: Lop/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaLop,TenLop,SiSo,MaNamHoc,MaGvcn")] Lop lop)
        {
            if (id != lop.MaLop) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Lops.Any(e => e.MaLop == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaNamHoc"] = new SelectList(_context.Namhocs, "MaNamHoc", "TenNamHoc", lop.MaNamHoc);
            ViewData["MaGvcn"] = new SelectList(_context.Giaoviens, "MaGv", "HoTen", lop.MaGvcn);
            return View(lop);
        }

        // GET: Lop/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var lop = await _context.Lops
                .Include(l => l.MaNamHocNavigation)
                .Include(l => l.MaGvcnNavigation)
                .FirstOrDefaultAsync(m => m.MaLop == id);

            if (lop == null) return NotFound();
            return View(lop);
        }

        // POST: Lop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lop = await _context.Lops.FindAsync(id);
            if (lop != null)
            {
                _context.Lops.Remove(lop);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
