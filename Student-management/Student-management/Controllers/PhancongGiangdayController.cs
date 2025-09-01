using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers
{
    public class PhancongGiangdayController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public PhancongGiangdayController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // GET: PhancongGiangday
        public async Task<IActionResult> Index()
        {
            var data = _context.PhancongGiangdays
                .Include(p => p.MaGvNavigation)
                .Include(p => p.MaHkNavigation)
                .Include(p => p.MaLopNavigation)
                .Include(p => p.MaMonHocNavigation);
            return View(await data.ToListAsync());
        }

        // GET: PhancongGiangday/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var pc = await _context.PhancongGiangdays
                .Include(p => p.MaGvNavigation)
                .Include(p => p.MaHkNavigation)
                .Include(p => p.MaLopNavigation)
                .Include(p => p.MaMonHocNavigation)
                .FirstOrDefaultAsync(m => m.MaPc == id);
            if (pc == null) return NotFound();

            return View(pc);
        }

        // GET: PhancongGiangday/Create
        public IActionResult Create()
        {
            ViewData["MaGv"] = new SelectList(_context.Giaoviens, "MaGv", "HoTen");
            ViewData["MaHk"] = new SelectList(_context.Hockies, "MaHk", "TenHk");
            ViewData["MaLop"] = new SelectList(_context.Lops, "MaLop", "TenLop");
            ViewData["MaMonHoc"] = new SelectList(_context.Monhocs, "MaMonHoc", "TenMonHoc"); // chỉnh theo model thật
            return View();
        }

        // POST: PhancongGiangday/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaPc,MaGv,MaMonHoc,MaLop,MaHk")] PhancongGiangday pc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaGv"] = new SelectList(_context.Giaoviens, "MaGv", "HoTen", pc.MaGv);
            ViewData["MaHk"] = new SelectList(_context.Hockies, "MaHk", "TenHk", pc.MaHk);
            ViewData["MaLop"] = new SelectList(_context.Lops, "MaLop", "TenLop", pc.MaLop);
            ViewData["MaMonHoc"] = new SelectList(_context.Monhocs, "MaMonHoc", "TenMonHoc", pc.MaMonHoc);
            return View(pc);
        }

        // GET: PhancongGiangday/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var pc = await _context.PhancongGiangdays.FindAsync(id);
            if (pc == null) return NotFound();

            ViewData["MaGv"] = new SelectList(_context.Giaoviens, "MaGv", "HoTen", pc.MaGv);
            ViewData["MaHk"] = new SelectList(_context.Hockies, "MaHk", "TenHk", pc.MaHk);
            ViewData["MaLop"] = new SelectList(_context.Lops, "MaLop", "TenLop", pc.MaLop);
            ViewData["MaMonHoc"] = new SelectList(_context.Monhocs, "MaMonHoc", "TenMonHoc", pc.MaMonHoc);
            return View(pc);
        }

        // POST: PhancongGiangday/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaPc,MaGv,MaMonHoc,MaLop,MaHk")] PhancongGiangday pc)
        {
            if (id != pc.MaPc) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhancongGiangdayExists(pc.MaPc)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaGv"] = new SelectList(_context.Giaoviens, "MaGv", "HoTen", pc.MaGv);
            ViewData["MaHk"] = new SelectList(_context.Hockies, "MaHk", "TenHk", pc.MaHk);
            ViewData["MaLop"] = new SelectList(_context.Lops, "MaLop", "TenLop", pc.MaLop);
            ViewData["MaMonHoc"] = new SelectList(_context.Monhocs, "MaMonHoc", "TenMonHoc", pc.MaMonHoc);
            return View(pc);
        }

        // GET: PhancongGiangday/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var pc = await _context.PhancongGiangdays
                .Include(p => p.MaGvNavigation)
                .Include(p => p.MaHkNavigation)
                .Include(p => p.MaLopNavigation)
                .Include(p => p.MaMonHocNavigation)
                .FirstOrDefaultAsync(m => m.MaPc == id);
            if (pc == null) return NotFound();

            return View(pc);
        }

        // POST: PhancongGiangday/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pc = await _context.PhancongGiangdays.FindAsync(id);
            if (pc != null)
            {
                _context.PhancongGiangdays.Remove(pc);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PhancongGiangdayExists(int id)
        {
            return _context.PhancongGiangdays.Any(e => e.MaPc == id);
        }
    }
}
