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

    public virtual DbSet<Diemdanh> Diemdanhs { get; set; }

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
     
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Diem>(entity =>
        {
            entity.HasKey(e => e.MaDiem).HasName("PK__DIEM__3332602576B6CCB8");

            entity.ToTable("DIEM");

            entity.Property(e => e.Diem15p).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.Diem1Tiet).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.DiemMieng).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.DiemThi).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.MaHk).HasColumnName("MaHK");
            entity.Property(e => e.MaHs).HasColumnName("MaHS");

            entity.HasOne(d => d.MaHkNavigation).WithMany(p => p.Diems)
                .HasForeignKey(d => d.MaHk)
                .HasConstraintName("FK__DIEM__MaHK__44FF419A");

            entity.HasOne(d => d.MaHsNavigation).WithMany(p => p.Diems)
                .HasForeignKey(d => d.MaHs)
                .HasConstraintName("FK__DIEM__MaHS__4316F928");

            entity.HasOne(d => d.MaMonHocNavigation).WithMany(p => p.Diems)
                .HasForeignKey(d => d.MaMonHoc)
                .HasConstraintName("FK__DIEM__MaMonHoc__440B1D61");
        });

        modelBuilder.Entity<Diemdanh>(entity =>
        {
            entity.HasKey(e => e.MaDiemDanh).HasName("PK__DIEMDANH__1512439DC41CE86B");

            entity.ToTable("DIEMDANH");

            entity.Property(e => e.GhiChu).HasMaxLength(255);
            entity.Property(e => e.MaHs).HasColumnName("MaHS");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaHsNavigation).WithMany(p => p.Diemdanhs)
                .HasForeignKey(d => d.MaHs)
                .HasConstraintName("FK__DIEMDANH__MaHS__4BAC3F29");

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.Diemdanhs)
                .HasForeignKey(d => d.MaLop)
                .HasConstraintName("FK__DIEMDANH__MaLop__4CA06362");
        });

        modelBuilder.Entity<Giaovien>(entity =>
        {
            entity.HasKey(e => e.MaGv).HasName("PK__GIAOVIEN__2725AEF378333925");

            entity.ToTable("GIAOVIEN");

            entity.Property(e => e.MaGv).HasColumnName("MaGV");
            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.Sdt)
                .HasMaxLength(20)
                .HasColumnName("SDT");

            entity.HasOne(d => d.MaMonHocNavigation).WithMany(p => p.Giaoviens)
                .HasForeignKey(d => d.MaMonHoc)
                .HasConstraintName("FK__GIAOVIEN__MaMonH__2B3F6F97");
        });

        modelBuilder.Entity<Hocky>(entity =>
        {
            entity.HasKey(e => e.MaHk).HasName("PK__HOCKY__2725A6E759FDF576");

            entity.ToTable("HOCKY");

            entity.Property(e => e.MaHk).HasColumnName("MaHK");
            entity.Property(e => e.TenHk)
                .HasMaxLength(50)
                .HasColumnName("TenHK");

            entity.HasOne(d => d.MaNamHocNavigation).WithMany(p => p.Hockies)
                .HasForeignKey(d => d.MaNamHoc)
                .HasConstraintName("FK__HOCKY__MaNamHoc__267ABA7A");
        });

        modelBuilder.Entity<Hocphi>(entity =>
        {
            entity.HasKey(e => e.MaHp).HasName("PK__HOCPHI__2725A6EC1C44F904");

            entity.ToTable("HOCPHI");

            entity.Property(e => e.MaHp).HasColumnName("MaHP");
            entity.Property(e => e.MaHk).HasColumnName("MaHK");
            entity.Property(e => e.MaHs).HasColumnName("MaHS");
            entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaHkNavigation).WithMany(p => p.Hocphis)
                .HasForeignKey(d => d.MaHk)
                .HasConstraintName("FK__HOCPHI__MaHK__48CFD27E");

            entity.HasOne(d => d.MaHsNavigation).WithMany(p => p.Hocphis)
                .HasForeignKey(d => d.MaHs)
                .HasConstraintName("FK__HOCPHI__MaHS__47DBAE45");
        });

        modelBuilder.Entity<Hocsinh>(entity =>
        {
            entity.HasKey(e => e.MaHs).HasName("PK__HOCSINH__2725A6EFC731D968");

            entity.ToTable("HOCSINH");

            entity.Property(e => e.MaHs).HasColumnName("MaHS");
            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.Sdt)
                .HasMaxLength(20)
                .HasColumnName("SDT");
            entity.Property(e => e.TrangThai).HasMaxLength(20);

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.Hocsinhs)
                .HasForeignKey(d => d.MaLop)
                .HasConstraintName("FK__HOCSINH__MaLop__31EC6D26");
        });

        modelBuilder.Entity<Lichhoc>(entity =>
        {
            entity.HasKey(e => e.MaLichHoc).HasName("PK__LICHHOC__150EBC21E5E52F17");

            entity.ToTable("LICHHOC");

            entity.Property(e => e.MaGv).HasColumnName("MaGV");
            entity.Property(e => e.MaHk).HasColumnName("MaHK");
            entity.Property(e => e.ThuTrongTuan).HasMaxLength(20);

            entity.HasOne(d => d.MaGvNavigation).WithMany(p => p.Lichhocs)
                .HasForeignKey(d => d.MaGv)
                .HasConstraintName("FK__LICHHOC__MaGV__3E52440B");

            entity.HasOne(d => d.MaHkNavigation).WithMany(p => p.Lichhocs)
                .HasForeignKey(d => d.MaHk)
                .HasConstraintName("FK__LICHHOC__MaHK__403A8C7D");

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.Lichhocs)
                .HasForeignKey(d => d.MaLop)
                .HasConstraintName("FK__LICHHOC__MaLop__3C69FB99");

            entity.HasOne(d => d.MaMonHocNavigation).WithMany(p => p.Lichhocs)
                .HasForeignKey(d => d.MaMonHoc)
                .HasConstraintName("FK__LICHHOC__MaMonHo__3D5E1FD2");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.Lichhocs)
                .HasForeignKey(d => d.MaPhong)
                .HasConstraintName("FK__LICHHOC__MaPhong__3F466844");
        });

        modelBuilder.Entity<Lop>(entity =>
        {
            entity.HasKey(e => e.MaLop).HasName("PK__LOP__3B98D273D480E6FE");

            entity.ToTable("LOP");

            entity.Property(e => e.MaGvcn).HasColumnName("MaGVCN");
            entity.Property(e => e.TenLop).HasMaxLength(50);

            entity.HasOne(d => d.MaGvcnNavigation).WithMany(p => p.Lops)
                .HasForeignKey(d => d.MaGvcn)
                .HasConstraintName("FK__LOP__MaGVCN__2F10007B");

            entity.HasOne(d => d.MaNamHocNavigation).WithMany(p => p.Lops)
                .HasForeignKey(d => d.MaNamHoc)
                .HasConstraintName("FK__LOP__MaNamHoc__2E1BDC42");
        });

        modelBuilder.Entity<Monhoc>(entity =>
        {
            entity.HasKey(e => e.MaMonHoc).HasName("PK__MONHOC__4127737FCCCADB5F");

            entity.ToTable("MONHOC");

            entity.Property(e => e.HeSo).HasColumnType("decimal(3, 2)");
            entity.Property(e => e.TenMonHoc).HasMaxLength(100);
        });

        modelBuilder.Entity<Namhoc>(entity =>
        {
            entity.HasKey(e => e.MaNamHoc).HasName("PK__NAMHOC__7DBADD741EEC3296");

            entity.ToTable("NAMHOC");

            entity.Property(e => e.TenNamHoc).HasMaxLength(50);
        });

        modelBuilder.Entity<PhancongGiangday>(entity =>
        {
            entity.HasKey(e => e.MaPc).HasName("PK__PHANCONG__2725E7E5BA82A205");

            entity.ToTable("PHANCONG_GIANGDAY");

            entity.Property(e => e.MaPc).HasColumnName("MaPC");
            entity.Property(e => e.MaGv).HasColumnName("MaGV");
            entity.Property(e => e.MaHk).HasColumnName("MaHK");

            entity.HasOne(d => d.MaGvNavigation).WithMany(p => p.PhancongGiangdays)
                .HasForeignKey(d => d.MaGv)
                .HasConstraintName("FK__PHANCONG_G__MaGV__36B12243");

            entity.HasOne(d => d.MaHkNavigation).WithMany(p => p.PhancongGiangdays)
                .HasForeignKey(d => d.MaHk)
                .HasConstraintName("FK__PHANCONG_G__MaHK__398D8EEE");

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.PhancongGiangdays)
                .HasForeignKey(d => d.MaLop)
                .HasConstraintName("FK__PHANCONG___MaLop__38996AB5");

            entity.HasOne(d => d.MaMonHocNavigation).WithMany(p => p.PhancongGiangdays)
                .HasForeignKey(d => d.MaMonHoc)
                .HasConstraintName("FK__PHANCONG___MaMon__37A5467C");
        });

        modelBuilder.Entity<Phonghoc>(entity =>
        {
            entity.HasKey(e => e.MaPhong).HasName("PK__PHONGHOC__20BD5E5BB252C03E");

            entity.ToTable("PHONGHOC");

            entity.Property(e => e.TenPhong).HasMaxLength(50);
            entity.Property(e => e.ViTri).HasMaxLength(100);
        });

        modelBuilder.Entity<Taikhoan>(entity =>
        {
            entity.HasKey(e => e.MaTk).HasName("PK__TAIKHOAN__2725007017408302");

            entity.ToTable("TAIKHOAN");

            entity.HasIndex(e => e.TenDangNhap, "UQ__TAIKHOAN__55F68FC02D1172B5").IsUnique();

            entity.Property(e => e.MaTk).HasColumnName("MaTK");
            entity.Property(e => e.MaGv).HasColumnName("MaGV");
            entity.Property(e => e.MaHs).HasColumnName("MaHS");
            entity.Property(e => e.MatKhau).HasMaxLength(255);
            entity.Property(e => e.TenDangNhap).HasMaxLength(50);
            entity.Property(e => e.VaiTro).HasMaxLength(20);

            entity.HasOne(d => d.MaGvNavigation).WithMany(p => p.Taikhoans)
                .HasForeignKey(d => d.MaGv)
                .HasConstraintName("FK__TAIKHOAN__MaGV__5165187F");

            entity.HasOne(d => d.MaHsNavigation).WithMany(p => p.Taikhoans)
                .HasForeignKey(d => d.MaHs)
                .HasConstraintName("FK__TAIKHOAN__MaHS__5070F446");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
