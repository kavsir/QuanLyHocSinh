using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Student_management.Controllers
{
    public class GiaovienController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public GiaovienController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // GET: Giaovien - ĐÃ NÂNG CẤP THEO ĐÚNG YÊU CẦU
        public async Task<IActionResult> Index(string searchString, int? maMonHoc)
        {
            // 1. Bắt đầu truy vấn cơ sở từ context
            var giaovienQuery = _context.Giaoviens
                                        .Include(g => g.MaMonHocNavigation)
                                        .AsQueryable();

            // QUYỀN HẠN CỦA GIÁO VIÊN:
            // Theo logic thông thường, một giáo viên có thể xem danh sách các giáo viên khác trong trường.
            // Do đó, chúng ta không cần lọc bớt danh sách giáo viên dựa trên vai trò ở đây.
            // Admin và Giáo viên đều có thể xem và tìm kiếm trên toàn bộ danh sách.

            // 2. Áp dụng bộ lọc theo Môn học (chuyên môn)
            if (maMonHoc.HasValue && maMonHoc > 0)
            {
                giaovienQuery = giaovienQuery.Where(g => g.MaMonHoc == maMonHoc);
            }

            // 3. Áp dụng tìm kiếm theo Họ Tên
            if (!string.IsNullOrEmpty(searchString))
            {
                giaovienQuery = giaovienQuery.Where(g => g.HoTen.Contains(searchString));
            }

            // 4. Chuẩn bị dữ liệu cho View
            ViewData["CurrentFilter"] = searchString;
            // Sử dụng đúng tên thuộc tính "MaMonHoc" và "TenMonHoc" từ model của bạn
            ViewData["MonHocList"] = new SelectList(_context.Monhocs, "MaMonHoc", "TenMonHoc", maMonHoc);

            // 5. Thực thi truy vấn và trả về kết quả
            return View(await giaovienQuery.ToListAsync());
        }


        // =============================================================
        // CÁC ACTION BÊN DƯỚI LÀ CODE GỐC CỦA BẠN - GIỮ NGUYÊN 100%
        // =============================================================

        // GET: Giaovien/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var gv = await _context.Giaoviens
                                   .Include(g => g.MaMonHocNavigation)
                                   .FirstOrDefaultAsync(g => g.MaGv == id);
            if (gv == null) return NotFound();
            return View(gv);
        }

        // GET: Giaovien/Create
        public IActionResult Create()
        {
            ViewData["MaMonHoc"] = new SelectList(_context.Monhocs, "MaMonHoc", "TenMonHoc");
            return View();
        }

        // POST: Giaovien/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Giaovien giaovien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giaovien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaMonHoc"] = new SelectList(_context.Monhocs, "MaMonHoc", "TenMonHoc", giaovien.MaMonHoc);
            return View(giaovien);
        }

        // GET: Giaovien/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var gv = await _context.Giaoviens.FindAsync(id);
            if (gv == null) return NotFound();
            ViewData["MaMonHoc"] = new SelectList(_context.Monhocs, "MaMonHoc", "TenMonHoc", gv.MaMonHoc);
            return View(gv);
        }

        // POST: Giaovien/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Giaovien giaovien)
        {
            if (id != giaovien.MaGv) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giaovien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Giaoviens.Any(e => e.MaGv == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaMonHoc"] = new SelectList(_context.Monhocs, "MaMonHoc", "TenMonHoc", giaovien.MaMonHoc);
            return View(giaovien);
        }

        // GET: Giaovien/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var gv = await _context.Giaoviens
                                   .Include(g => g.MaMonHocNavigation)
                                   .FirstOrDefaultAsync(g => g.MaGv == id);
            if (gv == null) return NotFound();
            return View(gv);
        }

        // POST: Giaovien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gv = await _context.Giaoviens.FindAsync(id);
            if (gv != null)
            {
                _context.Giaoviens.Remove(gv);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}