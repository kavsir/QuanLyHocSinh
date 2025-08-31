using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers
{
    public class TaikhoanController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public TaikhoanController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // GET: Taikhoan
        public async Task<IActionResult> Index()
        {
            var accounts = _context.Taikhoans
                                   .Include(t => t.MaHsNavigation)
                                   .Include(t => t.MaGvNavigation);
            return View(await accounts.ToListAsync());
        }

        // GET: Taikhoan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var taikhoan = await _context.Taikhoans
                .Include(t => t.MaHsNavigation)
                .Include(t => t.MaGvNavigation)
                .FirstOrDefaultAsync(m => m.MaTk == id);

            if (taikhoan == null) return NotFound();

            return View(taikhoan);
        }

        // GET: Taikhoan/Create
        public IActionResult Create()
        {
            // Lấy danh sách học sinh chưa có tài khoản
            var hsChuaCoTk = _context.Hocsinhs
                .Where(h => !_context.Taikhoans.Any(t => t.MaHs == h.MaHs))
                .ToList();

            // Lấy danh sách giáo viên chưa có tài khoản
            var gvChuaCoTk = _context.Giaoviens
                .Where(g => !_context.Taikhoans.Any(t => t.MaGv == g.MaGv))
                .ToList();

            ViewData["MaHs"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(hsChuaCoTk, "MaHs", "HoTen");
            ViewData["MaGv"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(gvChuaCoTk, "MaGv", "HoTen");

            return View();
        }


        // POST: Taikhoan/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaTk,TenDangNhap,MatKhau,VaiTro,MaHs,MaGv")] Taikhoan taikhoan)
        {
            // Kiểm tra tên đăng nhập đã tồn tại chưa
            if (_context.Taikhoans.Any(t => t.TenDangNhap == taikhoan.TenDangNhap))
            {
                ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại, vui lòng chọn tên khác.");
            }

            if (taikhoan.VaiTro == "Học sinh" && taikhoan.MaHs == null)
            {
                ModelState.AddModelError("MaHs", "Tài khoản học sinh phải gắn với 1 học sinh.");
            }

            if (taikhoan.VaiTro == "Giáo viên" && taikhoan.MaGv == null)
            {
                ModelState.AddModelError("MaGv", "Tài khoản giáo viên phải gắn với 1 giáo viên.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(taikhoan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Load lại dropdown khi có lỗi
            var hsChuaCoTk = _context.Hocsinhs
                .Where(h => !_context.Taikhoans.Any(t => t.MaHs == h.MaHs))
                .ToList();
            var gvChuaCoTk = _context.Giaoviens
                .Where(g => !_context.Taikhoans.Any(t => t.MaGv == g.MaGv))
                .ToList();

            ViewData["MaHs"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(hsChuaCoTk, "MaHs", "HoTen", taikhoan.MaHs);
            ViewData["MaGv"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(gvChuaCoTk, "MaGv", "HoTen", taikhoan.MaGv);

            return View(taikhoan);
        }

        // GET: Taikhoan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var taikhoan = await _context.Taikhoans.FindAsync(id);
            if (taikhoan == null) return NotFound();

            ViewData["MaHs"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Hocsinhs, "MaHs", "HoTen", taikhoan.MaHs);
            ViewData["MaGv"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Giaoviens, "MaGv", "HoTen", taikhoan.MaGv);
            return View(taikhoan);
        }

        // POST: Taikhoan/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaTk,TenDangNhap,MatKhau,VaiTro,MaHs,MaGv")] Taikhoan taikhoan)
        {
            if (id != taikhoan.MaTk) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taikhoan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Taikhoans.Any(e => e.MaTk == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaHs"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Hocsinhs, "MaHs", "HoTen", taikhoan.MaHs);
            ViewData["MaGv"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Giaoviens, "MaGv", "HoTen", taikhoan.MaGv);
            return View(taikhoan);
        }

        // GET: Taikhoan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var taikhoan = await _context.Taikhoans
                .Include(t => t.MaHsNavigation)
                .Include(t => t.MaGvNavigation)
                .FirstOrDefaultAsync(m => m.MaTk == id);

            if (taikhoan == null) return NotFound();

            return View(taikhoan);
        }

        // POST: Taikhoan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taikhoan = await _context.Taikhoans.FindAsync(id);
            if (taikhoan != null)
            {
                _context.Taikhoans.Remove(taikhoan);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
