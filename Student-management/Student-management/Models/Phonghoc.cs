using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Student_management.Models;

public partial class Phonghoc
{
    public int MaPhong { get; set; }

    [Required(ErrorMessage = "Tên phòng không được để trống.")]
    [StringLength(20, ErrorMessage = "Tên phòng không được vượt quá 20 ký tự.")]
    [RegularExpression(@"^[A-Z]\d{3}$", ErrorMessage = "Tên phòng phải có định dạng, ví dụ: A101, B205...")] // <<< THÊM RÀNG BUỘC ĐỊNH DẠNG
    [Display(Name = "Tên Phòng")]
    public string TenPhong { get; set; } = null!;

    [Required(ErrorMessage = "Sức chứa không được để trống.")]
    [Range(1, 40, ErrorMessage = "Sức chứa phải là một số từ 1 đến 40.")] // <<< CẬP NHẬT GIỚI HẠN
    [Display(Name = "Sức Chứa")]
    public int? SucChua { get; set; }

    [Required(ErrorMessage = "Vị trí không được để trống.")]
    [StringLength(100, ErrorMessage = "Vị trí không được vượt quá 100 ký tự.")]
    [Display(Name = "Vị Trí")]
    public string? ViTri { get; set; }

    public virtual ICollection<Lichhoc> Lichhocs { get; set; } = new List<Lichhoc>();
}