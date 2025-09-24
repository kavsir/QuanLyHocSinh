namespace Student_management.Models
{
    public class UserProfileViewModel
    {
        // Dùng để xác định loại người dùng để hiển thị giao diện phù hợp
        public string UserRole { get; set; } = "";

        // Sẽ chỉ một trong hai đối tượng này có giá trị
        public Hocsinh? StudentProfile { get; set; }
        public Giaovien? TeacherProfile { get; set; }
    }
}