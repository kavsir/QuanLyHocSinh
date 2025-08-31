using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers
{
    public class LichhocController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public LichhocController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // GET: Lichhoc
        public async Task<IActionResult> Index()
        {
            var data = _context.Lichhocs
                .Include(l => l.MaGvNavigation)
                .Include(l => l.MaHkNavigation)
                .Include(l => l.MaLopNavigation)
                .Include(l => l.MaMonHocNavigation)
                .Include(l => l.MaPhongNavigation);
            return View(await data.ToListAsync());
        }

        // GET: Lichhoc/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var lichhoc = await _context.Lichhocs
                .Include(l => l.MaGvNavigation)
                .Include(l => l.MaHkNavigation)
                .Include(l => l.MaLopNavigation)
                .Include(l => l.MaMonHocNavigation)
                .Include(l => l.MaPhongNavigation)
                .FirstOrDefaultAsync(m => m.MaLichHoc == id);

            if (lichhoc == null) return NotFound();

            return View(lichhoc);
        }

        // GET: Lichhoc/Create
        public IActionResult Create()
        {
            ViewData["MaGv"] = new SelectList(_context.Giaoviens, "MaGv", "HoTen");
            ViewData["MaHk"] = new SelectList(_context.Hockies, "MaHk", "TenHk");
            ViewData["MaLop"] = new SelectList(_context.Lops, "MaLop", "TenLop");
            ViewData["MaMonHoc"] = new SelectList(_context.Monhocs, "MaMonHoc", "TenMonHoc");
            ViewData["MaPhong"] = new SelectList(_context.Phonghocs, "MaPhong", "TenPhong");
            return View();
        }

        // POST: Lichhoc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLichHoc,MaLop,MaMonHoc,MaGv,MaPhong,MaHk,ThuTrongTuan,TietHoc")] Lichhoc lichhoc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lichhoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaGv"] = new SelectList(_context.Giaoviens, "MaGv", "HoTen", lichhoc.MaGv);
            ViewData["MaHk"] = new SelectList(_context.Hockies, "MaHk", "TenHk", lichhoc.MaHk);
            ViewData["MaLop"] = new SelectList(_context.Lops, "MaLop", "TenLop", lichhoc.MaLop);
            ViewData["MaMonHoc"] = new SelectList(_context.Monhocs, "MaMonHoc", "TenMonHoc", lichhoc.MaMonHoc);
            ViewData["MaPhong"] = new SelectList(_context.Phonghocs, "MaPhong", "TenPhong", lichhoc.MaPhong);
            return View(lichhoc);
        }

        // GET: Lichhoc/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var lichhoc = await _context.Lichhocs.FindAsync(id);
            if (lichhoc == null) return NotFound();

            ViewData["MaGv"] = new SelectList(_context.Giaoviens, "MaGv", "HoTen", lichhoc.MaGv);
            ViewData["MaHk"] = new SelectList(_context.Hockies, "MaHk", "TenHk", lichhoc.MaHk);
            ViewData["MaLop"] = new SelectList(_context.Lops, "MaLop", "TenLop", lichhoc.MaLop);
            ViewData["MaMonHoc"] = new SelectList(_context.Monhocs, "MaMonHoc", "TenMonHoc", lichhoc.MaMonHoc);
            ViewData["MaPhong"] = new SelectList(_context.Phonghocs, "MaPhong", "TenPhong", lichhoc.MaPhong);
            return View(lichhoc);
        }

        // POST: Lichhoc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaLichHoc,MaLop,MaMonHoc,MaGv,MaPhong,MaHk,ThuTrongTuan,TietHoc")] Lichhoc lichhoc)
        {
            if (id != lichhoc.MaLichHoc) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lichhoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LichhocExists(lichhoc.MaLichHoc))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaGv"] = new SelectList(_context.Giaoviens, "MaGv", "HoTen", lichhoc.MaGv);
            ViewData["MaHk"] = new SelectList(_context.Hockies, "MaHk", "TenHk", lichhoc.MaHk);
            ViewData["MaLop"] = new SelectList(_context.Lops, "MaLop", "TenLop", lichhoc.MaLop);
            ViewData["MaMonHoc"] = new SelectList(_context.Monhocs, "MaMonHoc", "TenMonHoc", lichhoc.MaMonHoc);
            ViewData["MaPhong"] = new SelectList(_context.Phonghocs, "MaPhong", "TenPhong", lichhoc.MaPhong);
            return View(lichhoc);
        }

        // GET: Lichhoc/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var lichhoc = await _context.Lichhocs
                .Include(l => l.MaGvNavigation)
                .Include(l => l.MaHkNavigation)
                .Include(l => l.MaLopNavigation)
                .Include(l => l.MaMonHocNavigation)
                .Include(l => l.MaPhongNavigation)
                .FirstOrDefaultAsync(m => m.MaLichHoc == id);
            if (lichhoc == null) return NotFound();

            return View(lichhoc);
        }

        // POST: Lichhoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lichhoc = await _context.Lichhocs.FindAsync(id);
            if (lichhoc != null)
            {
                _context.Lichhocs.Remove(lichhoc);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool LichhocExists(int id)
        {
            return _context.Lichhocs.Any(e => e.MaLichHoc == id);
        }
    }
}
