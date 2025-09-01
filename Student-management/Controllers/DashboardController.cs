using Microsoft.AspNetCore.Mvc;
using Student_management.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Student_management.Controllers
{
    public class DashboardController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public DashboardController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalStudents = await _context.Hocsinhs.CountAsync();
            var totalTeachers = await _context.Giaoviens.CountAsync();
            var totalClasses = await _context.Lops.CountAsync();
            var totalSubjects = await _context.Monhocs.CountAsync();
            var totalHocPhi = await _context.Hocphis.SumAsync(h => (decimal?)h.SoTien) ?? 0;
            var daDong = await _context.Hocphis.CountAsync(h => h.TrangThai == "Đã đóng");
            var chuaDong = await _context.Hocphis.CountAsync(h => h.TrangThai == "Chưa đóng");

            ViewData["TotalStudents"] = totalStudents;
            ViewData["TotalTeachers"] = totalTeachers;
            ViewData["TotalClasses"] = totalClasses;
            ViewData["TotalSubjects"] = totalSubjects;
            ViewData["TotalHocPhi"] = totalHocPhi;
            ViewData["DaDong"] = daDong;
            ViewData["ChuaDong"] = chuaDong;

            return View();
        }
    }
}
