using Microsoft.AspNetCore.Mvc.Rendering; // Thêm using này

namespace Student_management.Models
{
    public class AdminDashboardViewModel
    {
        // Dữ liệu thống kê cũ
        public int TongHocSinh { get; set; }
        public int TongGiaoVien { get; set; }
        public int TongLopHoc { get; set; }
        public double TiLeGvHs { get; set; }
        public List<Hocsinh> HocSinhMoiNhat { get; set; } = [];

        // Dữ liệu cho biểu đồ
        public List<string> TenLop { get; set; } = [];
        public List<int> SiSoMoiLop { get; set; } = [];

        // === THUỘC TÍNH MỚI CHO BỘ LỌC ===
        public int? SelectedNamHocId { get; set; }
        public List<SelectListItem> NamHocOptions { get; set; } = [];

        // === THUỘC TÍNH MỚI CHO CÁC THẺ CẢNH BÁO ===
        public int TaiKhoanCanTao { get; set; }
        public int LopChuaCoGvcn { get; set; }
    }
}