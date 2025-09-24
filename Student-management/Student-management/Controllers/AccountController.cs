using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Student_management.Controllers;

public class AccountController(QuanLyHocSinhContext context) : Controller
{
    // GET: /Account/Login
    public IActionResult Login()
    {
        // Xóa "thẻ an ninh" cũ nếu có
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return View();
    }

    // POST: /Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(Taikhoan model)
    {
        if (string.IsNullOrEmpty(model.TenDangNhap) || string.IsNullOrEmpty(model.MatKhau))
        {
            ModelState.AddModelError(string.Empty, "Tên đăng nhập và mật khẩu không được để trống.");
            return View(model);
        }

        var user = await context.Taikhoans.FirstOrDefaultAsync(tk => tk.TenDangNhap == model.TenDangNhap);

        if (user != null && user.MatKhau == model.MatKhau) // Cần thay bằng BCrypt.Verify sau này
        {
            // --- BẮT ĐẦU LOGIC TẠO "THẺ AN NINH" ---
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.TenDangNhap),
                new Claim(ClaimTypes.Role, user.VaiTro)
                // Bạn có thể thêm các thông tin khác vào "thẻ" ở đây
                // new Claim("UserId", user.MaTk.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
            // --- KẾT THÚC LOGIC TẠO "THẺ AN NINH" ---

            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
        return View(model);
    }

    // GET: /Account/Logout
    public async Task<IActionResult> Logout()
    {
        // Hủy "thẻ an ninh" hiện tại
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }
}