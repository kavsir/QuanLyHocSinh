namespace Student_management.Models
{
    // Một lớp phụ để chứa thông tin lớp được phân công
    public class AssignedClassInfo
    {
        public int MaLop { get; set; }
        public string TenLop { get; set; } = "";
        public int MaMonHoc { get; set; }
        public string TenMonHoc { get; set; } = "";
        public int SiSo { get; set; }
    }

    public class TeacherDashboardViewModel
    {
        public string HoTenGiaoVien { get; set; } = "N/A";
        public int TongSoLopDangDay { get; set; }
        public int TongSoHocSinhPhuTrach { get; set; }

        // Danh sách các lớp được phân công trong học kỳ hiện tại
        public List<AssignedClassInfo> CacLopDuocPhanCong { get; set; } = [];

        // Lịch dạy trong ngày hôm nay
        public List<Lichhoc> LichDayHomNay { get; set; } = [];
    }
}