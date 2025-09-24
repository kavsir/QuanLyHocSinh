using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Student_management.Models
{
    // Lớp này dùng để chứa thông tin một dòng trong bảng điểm của học sinh
    public class GradeItem
    {
        public string TenMonHoc { get; set; } = "";
        public decimal? DiemMieng { get; set; }
        public decimal? Diem15p { get; set; }
        public decimal? Diem1Tiet { get; set; }
        public decimal? DiemThi { get; set; }
        public decimal? DiemTBM { get; set; }
    }

    // Đây là ViewModel chính cho toàn bộ trang của học sinh
    public class StudentPortalViewModel
    {
        // 1. Thông tin cá nhân
        public Hocsinh? StudentInfo { get; set; }

        // 2. Bảng điểm cho học kỳ được chọn
        public List<GradeItem> Grades { get; set; } = new List<GradeItem>();

        // 3. Thời khóa biểu cho học kỳ được chọn
        public Dictionary<string, Dictionary<int, Lichhoc>> Timetable { get; set; } = new Dictionary<string, Dictionary<int, Lichhoc>>();

        // 4. Thông tin học phí cho học kỳ được chọn
        public List<Hocphi> Tuitions { get; set; } = new List<Hocphi>();

        // 5. Dữ liệu cần thiết cho bộ lọc học kỳ
        public int SelectedHocKyId { get; set; }
        public List<SelectListItem> HocKyOptions { get; set; } = new List<SelectListItem>();
    }
}