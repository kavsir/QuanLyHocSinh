using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;
using Student_management.Services;

namespace Student_management.Controllers
{
    public class NamHocController : Controller
    {
        private readonly QuanLyHocSinhContext _context;
        private readonly NamHocService _service;

        public NamHocController(QuanLyHocSinhContext context)
        {
            _context = context;
            _service = new NamHocService(context);
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Namhocs.ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Namhoc namHoc)
        {
            var yr = _service.ValidateAndNormalizeTenNamHoc(namHoc.TenNamHoc);
            if (!yr.ok)
            {
                ModelState.AddModelError("TenNamHoc", yr.error);
                return View(namHoc);
            }
            namHoc.TenNamHoc = yr.normalized;

            try
            {
                _service.ValidateBusiness(namHoc, yr, isEdit: false);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(namHoc);
            }

            _context.Add(namHoc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var nh = await _context.Namhocs.FindAsync(id);
            if (nh == null) return NotFound();
            return View(nh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Namhoc namHoc)
        {
            if (id != namHoc.MaNamHoc) return NotFound();

            var yr = _service.ValidateAndNormalizeTenNamHoc(namHoc.TenNamHoc);
            if (!yr.ok)
            {
                ModelState.AddModelError("TenNamHoc", yr.error);
                return View(namHoc);
            }
            namHoc.TenNamHoc = yr.normalized;

            try
            {
                _service.ValidateBusiness(namHoc, yr, isEdit: true);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(namHoc);
            }

            _context.Update(namHoc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var nh = await _context.Namhocs.FindAsync(id);
            if (nh == null) return NotFound();
            return View(nh);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nh = await _context.Namhocs.FindAsync(id);
            if (nh == null) return NotFound();

            try
            {
                _context.Namhocs.Remove(nh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); 
            }
            catch (DbUpdateException)
            {
                
                return RedirectToAction("DeleteError", "Error",
                    new { entityName = "Năm học", id = id, returnController = "NamHoc" });
            }
        }

    }
}
