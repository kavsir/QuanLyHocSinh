using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Student_management.Models;

public partial class Diem
{
    public int MaDiem { get; set; }

    [Required(ErrorMessage = "Bạn phải chọn học sinh.")]
    [Display(Name = "Học sinh")]
    public int? MaHs { get; set; }

    [Required(ErrorMessage = "Bạn phải chọn môn học.")]
    [Display(Name = "Môn học")]
    public int? MaMonHoc { get; set; }

    [Required(ErrorMessage = "Bạn phải chọn học kỳ.")]
    [Display(Name = "Học kỳ")]
    public int? MaHk { get; set; }

    [Range(typeof(decimal), "0.0", "10.0", ErrorMessage = "Điểm phải nằm trong thang điểm từ 0.0 đến 10.0.")]
    [Display(Name = "Điểm miệng")]
    public decimal? DiemMieng { get; set; }

    [Range(typeof(decimal), "0.0", "10.0", ErrorMessage = "Điểm phải nằm trong thang điểm từ 0.0 đến 10.0.")]
    [Display(Name = "Điểm 15 phút")]
    public decimal? Diem15p { get; set; }

    [Range(typeof(decimal), "0.0", "10.0", ErrorMessage = "Điểm phải nằm trong thang điểm từ 0.0 đến 10.0.")]
    [Display(Name = "Điểm 1 tiết")]
    public decimal? Diem1Tiet { get; set; }

    [Range(typeof(decimal), "0.0", "10.0", ErrorMessage = "Điểm phải nằm trong thang điểm từ 0.0 đến 10.0.")]
    [Display(Name = "Điểm thi")]
    public decimal? DiemThi { get; set; }

    public virtual Hocky? MaHkNavigation { get; set; }
    public virtual Hocsinh? MaHsNavigation { get; set; }
    public virtual Monhoc? MaMonHocNavigation { get; set; }
}