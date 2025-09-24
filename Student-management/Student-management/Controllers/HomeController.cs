using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Student_management.Controllers
{
    public class HomeController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public HomeController(QuanLyHocSinhContext context)
        {
            _context = context;
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public async Task<IActionResult> Index(int? selectedNamHocId, int? selectedHocKyId)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            // ============================
            // === XỬ LÝ CHO VAI TRÒ ADMIN ===
            // ============================
            if (role == "Admin")
            {
                var adminViewModel = new AdminDashboardViewModel();
                var namHocList = await _context.Namhocs.OrderByDescending(n => n.NgayBatDau).ToListAsync();
                adminViewModel.NamHocOptions = namHocList.Select(n => new SelectListItem
                {
                    Value = n.MaNamHoc.ToString(),
                    Text = n.TenNamHoc
                }).ToList();

                int activeNamHocId = selectedNamHocId ?? (namHocList.Any() ? namHocList.First().MaNamHoc : 0);
                adminViewModel.SelectedNamHocId = activeNamHocId;

                if (activeNamHocId > 0)
                {
                    adminViewModel.TongHocSinh = await _context.Hocsinhs
                        .CountAsync(h => h.MaLopNavigation != null && h.MaLopNavigation.MaNamHoc == activeNamHocId);
                    adminViewModel.TongLopHoc = await _context.Lops.CountAsync(l => l.MaNamHoc == activeNamHocId);

                    var siSoLop = await _context.Lops.AsNoTracking()
                        .Where(l => l.MaNamHoc == activeNamHocId)
                        .Include(l => l.Hocsinhs)
                        .OrderBy(l => l.TenLop)
                        .Select(l => new { TenLop = l.TenLop ?? "N/A", SiSo = l.Hocsinhs.Count })
                        .ToListAsync();
                    adminViewModel.TenLop = siSoLop.Select(s => s.TenLop).ToList();
                    adminViewModel.SiSoMoiLop = siSoLop.Select(s => s.SiSo).ToList();

                    adminViewModel.LopChuaCoGvcn = await _context.Lops
                        .CountAsync(l => l.MaGvcn == null && l.MaNamHoc == activeNamHocId);
                }

                adminViewModel.TongGiaoVien = await _context.Giaoviens.CountAsync();
                if (adminViewModel.TongHocSinh > 0)
                {
                    adminViewModel.TiLeGvHs = (double)adminViewModel.TongGiaoVien / adminViewModel.TongHocSinh;
                }

                var hocsinhChuaCoTk = await _context.Hocsinhs.CountAsync(h => !h.Taikhoans.Any());
                var giaovienChuaCoTk = await _context.Giaoviens.CountAsync(g => !g.Taikhoans.Any());
                adminViewModel.TaiKhoanCanTao = hocsinhChuaCoTk + giaovienChuaCoTk;

                adminViewModel.HocSinhMoiNhat = await _context.Hocsinhs
                    .Where(h => h.MaLopNavigation != null && h.MaLopNavigation.MaNamHoc == activeNamHocId)
                    .OrderByDescending(h => h.MaHs)
                    .Take(5)
                    .Include(h => h.MaLopNavigation)
                    .ToListAsync();

                return View(adminViewModel);
            }
            // ===============================
            // === XỬ LÝ CHO VAI TRÒ GIÁO VIÊN === (Đã nâng cấp)
            // ===============================
            else if (role == "GiaoVien")
            {
                var username = User.Identity?.Name;
                var taikhoan = await _context.Taikhoans.FirstOrDefaultAsync(tk => tk.TenDangNhap == username);
                if (taikhoan == null || taikhoan.MaGv == null)
                {
                    return RedirectToAction("Logout", "Account");
                }
                var gvId = taikhoan.MaGv.Value;

                var teacher = await _context.Giaoviens.FindAsync(gvId);
                var teacherViewModel = new TeacherDashboardViewModel
                {
                    HoTenGiaoVien = teacher?.HoTen ?? "Không tìm thấy tên"
                };

                var namHocList = await _context.Namhocs.OrderByDescending(n => n.NgayBatDau).ToListAsync();
                var today = DateTime.Now.Date;

                // <<< LOGIC MỚI: TÌM NĂM HỌC HIỆN TẠI DỰA VÀO NGÀY >>>
                var activeNamHoc = await _context.Namhocs
                    .FirstOrDefaultAsync(n => n.NgayBatDau.Date <= today && n.NgayKetThuc.Date >= today);

                // Nếu không tìm thấy năm học nào đang diễn ra, thì mới lấy năm học mới nhất
                if (activeNamHoc == null)
                {
                    activeNamHoc = namHocList.FirstOrDefault();
                }

                // Ưu tiên năm học được chọn từ dropdown, nếu không thì dùng năm học hiện tại
                int activeNamHocId = selectedNamHocId ?? (activeNamHoc?.MaNamHoc ?? 0);

                // Cập nhật lại Dropdown để hiển thị đúng năm học đang được chọn
                ViewBag.NamHocOptions = namHocList.Select(n => new SelectListItem
                {
                    Value = n.MaNamHoc.ToString(),
                    Text = n.TenNamHoc,
                    Selected = n.MaNamHoc == activeNamHocId // <<< Highlight đúng năm đang xem
                }).ToList();


                // <<< LOGIC MỚI: TÌM HỌC KỲ HIỆN TẠI DỰA VÀO NGÀY >>>
                var currentHk = await _context.Hockies
                    .Where(h => h.MaNamHoc == activeNamHocId && h.NgayBatDau.Date <= today && h.NgayKetThuc.Date >= today)
                    .FirstOrDefaultAsync();

                // Fallback: Nếu không có học kỳ nào đang diễn ra, lấy học kỳ gần nhất trong năm
                if (currentHk == null)
                {
                    currentHk = await _context.Hockies
                    .Where(h => h.MaNamHoc == activeNamHocId)
                    .OrderBy(h => h.NgayBatDau) // <<< SỬA THÀNH OrderBy
                    .FirstOrDefaultAsync();
                }


                if (currentHk != null)
                {
                    var assignments = await _context.PhancongGiangdays
                        .Where(p => p.MaGv == gvId && p.MaHk == currentHk.MaHk)
                        .Include(p => p.MaLopNavigation)
                        .Include(p => p.MaMonHocNavigation)
                        .ToListAsync();

                    teacherViewModel.CacLopDuocPhanCong = assignments.Select(a => new AssignedClassInfo
                    {
                        MaLop = a.MaLop ?? 0,
                        TenLop = a.MaLopNavigation?.TenLop ?? "N/A",
                        MaMonHoc = a.MaMonHoc ?? 0,
                        TenMonHoc = a.MaMonHocNavigation?.TenMonHoc ?? "N/A",
                        SiSo = a.MaLopNavigation?.SiSo ?? 0
                    }).ToList();

                    teacherViewModel.TongSoLopDangDay = teacherViewModel.CacLopDuocPhanCong.Count;
                    teacherViewModel.TongSoHocSinhPhuTrach = teacherViewModel.CacLopDuocPhanCong.Sum(c => c.SiSo);

                    string todayInVietnamese = GetTodayInVietnamese();
                    teacherViewModel.LichDayHomNay = await _context.Lichhocs
                        .Where(l => l.MaGv == gvId && l.MaHk == currentHk.MaHk && l.ThuTrongTuan == todayInVietnamese)
                        .Include(l => l.MaLopNavigation)
                        .Include(l => l.MaMonHocNavigation)
                        .OrderBy(l => l.TietHoc)
                        .ToListAsync();
                }
                else
                {
                    ViewBag.ErrorMessage = "Không tìm thấy học kỳ phù hợp cho năm học đã chọn.";
                }

                return View(teacherViewModel);
            }
            // =======================================================
            // === BỔ SUNG KHỐI LOGIC NÀY VÀO ACTION INDEX CỦA BẠN ===
            // =======================================================
            else if (role == "HocSinh")
            {
                var username = User.Identity?.Name;
                var taikhoan = await _context.Taikhoans.FirstOrDefaultAsync(tk => tk.TenDangNhap == username);
                if (taikhoan?.MaHs == null)
                {
                    // Nếu không tìm thấy tài khoản hoặc tài khoản không liên kết với học sinh
                    return RedirectToAction("Logout", "Account");
                }

                var hsId = taikhoan.MaHs.Value;

                var student = await _context.Hocsinhs
                    .Include(s => s.MaLopNavigation)
                        .ThenInclude(l => l!.MaGvcnNavigation)
                    .FirstOrDefaultAsync(s => s.MaHs == hsId);

                // Xử lý trường hợp học sinh chưa được xếp lớp
                if (student == null || student.MaLopNavigation == null)
                {
                    ViewData["ErrorMessage"] = "Thông tin của bạn chưa được cập nhật đầy đủ (chưa có lớp học). Vui lòng liên hệ với giáo vụ.";
                    return View("StudentPortalError");
                }

                var viewModel = new StudentPortalViewModel { StudentInfo = student };

                // Lấy danh sách học kỳ của năm học mà học sinh đang học
                var hockyList = await _context.Hockies
                    .Where(h => h.MaNamHoc == student.MaLopNavigation.MaNamHoc)
                    .OrderByDescending(h => h.TenHk)
                    .ToListAsync();

                viewModel.HocKyOptions = hockyList.Select(h => new SelectListItem { Value = h.MaHk.ToString(), Text = h.TenHk }).ToList();

                // Xác định học kỳ đang được chọn để hiển thị dữ liệu
                int activeHocKyId = selectedHocKyId ?? (hockyList.Any() ? hockyList.First().MaHk : 0);
                viewModel.SelectedHocKyId = activeHocKyId;

                if (activeHocKyId > 0)
                {
                    // Lấy Bảng điểm
                    viewModel.Grades = await _context.Diems
                        .Where(d => d.MaHs == hsId && d.MaHk == activeHocKyId)
                        .Include(d => d.MaMonHocNavigation)
                        .Select(d => new GradeItem
                        {
                            TenMonHoc = d.MaMonHocNavigation!.TenMonHoc ?? "",
                            DiemMieng = d.DiemMieng,
                            Diem15p = d.Diem15p,
                            Diem1Tiet = d.Diem1Tiet,
                            DiemThi = d.DiemThi,
                            DiemTBM = (d.DiemMieng + d.Diem15p + d.Diem1Tiet * 2 + d.DiemThi * 3) / 7
                        }).ToListAsync();

                    // Lấy Thời khóa biểu
                    var scheduleData = await _context.Lichhocs
                        .Where(l => l.MaLop == student.MaLop && l.MaHk == activeHocKyId)
                        .Include(l => l.MaGvNavigation).Include(l => l.MaMonHocNavigation).Include(l => l.MaPhongNavigation)
                        .ToListAsync();

                    foreach (var lich in scheduleData)
                    {
                        if (lich.ThuTrongTuan != null && lich.TietHoc.HasValue)
                        {
                            if (!viewModel.Timetable.ContainsKey(lich.ThuTrongTuan)) { viewModel.Timetable[lich.ThuTrongTuan] = new Dictionary<int, Lichhoc>(); }
                            viewModel.Timetable[lich.ThuTrongTuan][lich.TietHoc.Value] = lich;
                        }
                    }

                    // Lấy Học phí
                    viewModel.Tuitions = await _context.Hocphis.Where(hp => hp.MaHs == hsId && hp.MaHk == activeHocKyId).Include(hp => hp.MaHkNavigation).ToListAsync();
                }

                return View(viewModel);
            }
            // =======================================================
            // === KẾT THÚC KHỐI LOGIC BỔ SUNG ===
            // =======================================================

            // Giao diện chung cho khách (nếu có)
            return View(null);
        }

        private string GetTodayInVietnamese()
        {
            var today = DateTime.Now.DayOfWeek;
            return today switch
            {
                DayOfWeek.Monday => "Thứ Hai",
                DayOfWeek.Tuesday => "Thứ Ba",
                DayOfWeek.Wednesday => "Thứ Tư",
                DayOfWeek.Thursday => "Thứ Năm",
                DayOfWeek.Friday => "Thứ Sáu",
                DayOfWeek.Saturday => "Thứ Bảy",
                _ => "Chủ Nhật",
            };
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}