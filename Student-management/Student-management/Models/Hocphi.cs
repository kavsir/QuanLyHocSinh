﻿using System;
using System.Collections.Generic;

namespace Student_management.Models;

public partial class Hocphi
{
    public int MaHp { get; set; }

    public int MaHs { get; set; }

    public decimal? SoTien { get; set; }

    public DateTime? NgayDong { get; set; }

    public string? TrangThai { get; set; }

    public int MaHk { get; set; }

    public virtual Hocky MaHkNavigation { get; set; } = null!;

    public virtual Hocsinh MaHsNavigation { get; set; } = null!;
}
