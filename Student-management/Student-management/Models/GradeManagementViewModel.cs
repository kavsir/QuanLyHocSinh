using System.ComponentModel.DataAnnotations;

namespace Student_management.Models
{
    // Lớp này chứa thông tin điểm của một học sinh trong bảng
    public class GradeEntryItem
    {
        public int MaHs { get; set; }
        public string HoTenHs { get; set; } = "";

        // MaDiem > 0 nếu học sinh đã có điểm, = 0 nếu là bản ghi mới
        public int MaDiem { get; set; }

        [Range(0, 10, ErrorMessage = "Điểm phải từ 0 đến 10.")]
        public decimal? DiemMieng { get; set; }

        [Range(0, 10, ErrorMessage = "Điểm phải từ 0 đến 10.")] 
        public decimal? Diem15p { get; set; }

        [Range(0, 10, ErrorMessage = "Điểm phải từ 0 đến 10.")]
        public decimal? Diem1Tiet { get; set; }

        [Range(0, 10, ErrorMessage = "Điểm phải từ 0 đến 10.")]
        public decimal? DiemThi { get; set; }
    }

    // Lớp này là model chính cho View
    public class GradeManagementViewModel
    {
        public int LopId { get; set; }
        public string TenLop { get; set; } = "";
        public int MonHocId { get; set; }
        public string TenMonHoc { get; set; } = "";
        public int HocKyId { get; set; }
        public string TenHocKy { get; set; } = "";

        public List<GradeEntryItem> GradeEntries { get; set; } = [];
    }
}