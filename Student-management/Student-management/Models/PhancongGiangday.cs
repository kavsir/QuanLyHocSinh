using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Student_management.Models;

public partial class PhancongGiangday
{
    public int MaPc { get; set; }

    [Required(ErrorMessage = "Bạn phải chọn một giáo viên.")]
    [Display(Name = "Giáo viên")]
    public int? MaGv { get; set; }

    [Required(ErrorMessage = "Bạn phải chọn một môn học.")]
    [Display(Name = "Môn học")]
    public int? MaMonHoc { get; set; }

    [Required(ErrorMessage = "Bạn phải chọn một lớp học.")]
    [Display(Name = "Lớp học")]
    public int? MaLop { get; set; }

    [Required(ErrorMessage = "Bạn phải chọn một học kỳ.")]
    [Display(Name = "Học kỳ")]
    public int? MaHk { get; set; }

    public virtual Giaovien? MaGvNavigation { get; set; }
    public virtual Hocky? MaHkNavigation { get; set; }
    public virtual Lop? MaLopNavigation { get; set; }
    public virtual Monhoc? MaMonHocNavigation { get; set; }
}