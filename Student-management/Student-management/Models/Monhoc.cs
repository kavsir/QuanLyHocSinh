using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Student_management.Models;

public partial class Monhoc
{
    public int MaMonHoc { get; set; }

    [Required(ErrorMessage = "Tên môn học không được để trống.")]
    [StringLength(50, ErrorMessage = "Tên môn học không được vượt quá 50 ký tự.")]
    [Display(Name = "Tên Môn Học")]
    public string TenMonHoc { get; set; } = null!;

    [Required(ErrorMessage = "Số tiết không được để trống.")]
    [Range(1, 200, ErrorMessage = "Số tiết phải là một số dương và không quá 200.")]
    [Display(Name = "Số Tiết")]
    public int? SoTiet { get; set; }

    [Required(ErrorMessage = "Hệ số không được để trống.")]
    [Range(typeof(decimal), "1.0", "3.0", ErrorMessage = "Hệ số phải nằm trong khoảng từ 1.0 đến 3.0.")]
    [Display(Name = "Hệ Số Điểm")]
    public decimal? HeSo { get; set; }

    public virtual ICollection<Diem> Diems { get; set; } = new List<Diem>();
    public virtual ICollection<Giaovien> Giaoviens { get; set; } = new List<Giaovien>();
    public virtual ICollection<Lichhoc> Lichhocs { get; set; } = new List<Lichhoc>();
    public virtual ICollection<PhancongGiangday> PhancongGiangdays { get; set; } = new List<PhancongGiangday>();
}