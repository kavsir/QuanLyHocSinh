/*
------------------------------------------------------
-- SCRIPT TẠO DỮ LIỆU MẪU CHO DATABASE QUANLYHOCSINH
------------------------------------------------------
-- Script này sẽ xóa hết dữ liệu cũ và thêm vào dữ liệu mới.
-- Bao gồm: 2 năm học, nhiều môn học, giáo viên, lớp, học sinh,
-- tài khoản, phân công, lịch học, điểm và học phí.
------------------------------------------------------
*/

USE QuanLyHocSinh;
GO

------------------------------------------------------
-- BƯỚC 1: XÓA TOÀN BỘ DỮ LIỆU CŨ (THEO ĐÚNG THỨ TỰ KHÓA NGOẠI)
------------------------------------------------------
DELETE FROM DIEM;
DELETE FROM HOCPHI;
DELETE FROM LICHHOC;
DELETE FROM PHANCONG_GIANGDAY;
DELETE FROM TAIKHOAN;
DELETE FROM HOCSINH;
DELETE FROM LOP;
DELETE FROM GIAOVIEN;
DELETE FROM HOCKY;
DELETE FROM NAMHOC;
DELETE FROM MONHOC;
DELETE FROM PHONGHOC;
GO

------------------------------------------------------
-- BƯỚC 2: THÊM DỮ LIỆU MỚI
-- Sử dụng SET IDENTITY_INSERT để có thể chèn ID cụ thể, giúp liên kết dữ liệu dễ dàng.
------------------------------------------------------

-- Bảng Năm học
SET IDENTITY_INSERT NAMHOC ON;
INSERT INTO NAMHOC (MaNamHoc, TenNamHoc, NgayBatDau, NgayKetThuc) VALUES
(1, N'2024 - 2025', '2024-09-05', '2025-05-25'),
(2, N'2025 - 2026', '2025-09-05', '2026-05-25');
SET IDENTITY_INSERT NAMHOC OFF;
GO

-- Bảng Học kỳ
SET IDENTITY_INSERT HOCKY ON;
INSERT INTO HOCKY (MaHK, TenHK, NgayBatDau, NgayKetThuc, MaNamHoc) VALUES
(1, N'Học kỳ 1 (2024-2025)', '2024-09-05', '2025-01-15', 1),
(2, N'Học kỳ 2 (2024-2025)', '2025-01-20', '2025-05-25', 1),
(3, N'Học kỳ 1 (2025-2026)', '2025-09-05', '2026-01-15', 2),
(4, N'Học kỳ 2 (2025-2026)', '2026-01-20', '2026-05-25', 2);
SET IDENTITY_INSERT HOCKY OFF;
GO

-- Bảng Môn học
SET IDENTITY_INSERT MONHOC ON;
INSERT INTO MONHOC (MaMonHoc, TenMonHoc, SoTiet, HeSo) VALUES
(1, N'Toán', 90, 2.0),
(2, N'Ngữ Văn', 90, 2.0),
(3, N'Tiếng Anh', 70, 1.5),
(4, N'Vật Lý', 60, 1.0),
(5, N'Hóa Học', 60, 1.0),
(6, N'Sinh Học', 50, 1.0),
(7, N'Lịch Sử', 40, 1.0),
(8, N'Địa Lý', 40, 1.0),
(9, N'Giáo dục công dân', 30, 1.0),
(10, N'Thể dục', 30, 1.0);
SET IDENTITY_INSERT MONHOC OFF;
GO

-- Bảng Phòng học
SET IDENTITY_INSERT PHONGHOC ON;
INSERT INTO PHONGHOC (MaPhong, TenPhong, SucChua, ViTri) VALUES
(1, N'Phòng 101', 45, N'Tầng 1, dãy A'),
(2, N'Phòng 102', 45, N'Tầng 1, dãy A'),
(3, N'Phòng 201', 45, N'Tầng 2, dãy A'),
(4, N'Phòng 202', 45, N'Tầng 2, dãy A'),
(5, N'Phòng thực hành Lý', 50, N'Tầng 3, dãy B');
SET IDENTITY_INSERT PHONGHOC OFF;
GO

-- Bảng Giáo viên
SET IDENTITY_INSERT GIAOVIEN ON;
INSERT INTO GIAOVIEN (MaGV, HoTen, NgaySinh, GioiTinh, DiaChi, SDT, Email, MaMonHoc) VALUES
(1, N'Nguyễn Thị Lan', '1980-05-20', N'Nữ', N'Quận Cầu Giấy, Hà Nội', '0912345678', 'lan.nt@edu.com', 2),
(2, N'Trần Văn Minh', '1978-11-15', N'Nam', N'Quận Ba Đình, Hà Nội', '0987654321', 'minh.tv@edu.com', 1),
(3, N'Lê Hoàng Anh', '1985-02-10', N'Nam', N'Quận Đống Đa, Hà Nội', '0905123456', 'anh.lh@edu.com', 3),
(4, N'Phạm Thu Hà', '1982-08-30', N'Nữ', N'Quận Hai Bà Trưng, Hà Nội', '0934567890', 'ha.pt@edu.com', 4),
(5, N'Vũ Đình Tuấn', '1975-01-25', N'Nam', N'Quận Hoàn Kiếm, Hà Nội', '0978111222', 'tuan.vd@edu.com', 5);
SET IDENTITY_INSERT GIAOVIEN OFF;
GO

-- Bảng Lớp
SET IDENTITY_INSERT LOP ON;
INSERT INTO LOP (MaLop, TenLop, SiSo, MaNamHoc, MaGVCN) VALUES
(1, N'10A1', 0, 1, 1),
(2, N'10A2', 0, 1, 2),
(3, N'11B1', 0, 1, 3),
(4, N'12C1', 0, 1, 4);
SET IDENTITY_INSERT LOP OFF;
GO

-- Bảng Học sinh
SET IDENTITY_INSERT HOCSINH ON;
INSERT INTO HOCSINH (MaHS, HoTen, NgaySinh, GioiTinh, DiaChi, SDT, Email, MaLop, TrangThai) VALUES
(1, N'Nguyễn Văn An', '2009-01-15', N'Nam', N'Hà Nội', '0981112223', 'an.nv@hs.com', 1, N'Đang học'),
(2, N'Trần Thị Bình', '2009-03-22', N'Nữ', N'Hà Nội', '0982223334', 'binh.tt@hs.com', 1, N'Đang học'),
(3, N'Lê Văn Cường', '2009-05-30', N'Nam', N'Hải Phòng', '0983334445', 'cuong.lv@hs.com', 1, N'Đang học'),
(4, N'Phạm Thị Dung', '2009-07-12', N'Nữ', N'Hà Nội', '0984445556', 'dung.pt@hs.com', 2, N'Đang học'),
(5, N'Hoàng Văn Giang', '2009-09-01', N'Nam', N'Nam Định', '0985556667', 'giang.hv@hs.com', 2, N'Đang học'),
(6, N'Nguyễn Minh Khang', '2008-10-10', N'Nam', N'Hà Nội', '0981112222', 'khang@hs.com', 3, N'Đang học');
SET IDENTITY_INSERT HOCSINH OFF;
GO

-- Cập nhật lại sĩ số cho các lớp
UPDATE LOP SET SiSo = (SELECT COUNT(*) FROM HOCSINH WHERE HOCSINH.MaLop = LOP.MaLop);
GO

-- Bảng Tài khoản
-- Mật khẩu mặc định cho tất cả là "123456" (cần được băm)
SET IDENTITY_INSERT TAIKHOAN ON;
INSERT INTO TAIKHOAN (MaTK, TenDangNhap, MatKhau, VaiTro, MaHS, MaGV) VALUES
(1, N'admin', N'123456', N'Admin', NULL, NULL),
-- Giáo viên
(2, N'gv01', N'123456', N'GiaoVien', NULL, 1),
(3, N'gv02', N'123456', N'GiaoVien', NULL, 2),
(4, N'gv03', N'123456', N'GiaoVien', NULL, 3),
-- Học sinh
(5, N'hs01', N'123456', N'HocSinh', 1, NULL),
(6, N'hs02', N'123456', N'HocSinh', 2, NULL),
(7, N'hs03', N'123456', N'HocSinh', 3, NULL),
(8, N'hs06', N'123456', N'HocSinh', 6, NULL);
SET IDENTITY_INSERT TAIKHOAN OFF;
GO

-- Bảng Phân công giảng dạy cho Học kỳ 1 (2024-2025)
SET IDENTITY_INSERT PHANCONG_GIANGDAY ON;
INSERT INTO PHANCONG_GIANGDAY(MaPC, MaGV, MaMonHoc, MaLop, MaHK) VALUES
(1, 2, 1, 1, 1), -- GV Minh dạy Toán lớp 10A1
(2, 1, 2, 1, 1), -- GV Lan dạy Văn lớp 10A1
(3, 3, 3, 1, 1), -- GV Anh dạy Tiếng Anh lớp 10A1
(4, 2, 1, 2, 1), -- GV Minh dạy Toán lớp 10A2
(5, 1, 2, 2, 1); -- GV Lan dạy Văn lớp 10A2
SET IDENTITY_INSERT PHANCONG_GIANGDAY OFF;
GO

-- Bảng Lịch học
SET IDENTITY_INSERT LICHHOC ON;
INSERT INTO LICHHOC (MaLichHoc, MaLop, MaMonHoc, MaGV, MaPhong, MaHK, ThuTrongTuan, TietHoc) VALUES
(1, 1, 1, 2, 1, 1, N'Thứ Hai', 1),
(2, 1, 1, 2, 1, 1, N'Thứ Hai', 2),
(3, 1, 2, 1, 1, 1, N'Thứ Ba', 3),
(4, 2, 1, 2, 2, 1, N'Thứ Hai', 3),
(5, 2, 1, 2, 2, 1, N'Thứ Hai', 4);
SET IDENTITY_INSERT LICHHOC OFF;
GO

-- Bảng Điểm
SET IDENTITY_INSERT DIEM ON;
INSERT INTO DIEM (MaDiem, MaHS, MaMonHoc, MaHK, DiemMieng, Diem15p, Diem1Tiet, DiemThi) VALUES
(1, 1, 1, 1, 8.0, 8.5, 9.0, 8.5), -- Điểm Toán của Nguyễn Văn An
(2, 1, 2, 1, 7.0, 7.5, 8.0, 7.5), -- Điểm Văn của Nguyễn Văn An
(3, 2, 1, 1, 9.0, 9.5, 9.0, 9.5); -- Điểm Toán của Trần Thị Bình
SET IDENTITY_INSERT DIEM OFF;
GO

-- Bảng Học phí
SET IDENTITY_INSERT HOCPHI ON;
INSERT INTO HOCPHI (MaHP, MaHS, SoTien, NgayDong, TrangThai, MaHK) VALUES
(1, 1, 5000000, '2024-09-10', N'Đã đóng', 1),
(2, 2, 5000000, '2024-09-12', N'Đã đóng', 1),
(3, 3, 5000000, NULL, N'Chưa đóng', 1);
SET IDENTITY_INSERT HOCPHI OFF;
GO

PRINT 'Tạo dữ liệu mẫu thành công!';
GO