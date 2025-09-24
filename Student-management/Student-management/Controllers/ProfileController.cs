using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Student_management.Controllers;

[Authorize] // Yêu cầu người dùng phải đăng nhập để truy cập
public class ProfileController(QuanLyHocSinhContext context) : Controller
{
    // GET: Profile/Index
    public async Task<IActionResult> Index()
    {
        var username = User.Identity?.Name;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(username))
        {
            return RedirectToAction("Login", "Account");
        }

        var taikhoan = await context.Taikhoans
            .Include(t => t.MaGvNavigation)
            .Include(t => t.MaHsNavigation)
                .ThenInclude(hs => hs!.MaLopNavigation)
            .FirstOrDefaultAsync(t => t.TenDangNhap == username);

        if (taikhoan == null)
        {
            return NotFound("Không tìm thấy thông tin tài khoản.");
        }

        // Dùng ViewData để gửi thông tin ra View
        ViewData["Role"] = role;
        if (role == "GiaoVien" && taikhoan.MaGvNavigation != null)
        {
            ViewData["ProfileModel"] = taikhoan.MaGvNavigation;
        }
        else if (role == "HocSinh" && taikhoan.MaHsNavigation != null)
        {
            ViewData["ProfileModel"] = taikhoan.MaHsNavigation;
        }
        else
        {
            // Xử lý cho Admin hoặc các vai trò khác nếu cần
            return View("NoProfile");
        }

        return View();
    }
}