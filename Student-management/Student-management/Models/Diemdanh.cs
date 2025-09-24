using System;
using System.Collections.Generic;

namespace Student_management.Models;

public partial class Diemdanh
{
    public int MaDiemDanh { get; set; }

    public int? MaHs { get; set; }

    public int? MaLop { get; set; }

    public DateOnly NgayDiemDanh { get; set; }

    public int TietHoc { get; set; }

    public string TrangThai { get; set; } = null!;

    public string? GhiChu { get; set; }

    public virtual Hocsinh? MaHsNavigation { get; set; }

    public virtual Lop? MaLopNavigation { get; set; }
}
