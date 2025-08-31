using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers
{
    public class DiemController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public DiemController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // GET: Diem
        public async Task<IActionResult> Index()
        {
            var diems = _context.Diems
                .Include(d => d.MaHsNavigation)
                .Include(d => d.MaMonHocNavigation)
                .Include(d => d.MaHkNavigation);
            return View(await diems.ToListAsync());
        }

        // GET: Diem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var diem = await _context.Diems
                .Include(d => d.MaHsNavigation)
                .Include(d => d.MaMonHocNavigation)
                .Include(d => d.MaHkNavigation)
                .FirstOrDefaultAsync(m => m.MaDiem == id);
            if (diem == null) return NotFound();

            return View(diem);
        }

        // GET: Diem/Create
        public IActionResult Create()
        {
            ViewData["MaHs"] = new SelectList(_context.Hocsinhs, "MaHs", "HoTen");
            ViewData["MaMonHoc"] = new SelectList(_context.Monhocs, "MaMonHoc", "TenMonHoc");
            ViewData["MaHk"] = new SelectList(_context.Hockies, "MaHk", "TenHk");
            return View();
        }

        // POST: Diem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDiem,MaHs,MaMonHoc,MaHk,DiemMieng,Diem15p,Diem1Tiet,DiemThi")] Diem diem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(diem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaHs"] = new SelectList(_context.Hocsinhs, "MaHs", "HoTen", diem.MaHs);
            ViewData["MaMonHoc"] = new SelectList(_context.Monhocs, "MaMonHoc", "TenMonHoc", diem.MaMonHoc);
            ViewData["MaHk"] = new SelectList(_context.Hockies, "MaHk", "TenHk", diem.MaHk);
            return View(diem);
        }

        // GET: Diem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var diem = await _context.Diems.FindAsync(id);
            if (diem == null) return NotFound();

            ViewData["MaHs"] = new SelectList(_context.Hocsinhs, "MaHs", "HoTen", diem.MaHs);
            ViewData["MaMonHoc"] = new SelectList(_context.Monhocs, "MaMonHoc", "TenMonHoc", diem.MaMonHoc);
            ViewData["MaHk"] = new SelectList(_context.Hockies, "MaHk", "TenHk", diem.MaHk);
            return View(diem);
        }

        // POST: Diem/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDiem,MaHs,MaMonHoc,MaHk,DiemMieng,Diem15p,Diem1Tiet,DiemThi")] Diem diem)
        {
            if (id != diem.MaDiem) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Diems.Any(e => e.MaDiem == diem.MaDiem)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaHs"] = new SelectList(_context.Hocsinhs, "MaHs", "HoTen", diem.MaHs);
            ViewData["MaMonHoc"] = new SelectList(_context.Monhocs, "MaMonHoc", "TenMonHoc", diem.MaMonHoc);
            ViewData["MaHk"] = new SelectList(_context.Hockies, "MaHk", "TenHk", diem.MaHk);
            return View(diem);
        }

        // GET: Diem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var diem = await _context.Diems
                .Include(d => d.MaHsNavigation)
                .Include(d => d.MaMonHocNavigation)
                .Include(d => d.MaHkNavigation)
                .FirstOrDefaultAsync(m => m.MaDiem == id);
            if (diem == null) return NotFound();

            return View(diem);
        }

        // POST: Diem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var diem = await _context.Diems.FindAsync(id);
            if (diem != null)
            {
                _context.Diems.Remove(diem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
