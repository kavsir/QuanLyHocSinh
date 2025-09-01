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
            try
            {
                _context.Add(diem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDiem,MaHs,MaMonHoc,MaHk,DiemMieng,Diem15p,Diem1Tiet,DiemThi")] Diem diem)
        {
            if (id != diem.MaDiem)
                return NotFound();

            // Lấy entity hiện tại từ DB
            var diemToUpdate = await _context.Diems.FindAsync(id);
            if (diemToUpdate == null)
                return NotFound();

            // Gán giá trị mới
            diemToUpdate.MaHs = diem.MaHs;
            diemToUpdate.MaMonHoc = diem.MaMonHoc;
            diemToUpdate.MaHk = diem.MaHk;
            diemToUpdate.DiemMieng = diem.DiemMieng;
            diemToUpdate.Diem15p = diem.Diem15p;
            diemToUpdate.Diem1Tiet = diem.Diem1Tiet;
            diemToUpdate.DiemThi = diem.DiemThi;

            try
            {
                await _context.SaveChangesAsync(); // EF sẽ gửi UPDATE
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
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
