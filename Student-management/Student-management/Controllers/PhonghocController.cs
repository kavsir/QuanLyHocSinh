using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;
using Microsoft.AspNetCore.Authorization;

namespace Student_management.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PhonghocController : Controller
    {
        private readonly QuanLyHocSinhContext _context;

        public PhonghocController(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        // GET: Phonghoc
        public async Task<IActionResult> Index()
        {
            var phonghocs = await _context.Phonghocs.ToListAsync();
            return View(phonghocs);
        }

        // GET: Phonghoc/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var phonghoc = await _context.Phonghocs
                .FirstOrDefaultAsync(p => p.MaPhong == id);
            if (phonghoc == null) return NotFound();
            return View(phonghoc);
        }

        // GET: Phonghoc/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Phonghoc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Phonghoc phonghoc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phonghoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(phonghoc);
        }

        // GET: Phonghoc/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var phonghoc = await _context.Phonghocs.FindAsync(id);
            if (phonghoc == null) return NotFound();
            return View(phonghoc);
        }

        // POST: Phonghoc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Phonghoc phonghoc)
        {
            if (id != phonghoc.MaPhong) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phonghoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Phonghocs.Any(p => p.MaPhong == id))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(phonghoc);
        }

        // GET: Phonghoc/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var phonghoc = await _context.Phonghocs
                .FirstOrDefaultAsync(p => p.MaPhong == id);
            if (phonghoc == null) return NotFound();
            return View(phonghoc);
        }

        // POST: Phonghoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phonghoc = await _context.Phonghocs.FindAsync(id);
            if (phonghoc != null)
            {
                _context.Phonghocs.Remove(phonghoc);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
