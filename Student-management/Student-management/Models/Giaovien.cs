using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Student_management.Validation; // <<< Thêm using này

namespace Student_management.Models;

public partial class Giaovien
{
    public int MaGv { get; set; }

    [Required(ErrorMessage = "Họ và tên không được để trống.")]
    [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự.")]
    [Display(Name = "Họ và Tên")]
    public string? HoTen { get; set; }

    [Required(ErrorMessage = "Ngày sinh không được để trống.")]
    [DataType(DataType.Date)]
    [AgeRange(22, 65, ErrorMessage = "Giáo viên phải từ 22 đến 65 tuổi.")] // <<< Ràng buộc tuổi tùy chỉnh
    [Display(Name = "Ngày Sinh")]
    public DateOnly? NgaySinh { get; set; }

    [Required(ErrorMessage = "Giới tính không được để trống.")]
    [Display(Name = "Giới Tính")]
    public string? GioiTinh { get; set; }

    [Required(ErrorMessage = "Địa chỉ không được để trống.")]
    [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự.")]
    [Display(Name = "Địa Chỉ")]
    public string? DiaChi { get; set; }

    [Required(ErrorMessage = "Số điện thoại không được để trống.")]
    [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại không hợp lệ (phải có 10 số và bắt đầu bằng 0).")]
    [Display(Name = "Số Điện Thoại")]
    public string? Sdt { get; set; }

    [Required(ErrorMessage = "Email không được để trống.")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email phải có đuôi @gmail.com.")]
    [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự.")]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Bạn phải chọn chuyên môn cho giáo viên.")]
    [Display(Name = "Chuyên Môn")]
    public int? MaMonHoc { get; set; }

    public virtual ICollection<Lichhoc> Lichhocs { get; set; } = new List<Lichhoc>();
    public virtual ICollection<Lop> Lops { get; set; } = new List<Lop>();
    public virtual Monhoc? MaMonHocNavigation { get; set; }
    public virtual ICollection<PhancongGiangday> PhancongGiangdays { get; set; } = new List<PhancongGiangday>();
    public virtual ICollection<Taikhoan> Taikhoans { get; set; } = new List<Taikhoan>();
}