using Student_management.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Student_management.Validation;
namespace Student_management.Models;

public partial class Namhoc
{
    public int MaNamHoc { get; set; }

    [Required(ErrorMessage = "Tên năm học không được để trống.")]
    [RegularExpression(@"^\d{4}\s-\s\d{4}$", ErrorMessage = "Tên năm học phải đúng định dạng YYYY - YYYY.")]
    [FutureYearRange] // <<< GHI CHÚ: Thêm ràng buộc mới ở đây
    [Display(Name = "Tên Năm Học")]
    public string TenNamHoc { get; set; } = null!;

    [Required(ErrorMessage = "Ngày bắt đầu không được để trống.")]
    [DataType(DataType.Date)]
    [NoPastDate(ErrorMessage = "Ngày bắt đầu không được là một ngày trong quá khứ.")]
    [Display(Name = "Ngày Bắt Đầu")]
    public DateTime NgayBatDau { get; set; }

    [Required(ErrorMessage = "Ngày kết thúc không được để trống.")]
    [DataType(DataType.Date)]
    [NoPastDate(ErrorMessage = "Ngày kết thúc không được là một ngày trong quá khứ.")] // <<< GHI CHÚ: Thêm ràng buộc này cho cả ngày kết thúc
    [DateGreaterThan("NgayBatDau", ErrorMessage = "Ngày kết thúc phải sau ngày bắt đầu.")]
    [Display(Name = "Ngày Kết Thúc")]
    public DateTime NgayKetThuc { get; set; }

    public virtual ICollection<Hocky> Hockies { get; set; } = new List<Hocky>();

    public virtual ICollection<Lop> Lops { get; set; } = new List<Lop>();
}
