using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Student_management.Validation; // Thêm using này

namespace Student_management.Models;

public partial class Hocphi
{
    public int MaHp { get; set; }

    [Required(ErrorMessage = "Bạn phải chọn học sinh.")]
    [Display(Name = "Học sinh")]
    public int? MaHs { get; set; }

    [Required(ErrorMessage = "Số tiền không được để trống.")]
    [Range(typeof(decimal), "1", "999999999", ErrorMessage = "Số tiền phải là một số dương.")]
    [DataType(DataType.Currency)]
    [Display(Name = "Số tiền")]
    public decimal? SoTien { get; set; }

    [DataType(DataType.Date)]
    [NoFutureDateOnly(ErrorMessage = "Ngày đóng không thể là một ngày trong tương lai.")]
    [Display(Name = "Ngày đóng")]
    public DateOnly? NgayDong { get; set; } // Ngày đóng có thể null khi mới tạo

    [Required(ErrorMessage = "Bạn phải chọn trạng thái.")]
    [Display(Name = "Trạng thái")]
    public string? TrangThai { get; set; } // Ví dụ: "Đã đóng", "Chưa đóng"

    [Required(ErrorMessage = "Bạn phải chọn học kỳ.")]
    [Display(Name = "Học kỳ")]
    public int? MaHk { get; set; }

    public virtual Hocky? MaHkNavigation { get; set; }
    public virtual Hocsinh? MaHsNavigation { get; set; }
}