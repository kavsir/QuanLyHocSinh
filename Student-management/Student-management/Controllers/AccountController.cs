using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_management.Models;
using System.Security.Claims;

namespace Student_management.Controllers
{
    public class AccountController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public AccountController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
        {
            var user = _context.Taikhoans
                .FirstOrDefault(u => u.TenDangNhap == username && u.MatKhau == password);

            if (user == null)
            {
                ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu!";
                return View();
            }

            // Kiểm tra role có hợp lệ không
            var validRoles = new[] { "Admin", "GiaoVien", "HocSinh" };
            if (!validRoles.Contains(user.VaiTro))
            {
                ViewBag.Error = "Tài khoản không có vai trò hợp lệ!";
                return View();
            }

            // Tạo claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.TenDangNhap),
                new Claim(ClaimTypes.Role, user.VaiTro)
            };

            // Chỉ add claim ID tương ứng với role
            if (user.VaiTro == "HocSinh" && user.MaHs.HasValue)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.MaHs.Value.ToString()));
            }
            else if (user.VaiTro == "GiaoVien" && user.MaGv.HasValue)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.MaGv.Value.ToString()));
            }
            else if (user.VaiTro == "Admin")
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.MaTk.ToString())); // Admin dùng MaTk
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            // Chuyển hướng theo vai trò
            return user.VaiTro switch
            {
                "Admin" => RedirectToAction("Index", "Dashboard"),
                "GiaoVien" => RedirectToAction("Index", "TeacherDashboard"),
                "HocSinh" => RedirectToAction("Index", "ThoiKhoaBieu"),
                _ => RedirectToAction("AccessDenied", "Account")
            };
        }

        // GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        // GET: /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
