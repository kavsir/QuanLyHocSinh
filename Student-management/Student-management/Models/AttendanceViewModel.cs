using System.ComponentModel.DataAnnotations;

namespace Student_management.Models
{
    // Lớp này chứa thông tin điểm danh của MỘT học sinh
    public class AttendanceItem
    {
        public int MaHs { get; set; }
        public string HoTenHs { get; set; } = "";

        // Các trạng thái có thể có: 'Có mặt', 'Vắng có phép', 'Vắng không phép', 'Đi muộn'
        public string TrangThai { get; set; } = "Có mặt"; // Mặc định là 'Có mặt'
        public string? GhiChu { get; set; }
    }

    // Lớp này là model chính cho View
    public class AttendanceViewModel
    {
        public int LopId { get; set; }
        public string TenLop { get; set; } = "";

        [DataType(DataType.Date)]
        public DateTime NgayDiemDanh { get; set; } = DateTime.Today; // Mặc định là ngày hôm nay

        [Range(1, 10)]
        public int TietHoc { get; set; } = 1; // Mặc định là tiết 1

        public List<AttendanceItem> AttendanceList { get; set; } = [];
    }
}