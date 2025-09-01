﻿using System;
using System.Collections.Generic;

namespace Student_management.Models;

public partial class Hocky
{
    public int MaHk { get; set; }

    public string TenHk { get; set; } = null!;

    public DateTime NgayBatDau { get; set; }

    public DateTime NgayKetThuc { get; set; }

    public int MaNamHoc { get; set; }

    public virtual ICollection<Diem> Diems { get; set; } = new List<Diem>();

    public virtual ICollection<Hocphi> Hocphis { get; set; } = new List<Hocphi>();

    public virtual ICollection<Lichhoc> Lichhocs { get; set; } = new List<Lichhoc>();

    public virtual Namhoc? MaNamHocNavigation { get; set; } = null!;

    public virtual ICollection<PhancongGiangday> PhancongGiangdays { get; set; } = new List<PhancongGiangday>();
}
