using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;
 
namespace Student_management.Controllers
{
    [Route("Diemdanhs")]
    public class DiemdanhsController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public DiemdanhsController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // GET: Diemdanhs/Index?lopId=1
        public async Task<IActionResult> Index(int lopId, string? searchString, DateTime? ngay, string? trangThai)
        {
            var query = _context.Diemdanhs
                .Include(d => d.MaHsNavigation)
                .Include(d => d.MaLopNavigation)
                .Where(d => d.MaLop == lopId);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(d => d.MaHsNavigation != null && d.MaHsNavigation.HoTen.Contains(searchString));
            }
            if (ngay.HasValue)
            {
                    query = query.Where(d => d.NgayDiemDanh == DateOnly.FromDateTime(ngay.Value));
            }
            if (!string.IsNullOrEmpty(trangThai))
            {
                query = query.Where(d => d.TrangThai == trangThai);
            }

            var danhSach = await query
                .OrderByDescending(d => d.NgayDiemDanh)
                .ThenBy(d => d.TietHoc)
                .ToListAsync();

            ViewBag.TenLop = _context.Lops.FirstOrDefault(l => l.MaLop == lopId)?.TenLop ?? "";
            ViewBag.LopId = lopId;
            ViewBag.SearchString = searchString;
            ViewBag.Ngay = ngay?.ToString("yyyy-MM-dd");
            ViewBag.TrangThai = trangThai;

            return View(danhSach);
        }

        // GET: Diemdanhs/TakeAttendance?lopId=1
        [HttpGet("TakeAttendance")]
        public async Task<IActionResult> TakeAttendance(int lopId)
        {
            var lop = await _context.Lops
                                    .Include(l => l.Hocsinhs)
                                    .FirstOrDefaultAsync(l => l.MaLop == lopId);
            if (lop == null) return NotFound();

            var vm = new AttendanceViewModel
            {
                LopId = lop.MaLop,
                TenLop = lop.TenLop ?? "",
                NgayDiemDanh = DateTime.Now,
                TietHoc = 1,
                AttendanceList = lop.Hocsinhs.Select(hs => new AttendanceItem
                {
                    MaHs = hs.MaHs,
                    HoTenHs = hs.HoTen ?? ""
                }).ToList()
            };

            return View(vm);
        }

        // POST: Diemdanhs/TakeAttendance
        [HttpPost("TakeAttendance")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TakeAttendance(AttendanceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            foreach (var item in model.AttendanceList)
            {
                var existing = await _context.Diemdanhs.FirstOrDefaultAsync(d =>
                    d.MaHs == item.MaHs &&
                    d.MaLop == model.LopId &&
                    d.NgayDiemDanh == DateOnly.FromDateTime(model.NgayDiemDanh) &&
                    d.TietHoc == model.TietHoc);

                if (existing != null)
                {
                    // Cập nhật
                    existing.TrangThai = item.TrangThai ?? "Có mặt";
                    existing.GhiChu = item.GhiChu;
                }
                else
                {
                    // Thêm mới
                    var diemDanh = new Diemdanh
                    {
                        MaHs = item.MaHs,
                        MaLop = model.LopId,
                        NgayDiemDanh = DateOnly.FromDateTime(model.NgayDiemDanh),
                        TietHoc = model.TietHoc,
                        TrangThai = item.TrangThai ?? "Có mặt",
                        GhiChu = item.GhiChu
                    };
                    _context.Diemdanhs.Add(diemDanh);
                }
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = "Điểm danh thành công!";
            return RedirectToAction("Index", new { lopId = model.LopId });
        }
    }
}
