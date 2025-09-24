using Student_management.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Student_management.Models;

public partial class Hocky
{
    public int MaHk { get; set; }

    [Required(ErrorMessage = "Tên học kỳ không được để trống.")]
    [Display(Name = "Tên Học Kỳ")]
    public string? TenHk { get; set; }

    [Required(ErrorMessage = "Ngày bắt đầu không được để trống.")]
    [DataType(DataType.Date)]
    [NoPastDate(ErrorMessage = "Ngày bắt đầu không được là một ngày trong quá khứ.")]
    [Display(Name = "Ngày Bắt Đầu")]
    public DateTime NgayBatDau { get; set; }

    [Required(ErrorMessage = "Ngày kết thúc không được để trống.")]
    [DataType(DataType.Date)]
    [NoPastDate(ErrorMessage = "Ngày kết thúc không được là một ngày trong quá khứ.")]
    [DateGreaterThan("NgayBatDau", ErrorMessage = "Ngày kết thúc phải sau ngày bắt đầu.")]
    [Display(Name = "Ngày Kết Thúc")]
    public DateTime NgayKetThuc { get; set; }

    [Required(ErrorMessage = "Bạn phải chọn một năm học.")]
    [Display(Name = "Năm Học")]
    public int? MaNamHoc { get; set; }

    public virtual ICollection<Diem> Diems { get; set; } = new List<Diem>();

    public virtual ICollection<Hocphi> Hocphis { get; set; } = new List<Hocphi>();

    public virtual ICollection<Lichhoc> Lichhocs { get; set; } = new List<Lichhoc>();

    public virtual Namhoc? MaNamHocNavigation { get; set; }

    public virtual ICollection<PhancongGiangday> PhancongGiangdays { get; set; } = new List<PhancongGiangday>();
}
