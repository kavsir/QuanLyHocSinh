﻿using System;
using System.Collections.Generic;

namespace Student_management.Models;

public partial class Taikhoan
{
    public int MaTk { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string VaiTro { get; set; } = null!;

    public int? MaHs { get; set; }

    public int? MaGv { get; set; }

    public virtual Giaovien? MaGvNavigation { get; set; }

    public virtual Hocsinh? MaHsNavigation { get; set; }
}
