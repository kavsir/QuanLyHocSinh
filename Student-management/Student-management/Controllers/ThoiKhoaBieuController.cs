using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;
using System.Security.Claims;

namespace Student_management.Controllers
{
    public class ThoiKhoaBieuController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public ThoiKhoaBieuController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // Học sinh vào xem trực tiếp TKB của mình
        public async Task<IActionResult> Index()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (role == "HocSinh")
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int maHs = int.Parse(userId);

                var hocSinh = await _context.Hocsinhs
                    .Include(h => h.MaLopNavigation)
                    .FirstOrDefaultAsync(h => h.MaHs == maHs);

                if (hocSinh == null) return NotFound("Không tìm thấy học sinh.");

                var lichHoc = await _context.Lichhocs
                    .Include(l => l.MaMonHocNavigation)
                    .Include(l => l.MaGvNavigation)
                    .Include(l => l.MaPhongNavigation)
                    .Where(l => l.MaLop == hocSinh.MaLop)
                    .ToListAsync();

                ViewBag.TenLop = hocSinh.MaLopNavigation.TenLop;
                return View("XemTKB", lichHoc);
            }
            else if (role == "Admin")
            {
                // Admin chuyển sang trang chọn lớp
                var dsLop = await _context.Lops.ToListAsync();
                return View("ChonLop", dsLop);
            }

            return RedirectToAction("Login", "Account");
        }

        // Admin chọn lớp để xem
        public async Task<IActionResult> XemTheoLop(int id)
        {
            var lop = await _context.Lops.FindAsync(id);
            if (lop == null) return NotFound();

            var lichHoc = await _context.Lichhocs
                .Include(l => l.MaMonHocNavigation)
                .Include(l => l.MaGvNavigation)
                .Include(l => l.MaPhongNavigation)
                .Where(l => l.MaLop == id)
                .ToListAsync();

            ViewBag.TenLop = lop.TenLop;
            return View("XemTKB", lichHoc);
        }
    }
}
