namespace Student_management.Models
{
    public class MyScheduleViewModel
    {
        public string TenGiaoVien { get; set; } = "";
        public string TenHocKy { get; set; } = "";
        public Dictionary<string, Dictionary<int, Lichhoc>> TimetableGrid { get; set; } = new();
    }
}