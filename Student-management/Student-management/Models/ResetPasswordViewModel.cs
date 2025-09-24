using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Student_management.Models
{
    public class ResetPasswordViewModel
    {
        public int MaTk { get; set; }

        [DisplayName("Tên đăng nhập")]
        public string? TenDangNhap { get; set; }

        // <<< THÊM CÁC THUỘC TÍNH CHỈ ĐỂ HIỂN THỊ >>>
        [DisplayName("Vai trò")]
        public string? VaiTro { get; set; }

        [DisplayName("Người dùng liên kết")]
        public string? NguoiDungLienKet { get; set; }
        // <<< KẾT THÚC PHẦN THÊM >>>

        [Required(ErrorMessage = "Mật khẩu mới không được để trống.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$",
            ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt.")]
        [DisplayName("Mật khẩu mới")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bạn phải nhập lại mật khẩu.")]
        [DataType(DataType.Password)]
        [DisplayName("Xác nhận mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}