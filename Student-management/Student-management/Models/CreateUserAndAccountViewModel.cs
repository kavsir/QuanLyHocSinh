using Student_management.Validation; // Thêm using này nếu chưa có
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Student_management.Models
{
    public class CreateUserAndAccountViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn vai trò.")]
        [DisplayName("Vai trò Người dùng")]
        public string VaiTro { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ và tên không được để trống.")]
        [DisplayName("Họ và tên")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Họ tên phải có ít nhất 2 ký tự.")]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ngày sinh không được để trống.")] // <<< THÊM
        [DataType(DataType.Date)]
        [AgeRange(6, 65, ErrorMessage = "Người dùng phải từ 6 đến 65 tuổi.")] // <<< THÊM
        [DisplayName("Ngày sinh")]
        public DateOnly? NgaySinh { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn giới tính.")]
        public string? GioiTinh { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống.")] // <<< THÊM
        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự.")] // <<< THÊM
        public string? DiaChi { get; set; }

        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")] // <<< THAY THẾ: Dùng [EmailAddress] chuẩn
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự.")]
        [DisplayName("Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải có 10 số và bắt đầu bằng 0.")] // <<< Cải tiến Regex
        [DisplayName("Số điện thoại")]
        public string? Sdt { get; set; }

        [DisplayName("Môn chuyên môn")]
        public int? MaMonHoc { get; set; } // Sẽ kiểm tra trong Controller

        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        [DisplayName("Tên đăng nhập")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải có từ 3-50 ký tự.")]
        [RegularExpression(@"^[a-zA-Z0-9_.]+$", ErrorMessage = "Tên đăng nhập chỉ được chứa chữ cái, số, dấu gạch dưới và dấu chấm.")]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$",
            ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt.")] // <<< THÊM: Ràng buộc độ phức tạp mật khẩu
        [DisplayName("Mật khẩu")]
        public string MatKhau { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bạn phải nhập lại mật khẩu.")] // <<< THÊM
        [DataType(DataType.Password)]
        [DisplayName("Xác nhận mật khẩu")]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}