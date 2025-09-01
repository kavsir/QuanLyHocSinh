using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers
{
    public class HocsinhController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public HocsinhController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // GET: Hocsinh
        // Action DUY NHẤT để hiển thị danh sách, tích hợp tìm kiếm và lọc
        public async Task<IActionResult> Index(string searchString, int? maLop, int? maNamHoc)
        {
            // 1. Lấy ra query ban đầu cho Học sinh
            var hocsinhs = _context.Hocsinhs
                                .Include(h => h.MaLopNavigation)
                                .ThenInclude(l => l.MaNamHocNavigation)
                                .AsQueryable();

            // 2. Phân quyền dữ liệu dựa trên vai trò người dùng (Sẽ tích hợp ở bước sau)
            var userRole = HttpContext.Session.GetString("UserRole");
            var taiKhoanIdStr = HttpContext.Session.GetString("UserId");
            int.TryParse(taiKhoanIdStr, out int taiKhoanId);

            switch (userRole)
            {
                case "GiaoVien":
                    var taiKhoanGv = await _context.Taikhoans.FirstOrDefaultAsync(t => t.MaTk == taiKhoanId);
                    if (taiKhoanGv?.MaGv != null)
                    {
                        var lopGvDayIds = await _context.PhancongGiangdays
                                                    .Where(pc => pc.MaGv == taiKhoanGv.MaGv)
                                                    .Select(pc => pc.MaLop)
                                                    .Distinct()
                                                    .ToListAsync();
                        hocsinhs = hocsinhs.Where(hs => lopGvDayIds.Contains(hs.MaLop));
                    }
                    break;
                case "HocSinh":
                    var taiKhoanHs = await _context.Taikhoans.FirstOrDefaultAsync(t => t.MaTk == taiKhoanId);
                    if (taiKhoanHs?.MaHs != null)
                    {
                        var hocSinhHienTai = await _context.Hocsinhs.FindAsync(taiKhoanHs.MaHs);
                        if (hocSinhHienTai?.MaLop != null)
                        {
                            hocsinhs = hocsinhs.Where(hs => hs.MaLop == hocSinhHienTai.MaLop);
                        }
                        else
                        {
                            hocsinhs = hocsinhs.Where(hs => false);
                        }
                    }
                    break;
            }

            // 3. Áp dụng bộ lọc (Filter)
            if (maLop.HasValue && maLop > 0)
            {
                hocsinhs = hocsinhs.Where(s => s.MaLop == maLop);
            }
            if (maNamHoc.HasValue && maNamHoc > 0)
            {
                hocsinhs = hocsinhs.Where(s => s.MaLopNavigation.MaNamHoc == maNamHoc);
            }

            // 4. Áp dụng tìm kiếm (Search)
            if (!string.IsNullOrEmpty(searchString))
            {
                hocsinhs = hocsinhs.Where(s => s.HoTen.Contains(searchString));
            }

            // 5. Chuẩn bị dữ liệu cho DropDownList trên View
            ViewData["LopList"] = new SelectList(_context.Lops, "MaLop", "TenLop", maLop);
            ViewData["NamHocList"] = new SelectList(_context.Namhocs, "MaNamHoc", "TenNamHoc", maNamHoc);
            ViewData["CurrentFilter"] = searchString;

            return View(await hocsinhs.ToListAsync());
        }

        // GET: Hocsinh/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var hocsinh = await _context.Hocsinhs
                .Include(h => h.MaLopNavigation)
                .FirstOrDefaultAsync(m => m.MaHs == id);

            if (hocsinh == null) return NotFound();
            return View(hocsinh);
        }

        // GET: Hocsinh/Create
        public IActionResult Create()
        {
            ViewBag.MaLop = new SelectList(_context.Lops, "MaLop", "TenLop");
            return View();
        }

        // POST: Hocsinh/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hocsinh hocsinh)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hocsinh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MaLop = new SelectList(_context.Lops, "MaLop", "TenLop", hocsinh.MaLop);
            return View(hocsinh);
        }

        // GET: Hocsinh/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var hocsinh = await _context.Hocsinhs.FindAsync(id);
            if (hocsinh == null) return NotFound();

            ViewBag.MaLop = new SelectList(_context.Lops, "MaLop", "TenLop", hocsinh.MaLop);
            return View(hocsinh);
        }

        // POST: Hocsinh/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Hocsinh hocsinh)
        {
            if (id != hocsinh.MaHs) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hocsinh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Hocsinhs.Any(e => e.MaHs == hocsinh.MaHs))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MaLop = new SelectList(_context.Lops, "MaLop", "TenLop", hocsinh.MaLop);
            return View(hocsinh);
        }

        // GET: Hocsinh/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var hocsinh = await _context.Hocsinhs
                .Include(h => h.MaLopNavigation)
                .FirstOrDefaultAsync(m => m.MaHs == id);
            if (hocsinh == null) return NotFound();

            return View(hocsinh);
        }

        // POST: Hocsinh/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hocsinh = await _context.Hocsinhs.FindAsync(id);
            if (hocsinh != null)
            {
                var relatedTaikhoan = await _context.Taikhoans.FirstOrDefaultAsync(t => t.MaHs == id);
                if (relatedTaikhoan != null)
                {
                    _context.Taikhoans.Remove(relatedTaikhoan);
                }

                _context.Hocsinhs.Remove(hocsinh);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}