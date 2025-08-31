using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Student_management.Models;

public partial class QuanLyHocSinhContext : DbContext
{
    public QuanLyHocSinhContext()
    {
    }

    public QuanLyHocSinhContext(DbContextOptions<QuanLyHocSinhContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Diem> Diems { get; set; }

    public virtual DbSet<Giaovien> Giaoviens { get; set; }

    public virtual DbSet<Hocky> Hockies { get; set; }

    public virtual DbSet<Hocphi> Hocphis { get; set; }

    public virtual DbSet<Hocsinh> Hocsinhs { get; set; }

    public virtual DbSet<Lichhoc> Lichhocs { get; set; }

    public virtual DbSet<Lop> Lops { get; set; }

    public virtual DbSet<Monhoc> Monhocs { get; set; }

    public virtual DbSet<Namhoc> Namhocs { get; set; }

    public virtual DbSet<PhancongGiangday> PhancongGiangdays { get; set; }

    public virtual DbSet<Phonghoc> Phonghocs { get; set; }

    public virtual DbSet<Taikhoan> Taikhoans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-BCNU3UF\\SQLEXPRESS;Database=QuanLyHocSinh;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Diem>(entity =>
        {
            entity.HasKey(e => e.MaDiem).HasName("PK__DIEM__33326025335AF806");

            entity.ToTable("DIEM");

            entity.Property(e => e.MaHk).HasColumnName("MaHK");
            entity.Property(e => e.MaHs).HasColumnName("MaHS");

            entity.HasOne(d => d.MaHkNavigation).WithMany(p => p.Diems)
                .HasForeignKey(d => d.MaHk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DIEM_HOCKY");

            entity.HasOne(d => d.MaHsNavigation).WithMany(p => p.Diems)
                .HasForeignKey(d => d.MaHs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DIEM_HOCSINH");

            entity.HasOne(d => d.MaMonHocNavigation).WithMany(p => p.Diems)
                .HasForeignKey(d => d.MaMonHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DIEM_MONHOC");
        });

        modelBuilder.Entity<Giaovien>(entity =>
        {
            entity.HasKey(e => e.MaGv).HasName("PK__GIAOVIEN__2725AEF3D078C03E");

            entity.ToTable("GIAOVIEN");

            entity.Property(e => e.MaGv).HasColumnName("MaGV");
            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.Sdt)
                .HasMaxLength(20)
                .HasColumnName("SDT");

            entity.HasOne(d => d.MaMonHocNavigation).WithMany(p => p.Giaoviens)
                .HasForeignKey(d => d.MaMonHoc)
                .HasConstraintName("FK_GIAOVIEN_MONHOC");
        });

        modelBuilder.Entity<Hocky>(entity =>
        {
            entity.HasKey(e => e.MaHk).HasName("PK__HOCKY__2725A6E798DDF631");

            entity.ToTable("HOCKY");

            entity.Property(e => e.MaHk).HasColumnName("MaHK");
            entity.Property(e => e.NgayBatDau).HasColumnType("datetime");
            entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");
            entity.Property(e => e.TenHk)
                .HasMaxLength(50)
                .HasColumnName("TenHK");

            entity.HasOne(d => d.MaNamHocNavigation).WithMany(p => p.Hockies)
                .HasForeignKey(d => d.MaNamHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HOCKY_NAMHOC");
        });

        modelBuilder.Entity<Hocphi>(entity =>
        {
            entity.HasKey(e => e.MaHp).HasName("PK__HOCPHI__2725A6EC2BA5250D");

            entity.ToTable("HOCPHI");

            entity.Property(e => e.MaHp).HasColumnName("MaHP");
            entity.Property(e => e.MaHk).HasColumnName("MaHK");
            entity.Property(e => e.MaHs).HasColumnName("MaHS");
            entity.Property(e => e.NgayDong).HasColumnType("datetime");
            entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaHkNavigation).WithMany(p => p.Hocphis)
                .HasForeignKey(d => d.MaHk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HP_HOCKY");

            entity.HasOne(d => d.MaHsNavigation).WithMany(p => p.Hocphis)
                .HasForeignKey(d => d.MaHs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HP_HOCSINH");
        });

        modelBuilder.Entity<Hocsinh>(entity =>
        {
            entity.HasKey(e => e.MaHs).HasName("PK__HOCSINH__2725A6EF8DF464F8");

            entity.ToTable("HOCSINH");

            entity.Property(e => e.MaHs).HasColumnName("MaHS");
            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.Sdt)
                .HasMaxLength(20)
                .HasColumnName("SDT");
            entity.Property(e => e.TrangThai).HasMaxLength(20);

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.Hocsinhs)
                .HasForeignKey(d => d.MaLop)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HOCSINH_LOP");
        });

        modelBuilder.Entity<Lichhoc>(entity =>
        {
            entity.HasKey(e => e.MaLichHoc).HasName("PK__LICHHOC__150EBC21FE727732");

            entity.ToTable("LICHHOC");

            entity.Property(e => e.MaGv).HasColumnName("MaGV");
            entity.Property(e => e.MaHk).HasColumnName("MaHK");
            entity.Property(e => e.ThuTrongTuan).HasMaxLength(20);

            entity.HasOne(d => d.MaGvNavigation).WithMany(p => p.Lichhocs)
                .HasForeignKey(d => d.MaGv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LH_GIAOVIEN");

            entity.HasOne(d => d.MaHkNavigation).WithMany(p => p.Lichhocs)
                .HasForeignKey(d => d.MaHk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LH_HOCKY");

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.Lichhocs)
                .HasForeignKey(d => d.MaLop)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LH_LOP");

            entity.HasOne(d => d.MaMonHocNavigation).WithMany(p => p.Lichhocs)
                .HasForeignKey(d => d.MaMonHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LH_MONHOC");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.Lichhocs)
                .HasForeignKey(d => d.MaPhong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LH_PHONGHOC");
        });

        modelBuilder.Entity<Lop>(entity =>
        {
            entity.HasKey(e => e.MaLop).HasName("PK__LOP__3B98D273AFDDE0EA");

            entity.ToTable("LOP");

            entity.Property(e => e.MaGvcn).HasColumnName("MaGVCN");
            entity.Property(e => e.TenLop).HasMaxLength(50);

            entity.HasOne(d => d.MaGvcnNavigation).WithMany(p => p.Lops)
                .HasForeignKey(d => d.MaGvcn)
                .HasConstraintName("FK_LOP_GIAOVIEN");

            entity.HasOne(d => d.MaNamHocNavigation).WithMany(p => p.Lops)
                .HasForeignKey(d => d.MaNamHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LOP_NAMHOC");
        });

        modelBuilder.Entity<Monhoc>(entity =>
        {
            entity.HasKey(e => e.MaMonHoc).HasName("PK__MONHOC__4127737F07677F91");

            entity.ToTable("MONHOC");

            entity.Property(e => e.TenMonHoc).HasMaxLength(100);
        });

        modelBuilder.Entity<Namhoc>(entity =>
        {
            entity.HasKey(e => e.MaNamHoc).HasName("PK__NAMHOC__7DBADD74AFF1217A");

            entity.ToTable("NAMHOC");

            entity.Property(e => e.TenNamHoc).HasMaxLength(50);
        });

        modelBuilder.Entity<PhancongGiangday>(entity =>
        {
            entity.HasKey(e => e.MaPc).HasName("PK__PHANCONG__2725E7E5AE0FB1F2");

            entity.ToTable("PHANCONG_GIANGDAY");

            entity.Property(e => e.MaPc).HasColumnName("MaPC");
            entity.Property(e => e.MaGv).HasColumnName("MaGV");
            entity.Property(e => e.MaHk).HasColumnName("MaHK");

            entity.HasOne(d => d.MaGvNavigation).WithMany(p => p.PhancongGiangdays)
                .HasForeignKey(d => d.MaGv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PC_GIAOVIEN");

            entity.HasOne(d => d.MaHkNavigation).WithMany(p => p.PhancongGiangdays)
                .HasForeignKey(d => d.MaHk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PC_HOCKY");

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.PhancongGiangdays)
                .HasForeignKey(d => d.MaLop)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PC_LOP");

            entity.HasOne(d => d.MaMonHocNavigation).WithMany(p => p.PhancongGiangdays)
                .HasForeignKey(d => d.MaMonHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PC_MONHOC");
        });

        modelBuilder.Entity<Phonghoc>(entity =>
        {
            entity.HasKey(e => e.MaPhong).HasName("PK__PHONGHOC__20BD5E5B8EB74A64");

            entity.ToTable("PHONGHOC");

            entity.Property(e => e.TenPhong).HasMaxLength(50);
            entity.Property(e => e.ViTri).HasMaxLength(100);
        });

        modelBuilder.Entity<Taikhoan>(entity =>
        {
            entity.HasKey(e => e.MaTk).HasName("PK__TAIKHOAN__272500707D09B47B");

            entity.ToTable("TAIKHOAN");

            entity.HasIndex(e => e.TenDangNhap, "UQ__TAIKHOAN__55F68FC02A10495F").IsUnique();

            entity.Property(e => e.MaTk).HasColumnName("MaTK");
            entity.Property(e => e.MaGv).HasColumnName("MaGV");
            entity.Property(e => e.MaHs).HasColumnName("MaHS");
            entity.Property(e => e.MatKhau).HasMaxLength(255);
            entity.Property(e => e.TenDangNhap).HasMaxLength(50);
            entity.Property(e => e.VaiTro).HasMaxLength(20);

            entity.HasOne(d => d.MaGvNavigation).WithMany(p => p.Taikhoans)
                .HasForeignKey(d => d.MaGv)
                .HasConstraintName("FK_TK_GIAOVIEN");

            entity.HasOne(d => d.MaHsNavigation).WithMany(p => p.Taikhoans)
                .HasForeignKey(d => d.MaHs)
                .HasConstraintName("FK_TK_HOCSINH");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
