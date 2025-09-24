using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

namespace Student_management.Controllers;

[Authorize(Roles = "Admin,GiaoVien")]
public class DiemsController(QuanLyHocSinhContext context) : Controller
{
    // GET: Diems
    public async Task<IActionResult> Index(string? searchString, int? subjectFilter, string? sortBy)
    {
        ViewBag.SubjectList = new SelectList(await context.Monhocs.OrderBy(m => m.TenMonHoc).ToListAsync(), "MaMonHoc", "TenMonHoc", subjectFilter);
        ViewData["CurrentSearch"] = searchString;
        ViewData["CurrentSubjectFilter"] = subjectFilter;
        ViewData["NameSortParm"] = String.IsNullOrEmpty(sortBy) ? "name_desc" : "";
        ViewData["ExamSortParm"] = sortBy == "exam" ? "exam_desc" : "exam";

        var diemsQuery = context.Diems
            .Include(d => d.MaHsNavigation).ThenInclude(hs => hs!.MaLopNavigation)
            .Include(d => d.MaMonHocNavigation)
            .AsQueryable();

        if (!String.IsNullOrEmpty(searchString))
        {
            diemsQuery = diemsQuery.Where(d => d.MaHsNavigation != null && d.MaHsNavigation.HoTen!.Contains(searchString));
        }
        if (subjectFilter.HasValue)
        {
            diemsQuery = diemsQuery.Where(d => d.MaMonHoc == subjectFilter);
        }

        switch (sortBy)
        {
            case "name_desc": diemsQuery = diemsQuery.OrderByDescending(d => d.MaHsNavigation!.HoTen); break;
            case "exam": diemsQuery = diemsQuery.OrderBy(d => d.DiemThi); break;
            case "exam_desc": diemsQuery = diemsQuery.OrderByDescending(d => d.DiemThi); break;
            default: diemsQuery = diemsQuery.OrderBy(d => d.MaHsNavigation!.HoTen); break;
        }

        return View(await diemsQuery.ToListAsync());
    }

    // =============================================================
    // === ACTION MỚI DÀNH CHO GIAO DIỆN NHẬP ĐIỂM CỦA GIÁO VIÊN ===
    // =============================================================
    // GET: /Diems/ManageGrades?lopId=1&monHocId=1
    public async Task<IActionResult> ManageGrades(int lopId, int monHocId)
    {
        var lop = await context.Lops.FindAsync(lopId);
        var monHoc = await context.Monhocs.FindAsync(monHocId);
        if (lop == null || monHoc == null) return NotFound();

        var currentHk = await context.Hockies.Where(h => h.NgayBatDau.Date <= DateTime.Now.Date).OrderByDescending(h => h.NgayBatDau).FirstOrDefaultAsync();
        if (currentHk == null)
        {
            TempData["ErrorMessage"] = "Không tìm thấy học kỳ hiện tại để nhập điểm.";
            return RedirectToAction("Index", "Home");
        }

        var viewModel = new GradeManagementViewModel
        {
            LopId = lop.MaLop,
            TenLop = lop.TenLop ?? "",
            MonHocId = monHoc.MaMonHoc,
            TenMonHoc = monHoc.TenMonHoc ?? "",
            HocKyId = currentHk.MaHk,
            TenHocKy = currentHk.TenHk ?? ""
        };

        var studentsInClass = await context.Hocsinhs.Where(hs => hs.MaLop == lopId).OrderBy(hs => hs.HoTen).ToListAsync();
        var existingGrades = await context.Diems.Where(d => d.MaMonHoc == monHocId && d.MaHk == currentHk.MaHk && studentsInClass.Select(s => s.MaHs).Contains(d.MaHs ?? 0)).ToListAsync();

        foreach (var student in studentsInClass)
        {
            var grade = existingGrades.FirstOrDefault(g => g.MaHs == student.MaHs);
            viewModel.GradeEntries.Add(new GradeEntryItem
            {
                MaHs = student.MaHs,
                HoTenHs = student.HoTen ?? "",
                MaDiem = grade?.MaDiem ?? 0,
                DiemMieng = grade?.DiemMieng,
                Diem15p = grade?.Diem15p,
                Diem1Tiet = grade?.Diem1Tiet,
                DiemThi = grade?.DiemThi
            });
        }
        return View(viewModel);
    }

    // POST: /Diems/ManageGrades
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ManageGrades(GradeManagementViewModel model)
    {
        // BƯỚC KIỂM TRA BỊ THIẾU TRƯỚC ĐÂY
        if (!ModelState.IsValid)
        {
            // Nếu có lỗi (ví dụ điểm > 10), ModelState.IsValid sẽ là false
            // Cần tải lại thông tin tiêu đề cho View trước khi trả về
            var lop = await context.Lops.FindAsync(model.LopId);
            var monHoc = await context.Monhocs.FindAsync(model.MonHocId);
            var hocKy = await context.Hockies.FindAsync(model.HocKyId);

            model.TenLop = lop?.TenLop ?? "";
            model.TenMonHoc = monHoc?.TenMonHoc ?? "";
            model.TenHocKy = hocKy?.TenHk ?? "";

            // Trả về lại View với các lỗi để người dùng sửa
            return View(model);
        }

        // Logic lưu chỉ chạy khi tất cả dữ liệu đã hợp lệ
        foreach (var entry in model.GradeEntries)
        {
            if (entry.MaDiem > 0) // Cập nhật điểm đã có
            {
                var grade = await context.Diems.FindAsync(entry.MaDiem);
                if (grade != null)
                {
                    grade.DiemMieng = entry.DiemMieng;
                    grade.Diem15p = entry.Diem15p;
                    grade.Diem1Tiet = entry.Diem1Tiet;
                    grade.DiemThi = entry.DiemThi;
                }
            }
            else // Thêm điểm mới
            {
                if (entry.DiemMieng.HasValue || entry.Diem15p.HasValue || entry.Diem1Tiet.HasValue || entry.DiemThi.HasValue)
                {
                    var newGrade = new Diem
                    {
                        MaHs = entry.MaHs,
                        MaMonHoc = model.MonHocId,
                        MaHk = model.HocKyId,
                        DiemMieng = entry.DiemMieng,
                        Diem15p = entry.Diem15p,
                        Diem1Tiet = entry.Diem1Tiet,
                        DiemThi = entry.DiemThi
                    };
                    context.Diems.Add(newGrade);
                }
            }
        }

        await context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Đã cập nhật bảng điểm cho lớp {model.TenLop}.";
        return RedirectToAction("Index", "Home");
    }

    // GET: Diems/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var diem = await context.Diems
            .Include(d => d.MaHsNavigation)
            .Include(d => d.MaMonHocNavigation)
            .Include(d => d.MaHkNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.MaDiem == id);

        if (diem == null)
        {
            return NotFound();
        }

        return View(diem);
    }



    private void PopulateDropdowns(object? selectedHs = null, object? selectedMonHoc = null, object? selectedHk = null)

    {

        ViewData["MaHs"] = new SelectList(context.Hocsinhs, "MaHs", "HoTen", selectedHs);

        ViewData["MaMonHoc"] = new SelectList(context.Monhocs, "MaMonHoc", "TenMonHoc", selectedMonHoc);

        ViewData["MaHk"] = new SelectList(context.Hockies, "MaHk", "TenHk", selectedHk);

    }



    // GET: Diems/Create

    public IActionResult Create()

    {

        PopulateDropdowns();

        return View();

    }



    // POST: Diems/Create

    [HttpPost]

    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Create([Bind("MaHs,MaMonHoc,MaHk,DiemMieng,Diem15p,Diem1Tiet,DiemThi")] Diem diem)

    {

        if (ModelState.IsValid)

        {

            bool isExisting = await context.Diems.AnyAsync(d =>

              d.MaHs == diem.MaHs &&

              d.MaMonHoc == diem.MaMonHoc &&

              d.MaHk == diem.MaHk);



            if (isExisting)

            {

                ModelState.AddModelError("", "Học sinh này đã có bảng điểm cho môn học này trong học kỳ đã chọn.");

            }

        }



        if (ModelState.IsValid)

        {

            context.Add(diem);

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }



        PopulateDropdowns(diem.MaHs, diem.MaMonHoc, diem.MaHk);

        return View(diem);

    }



    // GET: Diems/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // Tải các thông tin liên quan để hiển thị trên form edit
        var diem = await context.Diems
            .Include(d => d.MaHsNavigation)
            .Include(d => d.MaMonHocNavigation)
            .Include(d => d.MaHkNavigation)
            .FirstOrDefaultAsync(d => d.MaDiem == id);

        if (diem == null)
        {
            return NotFound();
        }
        return View(diem);
    }

    // POST: Diems/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MaDiem,DiemMieng,Diem15p,Diem1Tiet,DiemThi")] Diem diem)
    {
        if (id != diem.MaDiem)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                // Cách cập nhật an toàn: Chỉ cập nhật các trường điểm số
                var diemToUpdate = await context.Diems.FindAsync(id);
                if (diemToUpdate == null) return NotFound();

                diemToUpdate.DiemMieng = diem.DiemMieng;
                diemToUpdate.Diem15p = diem.Diem15p;
                diemToUpdate.Diem1Tiet = diem.Diem1Tiet;
                diemToUpdate.DiemThi = diem.DiemThi;

                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật điểm số thành công!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiemExists(diem.MaDiem))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            // Sau khi sửa xong, quay về trang Index
            return RedirectToAction(nameof(Index));
        }

        // Nếu có lỗi, tải lại thông tin cần thiết và hiển thị lại form
        var diemWithError = await context.Diems
           .Include(d => d.MaHsNavigation)
           .Include(d => d.MaMonHocNavigation)
           .Include(d => d.MaHkNavigation)
           .FirstOrDefaultAsync(d => d.MaDiem == id);

        return View(diemWithError);
    }

    private bool DiemExists(int id)
    {
        return context.Diems.Any(e => e.MaDiem == id);
    }


    // GET: Diems/Delete/5

    public async Task<IActionResult> Delete(int? id)

    {

        if (id == null)

        {

            return NotFound();

        }



        var diem = await context.Diems

          .Include(d => d.MaHkNavigation)

          .Include(d => d.MaHsNavigation)

          .Include(d => d.MaMonHocNavigation)

          .FirstOrDefaultAsync(m => m.MaDiem == id);



        if (diem == null)

        {

            return NotFound();

        }



        return View(diem);

    }



    // POST: Diems/Delete/5

    [HttpPost, ActionName("Delete")]

    [ValidateAntiForgeryToken]

    public async Task<IActionResult> DeleteConfirmed(int id)

    {

        var diem = await context.Diems.FindAsync(id);

        if (diem != null)

        {

            context.Diems.Remove(diem);

            await context.SaveChangesAsync();

        }



        return RedirectToAction(nameof(Index));

    }


     

}