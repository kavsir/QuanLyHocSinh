using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Student_management.Models;

public partial class Lichhoc
{
    public int MaLichHoc { get; set; }

    [Required(ErrorMessage = "Bạn phải chọn Lớp học.")]
    [Display(Name = "Lớp học")]
    public int? MaLop { get; set; }

    [Required(ErrorMessage = "Bạn phải chọn Môn học.")]
    [Display(Name = "Môn học")]
    public int? MaMonHoc { get; set; }

    [Required(ErrorMessage = "Bạn phải chọn Giáo viên.")]
    [Display(Name = "Giáo viên")]
    public int? MaGv { get; set; }

    [Required(ErrorMessage = "Bạn phải chọn Phòng học.")]
    [Display(Name = "Phòng học")]
    public int? MaPhong { get; set; }

    [Required(ErrorMessage = "Bạn phải chọn Học kỳ.")]
    [Display(Name = "Học kỳ")]
    public int? MaHk { get; set; }

    [Required(ErrorMessage = "Bạn phải chọn Thứ trong tuần.")]
    [Display(Name = "Thứ trong tuần")]
    public string? ThuTrongTuan { get; set; }

    [Required(ErrorMessage = "Bạn phải chọn Tiết học.")]
    [Range(1, 10, ErrorMessage = "Tiết học phải là một số từ 1 đến 10.")] // <<< THAY ĐỔI Ở ĐÂY
    [Display(Name = "Tiết học")]
    public int? TietHoc { get; set; }

    public virtual Giaovien? MaGvNavigation { get; set; }
    public virtual Hocky? MaHkNavigation { get; set; }
    public virtual Lop? MaLopNavigation { get; set; }
    public virtual Monhoc? MaMonHocNavigation { get; set; }
    public virtual Phonghoc? MaPhongNavigation { get; set; }
}