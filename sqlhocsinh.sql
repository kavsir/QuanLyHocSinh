------------------------------------------------------
-- Xóa database cũ (nếu có)
------------------------------------------------------
USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'QuanLyHocSinh')
BEGIN
    ALTER DATABASE QuanLyHocSinh SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyHocSinh;
END
GO

------------------------------------------------------
-- Tạo database mới
------------------------------------------------------
CREATE DATABASE QuanLyHocSinh;
GO
USE QuanLyHocSinh;
GO

------------------------------------------------------
-- Bảng Năm học
------------------------------------------------------
CREATE TABLE NAMHOC (
    MaNamHoc INT IDENTITY(1,1) PRIMARY KEY,
    TenNamHoc NVARCHAR(50),
    NgayBatDau DATE,
    NgayKetThuc DATE
);

------------------------------------------------------
-- Bảng Học kỳ
------------------------------------------------------
CREATE TABLE HOCKY (
    MaHK INT IDENTITY(1,1) PRIMARY KEY,
    TenHK NVARCHAR(50),
    NgayBatDau DATE,
    NgayKetThuc DATE,
    MaNamHoc INT FOREIGN KEY REFERENCES NAMHOC(MaNamHoc)
);

------------------------------------------------------
-- Bảng Môn học
------------------------------------------------------
CREATE TABLE MONHOC (
    MaMonHoc INT IDENTITY(1,1) PRIMARY KEY,
    TenMonHoc NVARCHAR(100),
    SoTiet INT,
    HeSo DECIMAL(3,2)
);

------------------------------------------------------
-- Bảng Giáo viên
------------------------------------------------------
CREATE TABLE GIAOVIEN (
    MaGV INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100),
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    DiaChi NVARCHAR(200),
    SDT NVARCHAR(20),
    Email NVARCHAR(100),
    MaMonHoc INT FOREIGN KEY REFERENCES MONHOC(MaMonHoc)
);

------------------------------------------------------
-- Bảng Lớp
------------------------------------------------------
CREATE TABLE LOP (
    MaLop INT IDENTITY(1,1) PRIMARY KEY,
    TenLop NVARCHAR(50),
    SiSo INT,
    MaNamHoc INT FOREIGN KEY REFERENCES NAMHOC(MaNamHoc),
    MaGVCN INT FOREIGN KEY REFERENCES GIAOVIEN(MaGV)
);

------------------------------------------------------
-- Bảng Học sinh
------------------------------------------------------
CREATE TABLE HOCSINH (
    MaHS INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100),
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    DiaChi NVARCHAR(200),
    SDT NVARCHAR(20),
    Email NVARCHAR(100),
    MaLop INT FOREIGN KEY REFERENCES LOP(MaLop),
    TrangThai NVARCHAR(20)
);

------------------------------------------------------
-- Bảng Phòng học
------------------------------------------------------
CREATE TABLE PHONGHOC (
    MaPhong INT IDENTITY(1,1) PRIMARY KEY,
    TenPhong NVARCHAR(50),
    SucChua INT,
    ViTri NVARCHAR(100)
);

------------------------------------------------------
-- Bảng Phân công giảng dạy
------------------------------------------------------
CREATE TABLE PHANCONG_GIANGDAY (
    MaPC INT IDENTITY(1,1) PRIMARY KEY,
    MaGV INT FOREIGN KEY REFERENCES GIAOVIEN(MaGV),
    MaMonHoc INT FOREIGN KEY REFERENCES MONHOC(MaMonHoc),
    MaLop INT FOREIGN KEY REFERENCES LOP(MaLop),
    MaHK INT FOREIGN KEY REFERENCES HOCKY(MaHK)
);

------------------------------------------------------
-- Bảng Lịch học
------------------------------------------------------
CREATE TABLE LICHHOC (
    MaLichHoc INT IDENTITY(1,1) PRIMARY KEY,
    MaLop INT FOREIGN KEY REFERENCES LOP(MaLop),
    MaMonHoc INT FOREIGN KEY REFERENCES MONHOC(MaMonHoc),
    MaGV INT FOREIGN KEY REFERENCES GIAOVIEN(MaGV),
    MaPhong INT FOREIGN KEY REFERENCES PHONGHOC(MaPhong),
    MaHK INT FOREIGN KEY REFERENCES HOCKY(MaHK),
    ThuTrongTuan NVARCHAR(20),
    TietHoc INT
);

------------------------------------------------------
-- Bảng Điểm
------------------------------------------------------
CREATE TABLE DIEM (
    MaDiem INT IDENTITY(1,1) PRIMARY KEY,
    MaHS INT FOREIGN KEY REFERENCES HOCSINH(MaHS),
    MaMonHoc INT FOREIGN KEY REFERENCES MONHOC(MaMonHoc),
    MaHK INT FOREIGN KEY REFERENCES HOCKY(MaHK),
    DiemMieng DECIMAL(4,2),
    Diem15p DECIMAL(4,2),
    Diem1Tiet DECIMAL(4,2),
    DiemThi DECIMAL(4,2)
);

------------------------------------------------------
-- Bảng Học phí
------------------------------------------------------
CREATE TABLE HOCPHI (
    MaHP INT IDENTITY(1,1) PRIMARY KEY,
    MaHS INT FOREIGN KEY REFERENCES HOCSINH(MaHS),
    SoTien DECIMAL(18,2),
    NgayDong DATE,
    TrangThai NVARCHAR(50),
    MaHK INT FOREIGN KEY REFERENCES HOCKY(MaHK)
);

------------------------------------------------------
-- Bảng Tài khoản (role chỉ là text: Admin/HocSinh/GiaoVien)
------------------------------------------------------
CREATE TABLE TAIKHOAN (
    MaTK INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap NVARCHAR(50) UNIQUE,
    MatKhau NVARCHAR(255),
    VaiTro NVARCHAR(20), -- Admin / HocSinh / GiaoVien
    MaHS INT NULL FOREIGN KEY REFERENCES HOCSINH(MaHS),
    MaGV INT NULL FOREIGN KEY REFERENCES GIAOVIEN(MaGV)
);
-- Bảng Điểm danh

GO

CREATE TABLE DIEMDANH (
    MaDiemDanh INT IDENTITY(1,1) PRIMARY KEY,
    MaHS INT FOREIGN KEY REFERENCES HOCSINH(MaHS),
    MaLop INT FOREIGN KEY REFERENCES LOP(MaLop),
    NgayDiemDanh DATE NOT NULL,
    TietHoc INT NOT NULL,
    TrangThai NVARCHAR(50) NOT NULL, -- Ví dụ: 'Có mặt', 'Vắng có phép', 'Vắng không phép', 'Đi muộn'
    GhiChu NVARCHAR(255) NULL
);
GO