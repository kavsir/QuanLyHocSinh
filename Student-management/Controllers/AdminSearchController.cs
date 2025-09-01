using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student_management.Controllers
{
    public class AdminSearchController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public AdminSearchController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // GET: /AdminSearch?query=...
        public async Task<IActionResult> Index(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new { success = false });
            }

            // Chuyển từ khóa tìm kiếm về chữ thường
            var lowerCaseQuery = query.ToLower();
            var results = new List<object>();

            // 1. Tìm kiếm Học sinh (theo Tên hoặc Mã HS)
            var hocsinhs = await _context.Hocsinhs
                // Chuyển cả dữ liệu trong database về chữ thường trước khi so sánh
                .Where(hs => hs.HoTen.ToLower().Contains(lowerCaseQuery) || hs.MaHs.ToString().Contains(lowerCaseQuery))
                .Select(hs => new {
                    Ten = hs.HoTen,
                    Loai = "Học sinh",
                    Url = Url.Action("Details", "Hocsinh", new { id = hs.MaHs })
                })
                .Take(5)
                .ToListAsync();
            results.AddRange(hocsinhs);

            // 2. Tìm kiếm Giáo viên (theo Tên hoặc Mã GV)
            var giaoviens = await _context.Giaoviens
                .Where(gv => gv.HoTen.ToLower().Contains(lowerCaseQuery) || gv.MaGv.ToString().Contains(lowerCaseQuery))
                .Select(gv => new {
                    Ten = gv.HoTen,
                    Loai = "Giáo viên",
                    Url = Url.Action("Details", "Giaovien", new { id = gv.MaGv })
                })
                .Take(5)
                .ToListAsync();
            results.AddRange(giaoviens);

            // 3. Tìm kiếm Lớp học (theo Tên lớp)
            var lops = await _context.Lops
                .Where(l => l.TenLop.ToLower().Contains(lowerCaseQuery))
                .Select(l => new {
                    Ten = l.TenLop,
                    Loai = "Lớp học",
                    Url = Url.Action("Details", "Lop", new { id = l.MaLop })
                })
                .Take(5)
                .ToListAsync();
            results.AddRange(lops);

            // 4. Tìm kiếm Môn học (theo Tên môn)
            var monhocs = await _context.Monhocs
                .Where(m => m.TenMonHoc.ToLower().Contains(lowerCaseQuery))
                .Select(m => new {
                    Ten = m.TenMonHoc,
                    Loai = "Môn học",
                    Url = Url.Action("Details", "Monhoc", new { id = m.MaMonHoc })
                })
                .Take(5)
                .ToListAsync();
            results.AddRange(monhocs);

            // 5. Tìm kiếm Năm học (theo Tên năm học)
            var namhocs = await _context.Namhocs
                .Where(nh => nh.TenNamHoc.ToLower().Contains(lowerCaseQuery))
                .Select(nh => new {
                    Ten = nh.TenNamHoc,
                    Loai = "Năm học",
                    Url = Url.Action("Index", "Namhoc")
                })
                .Take(3)
                .ToListAsync();
            results.AddRange(namhocs);

            // 6. Tìm kiếm Học kỳ (theo Tên học kỳ)
            var hockys = await _context.Hockies
                .Where(hk => hk.TenHk.ToLower().Contains(lowerCaseQuery))
                .Select(hk => new {
                    Ten = hk.TenHk,
                    Loai = "Học kỳ",
                    Url = Url.Action("Index", "Hocky")
                })
                .Take(3)
                .ToListAsync();
            results.AddRange(hockys);

            // 7. Tìm kiếm Phòng học (theo Tên phòng)
            var phonghocs = await _context.Phonghocs
                .Where(ph => ph.TenPhong.ToLower().Contains(lowerCaseQuery))
                .Select(ph => new {
                    Ten = ph.TenPhong,
                    Loai = "Phòng học",
                    Url = Url.Action("Index", "Phonghoc")
                })
                .Take(3)
                .ToListAsync();
            results.AddRange(phonghocs);

            // 8. Tìm kiếm Tài khoản (theo Tên đăng nhập)
            var taikhoans = await _context.Taikhoans
                .Where(tk => tk.TenDangNhap.ToLower().Contains(lowerCaseQuery))
                .Select(tk => new {
                    Ten = tk.TenDangNhap,
                    Loai = "Tài khoản",
                    Url = Url.Action("Details", "Taikhoan", new { id = tk.MaTk })
                })
                .Take(3)
                .ToListAsync();
            results.AddRange(taikhoans);

            return Json(new { success = true, data = results });
        }
    }
}