using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Student_management.Models;

public partial class Lop
{
    public int MaLop { get; set; }

    [Required(ErrorMessage = "Tên lớp không được để trống.")]
    [StringLength(10, ErrorMessage = "Tên lớp không được vượt quá 10 ký tự.")]
    [Display(Name = "Tên Lớp")]
    public string TenLop { get; set; } = null!;

    [Range(0, 100, ErrorMessage = "Sĩ số phải nằm trong khoảng từ 0 đến 100.")]
    [Display(Name = "Sĩ Số")]
    public int? SiSo { get; set; }

    [Required(ErrorMessage = "Lớp học phải thuộc về một năm học.")]
    [Display(Name = "Năm Học")]
    public int? MaNamHoc { get; set; }

    [Display(Name = "Giáo viên Chủ nhiệm")]
    public int? MaGvcn { get; set; } // GVCN có thể được gán sau, nên không bắt buộc

    public virtual ICollection<Diemdanh> Diemdanhs { get; set; } = new List<Diemdanh>();
    public virtual ICollection<Hocsinh> Hocsinhs { get; set; } = new List<Hocsinh>();
    public virtual ICollection<Lichhoc> Lichhocs { get; set; } = new List<Lichhoc>();
    public virtual Giaovien? MaGvcnNavigation { get; set; }
    public virtual Namhoc? MaNamHocNavigation { get; set; }
    public virtual ICollection<PhancongGiangday> PhancongGiangdays { get; set; } = new List<PhancongGiangday>();
}