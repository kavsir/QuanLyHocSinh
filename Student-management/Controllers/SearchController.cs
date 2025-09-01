using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace Student_management.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public SearchController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new { success = true, results = new object[0] });
            }

            var lowerCaseQuery = query.ToLower();
            var combinedResults = new List<object>();

            var userRole = HttpContext.Session.GetString("UserRole");
            var taiKhoanIdStr = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userRole) || !int.TryParse(taiKhoanIdStr, out int taiKhoanId))
            {
                return Unauthorized();
            }

            switch (userRole)
            {
                case "Admin":
                    await SearchForAdmin(lowerCaseQuery, combinedResults);
                    break;
                case "GiaoVien":
                    await SearchForGiaoVien(lowerCaseQuery, taiKhoanId, combinedResults);
                    break;
                case "HocSinh":
                    await SearchForHocSinh(lowerCaseQuery, taiKhoanId, combinedResults);
                    break;
            }

            return Json(new { success = true, results = combinedResults });
        }

        private async Task SearchForAdmin(string query, List<object> results)
        {
            var hocsinhResults = await _context.Hocsinhs
                .Where(hs => hs.HoTen.ToLower().Contains(query) || hs.MaHs.ToString().Contains(query))
                .Select(hs => new { id = hs.MaHs, ten = hs.HoTen, loai = "Học sinh", url = Url.Action("Details", "Hocsinh", new { id = hs.MaHs }) })
                .Take(5)
                .ToListAsync();

            var giaovienResults = await _context.Giaoviens
                .Where(gv => gv.HoTen.ToLower().Contains(query) || gv.MaGv.ToString().Contains(query))
                .Select(gv => new { id = gv.MaGv, ten = gv.HoTen, loai = "Giáo viên", url = Url.Action("Details", "Giaovien", new { id = gv.MaGv }) })
                .Take(5)
                .ToListAsync();

            var lopResults = await _context.Lops
                .Where(l => l.TenLop.ToLower().Contains(query))
                .Select(l => new { id = l.MaLop, ten = l.TenLop, loai = "Lớp học", url = Url.Action("Details", "Lop", new { id = l.MaLop }) })
                .Take(5)
                .ToListAsync();

            results.AddRange(hocsinhResults);
            results.AddRange(giaovienResults);
            results.AddRange(lopResults);
        }

        private async Task SearchForGiaoVien(string query, int taiKhoanId, List<object> results)
        {
            var hocsinhResults = await _context.Hocsinhs
                .Where(hs => hs.HoTen.ToLower().Contains(query))
                .Select(hs => new { id = hs.MaHs, ten = hs.HoTen, loai = "Học sinh", url = Url.Action("Details", "Hocsinh", new { id = hs.MaHs }) })
                .Take(5)
                .ToListAsync();

            var giaovienResults = await _context.Giaoviens
                .Where(gv => gv.HoTen.ToLower().Contains(query))
                .Select(gv => new { id = gv.MaGv, ten = gv.HoTen, loai = "Giáo viên", url = Url.Action("Details", "Giaovien", new { id = gv.MaGv }) })
                .Take(5)
                .ToListAsync();

            results.AddRange(hocsinhResults);
            results.AddRange(giaovienResults);
        }

        private async Task SearchForHocSinh(string query, int taiKhoanId, List<object> results)
        {
            var taiKhoanHocSinh = await _context.Taikhoans.FindAsync(taiKhoanId);
            if (taiKhoanHocSinh?.MaHs == null) return;

            var giaovienResults = await _context.Giaoviens
                .Where(gv => gv.HoTen.ToLower().Contains(query))
                .Select(gv => new { id = gv.MaGv, ten = gv.HoTen, loai = "Giáo viên", url = Url.Action("Details", "Giaovien", new { id = gv.MaGv }) })
                .Take(5)
                .ToListAsync();

            var currentStudent = await _context.Hocsinhs.FindAsync(taiKhoanHocSinh.MaHs);
            if (currentStudent?.MaLop != null)
            {
                var hocsinhResults = await _context.Hocsinhs
                    .Where(hs => hs.MaLop == currentStudent.MaLop && hs.HoTen.ToLower().Contains(query))
                    .Select(hs => new { id = hs.MaHs, ten = hs.HoTen, loai = "Bạn cùng lớp", url = Url.Action("Details", "Hocsinh", new { id = hs.MaHs }) })
                    .Take(5)
                    .ToListAsync();
                results.AddRange(hocsinhResults);
            }

            results.AddRange(giaovienResults);
        }
    }
}