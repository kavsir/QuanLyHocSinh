using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Student_management.Validation;

namespace Student_management.Models;

public partial class Hocsinh
{
    public int MaHs { get; set; }

    [Required(ErrorMessage = "Họ và tên không được để trống.")]
    [StringLength(100)]
    [Display(Name = "Họ và Tên")]
    public string HoTen { get; set; } = null!;

    [Required(ErrorMessage = "Ngày sinh không được để trống.")]
    [DataType(DataType.Date)]
    [AgeRange(6, 19, ErrorMessage = "Học sinh phải từ 6 đến 19 tuổi.")]
    [Display(Name = "Ngày Sinh")]
    public DateOnly? NgaySinh { get; set; }

    [Required(ErrorMessage = "Giới tính không được để trống.")]
    [Display(Name = "Giới Tính")]
    public string GioiTinh { get; set; } = null!;

    [Required(ErrorMessage = "Địa chỉ không được để trống.")]
    [StringLength(255)]
    [Display(Name = "Địa Chỉ")]
    public string DiaChi { get; set; } = null!;

    // === SỬA LỖI CHO SỐ ĐIỆN THOẠI ===
    [Required(ErrorMessage = "Số điện thoại (phụ huynh) không được để trống.")]
    [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
    [Display(Name = "Số Điện Thoại")]
    public string Sdt { get; set; } = null!; // Khai báo là `string` (không null)

    [Required(ErrorMessage = "Email không được để trống.")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email phải có đuôi @gmail.com.")]
    [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự.")]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Lớp không được để trống.")]
    [Display(Name = "Lớp")]
    public int? MaLop { get; set; }

    public string? TrangThai { get; set; }

    public virtual ICollection<Diemdanh> Diemdanhs { get; set; } = new List<Diemdanh>();
    public virtual ICollection<Diem> Diems { get; set; } = new List<Diem>();
    public virtual ICollection<Hocphi> Hocphis { get; set; } = new List<Hocphi>();
    public virtual Lop? MaLopNavigation { get; set; }
    public virtual ICollection<Taikhoan> Taikhoans { get; set; } = new List<Taikhoan>();
}