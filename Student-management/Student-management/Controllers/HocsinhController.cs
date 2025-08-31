using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers
{
    public class HocsinhController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public HocsinhController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // GET: Hocsinh
        public async Task<IActionResult> Index()
        {
            var hs = _context.Hocsinhs
                .Include(h => h.MaLopNavigation);
            return View(await hs.ToListAsync());
        }

        // GET: Hocsinh/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var hocsinh = await _context.Hocsinhs
                .Include(h => h.MaLopNavigation)
                .FirstOrDefaultAsync(m => m.MaHs == id);

            if (hocsinh == null) return NotFound();
            return View(hocsinh);
        }
        // GET: Hocsinh/Create
        public IActionResult Create()
        {
            ViewBag.MaLop = new SelectList(_context.Lops, "MaLop", "TenLop");
            return View();
        }

        // POST: Hocsinh/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hocsinh hocsinh)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hocsinh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Log lỗi để biết lý do không insert
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                          .Select(e => e.ErrorMessage)
                                          .ToList();
            Console.WriteLine("Lỗi ModelState: " + string.Join(", ", errors));

            ViewBag.MaLop = new SelectList(_context.Lops, "MaLop", "TenLop", hocsinh.MaLop);
            return View(hocsinh);
        }



        // GET: Hocsinh/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var hocsinh = await _context.Hocsinhs.FindAsync(id);
            if (hocsinh == null) return NotFound();

            ViewBag.MaLop = new SelectList(_context.Lops, "MaLop", "TenLop", hocsinh.MaLop);
            return View(hocsinh);
        }

        // POST: Hocsinh/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Hocsinh hocsinh)
        {
            if (id != hocsinh.MaHs) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hocsinh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Hocsinhs.Any(e => e.MaHs == hocsinh.MaHs))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MaLop = new SelectList(_context.Lops, "MaLop", "TenLop", hocsinh.MaLop);
            return View(hocsinh);
        }

        // GET: Hocsinh/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var hocsinh = await _context.Hocsinhs
                .Include(h => h.MaLopNavigation)
                .FirstOrDefaultAsync(m => m.MaHs == id);
            if (hocsinh == null) return NotFound();

            return View(hocsinh);
        }

        // POST: Hocsinh/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hocsinh = await _context.Hocsinhs.FindAsync(id);
            if (hocsinh != null)
            {
                _context.Hocsinhs.Remove(hocsinh);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
