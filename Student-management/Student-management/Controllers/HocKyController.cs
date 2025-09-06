using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;
using System.Threading.Tasks;

namespace Student_management.Controllers
{
    public class HockyController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public HockyController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // GET: Danh sách học kỳ
        public async Task<IActionResult> Index()
        {
            var data = await _context.Hockies
                .Include(h => h.MaNamHocNavigation) // load năm học
                .ToListAsync();
            return View(data);
        }

        // GET: Thêm mới
        public IActionResult Create()
        {
            ViewBag.NamHocs = _context.Namhocs.ToList();
            return View();
        }

        // POST: Thêm mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hocky hk)
        {
            if (hk.NgayKetThuc <= hk.NgayBatDau)
            {
                ModelState.AddModelError("", "Ngày kết thúc phải sau ngày bắt đầu.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.NamHocs = _context.Namhocs.ToList();
                return View(hk);
            }

            _context.Add(hk);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Sửa
        public async Task<IActionResult> Edit(int id)
        {
            var hk = await _context.Hockies.FindAsync(id);
            if (hk == null) return NotFound();

            ViewBag.NamHocs = _context.Namhocs.ToList();
            return View(hk);
        }

        // POST: Sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Hocky hk)
        {
            if (id != hk.MaHk) return NotFound();

            if (hk.NgayKetThuc <= hk.NgayBatDau)
            {
                ModelState.AddModelError("", "Ngày kết thúc phải sau ngày bắt đầu.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.NamHocs = _context.Namhocs.ToList();
                return View(hk);
            }

            try
            {
                _context.Update(hk);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Hockies.Any(e => e.MaHk == id))
                    return NotFound();
                else throw;
            }
        }

        // GET: Xóa
        public async Task<IActionResult> Delete(int id)
        {
            var hk = await _context.Hockies
                .Include(h => h.MaNamHocNavigation)
                .FirstOrDefaultAsync(h => h.MaHk == id);

            if (hk == null) return NotFound();
            return View(hk);
        }

        // POST: Xóa
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hk = await _context.Hockies.FindAsync(id);
            if (hk == null) return NotFound();

            try
            {
                _context.Hockies.Remove(hk);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                // nếu bị lỗi FK → redirect sang trang báo lỗi chung
                return RedirectToAction("DeleteError", "Error",
                    new { entityName = "Học kỳ", id = id, returnController = "Hocky" });
            }
        }
    }
}
