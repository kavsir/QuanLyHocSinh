using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Student_management.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MONHOC",
                columns: table => new
                {
                    MaMonHoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenMonHoc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoTiet = table.Column<int>(type: "int", nullable: true),
                    HeSo = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MONHOC__4127737F4381EA57", x => x.MaMonHoc);
                });

            migrationBuilder.CreateTable(
                name: "NAMHOC",
                columns: table => new
                {
                    MaNamHoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNamHoc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NgayBatDau = table.Column<DateOnly>(type: "date", nullable: false),
                    NgayKetThuc = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NAMHOC__7DBADD74A20088A1", x => x.MaNamHoc);
                });

            migrationBuilder.CreateTable(
                name: "PHONGHOC",
                columns: table => new
                {
                    MaPhong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenPhong = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SucChua = table.Column<int>(type: "int", nullable: true),
                    ViTri = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PHONGHOC__20BD5E5BFA4C78DA", x => x.MaPhong);
                });

            migrationBuilder.CreateTable(
                name: "GIAOVIEN",
                columns: table => new
                {
                    MaGV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SDT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MaMonHoc = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GIAOVIEN__2725AEF30D4C2168", x => x.MaGV);
                    table.ForeignKey(
                        name: "FK_GIAOVIEN_MONHOC",
                        column: x => x.MaMonHoc,
                        principalTable: "MONHOC",
                        principalColumn: "MaMonHoc");
                });

            migrationBuilder.CreateTable(
                name: "HOCKY",
                columns: table => new
                {
                    MaHK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenHK = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaNamHoc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HOCKY__2725A6E74A815533", x => x.MaHK);
                    table.ForeignKey(
                        name: "FK_HOCKY_NAMHOC",
                        column: x => x.MaNamHoc,
                        principalTable: "NAMHOC",
                        principalColumn: "MaNamHoc");
                });

            migrationBuilder.CreateTable(
                name: "LOP",
                columns: table => new
                {
                    MaLop = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLop = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SiSo = table.Column<int>(type: "int", nullable: true),
                    MaNamHoc = table.Column<int>(type: "int", nullable: false),
                    MaGVCN = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LOP__3B98D273DE505D17", x => x.MaLop);
                    table.ForeignKey(
                        name: "FK_LOP_GIAOVIEN",
                        column: x => x.MaGVCN,
                        principalTable: "GIAOVIEN",
                        principalColumn: "MaGV");
                    table.ForeignKey(
                        name: "FK_LOP_NAMHOC",
                        column: x => x.MaNamHoc,
                        principalTable: "NAMHOC",
                        principalColumn: "MaNamHoc");
                });

            migrationBuilder.CreateTable(
                name: "HOCSINH",
                columns: table => new
                {
                    MaHS = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SDT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MaLop = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HOCSINH__2725A6EFD23810DA", x => x.MaHS);
                    table.ForeignKey(
                        name: "FK_HOCSINH_LOP",
                        column: x => x.MaLop,
                        principalTable: "LOP",
                        principalColumn: "MaLop");
                });

            migrationBuilder.CreateTable(
                name: "LICHHOC",
                columns: table => new
                {
                    MaLichHoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaLop = table.Column<int>(type: "int", nullable: false),
                    MaMonHoc = table.Column<int>(type: "int", nullable: false),
                    MaGV = table.Column<int>(type: "int", nullable: false),
                    MaPhong = table.Column<int>(type: "int", nullable: false),
                    MaHK = table.Column<int>(type: "int", nullable: false),
                    ThuTrongTuan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TietHoc = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LICHHOC__150EBC217240C5E5", x => x.MaLichHoc);
                    table.ForeignKey(
                        name: "FK_LH_GIAOVIEN",
                        column: x => x.MaGV,
                        principalTable: "GIAOVIEN",
                        principalColumn: "MaGV");
                    table.ForeignKey(
                        name: "FK_LH_HOCKY",
                        column: x => x.MaHK,
                        principalTable: "HOCKY",
                        principalColumn: "MaHK");
                    table.ForeignKey(
                        name: "FK_LH_LOP",
                        column: x => x.MaLop,
                        principalTable: "LOP",
                        principalColumn: "MaLop");
                    table.ForeignKey(
                        name: "FK_LH_MONHOC",
                        column: x => x.MaMonHoc,
                        principalTable: "MONHOC",
                        principalColumn: "MaMonHoc");
                    table.ForeignKey(
                        name: "FK_LH_PHONGHOC",
                        column: x => x.MaPhong,
                        principalTable: "PHONGHOC",
                        principalColumn: "MaPhong");
                });

            migrationBuilder.CreateTable(
                name: "PHANCONG_GIANGDAY",
                columns: table => new
                {
                    MaPC = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaGV = table.Column<int>(type: "int", nullable: false),
                    MaMonHoc = table.Column<int>(type: "int", nullable: false),
                    MaLop = table.Column<int>(type: "int", nullable: false),
                    MaHK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PHANCONG__2725E7E5CD65F42C", x => x.MaPC);
                    table.ForeignKey(
                        name: "FK_PC_GIAOVIEN",
                        column: x => x.MaGV,
                        principalTable: "GIAOVIEN",
                        principalColumn: "MaGV");
                    table.ForeignKey(
                        name: "FK_PC_HOCKY",
                        column: x => x.MaHK,
                        principalTable: "HOCKY",
                        principalColumn: "MaHK");
                    table.ForeignKey(
                        name: "FK_PC_LOP",
                        column: x => x.MaLop,
                        principalTable: "LOP",
                        principalColumn: "MaLop");
                    table.ForeignKey(
                        name: "FK_PC_MONHOC",
                        column: x => x.MaMonHoc,
                        principalTable: "MONHOC",
                        principalColumn: "MaMonHoc");
                });

            migrationBuilder.CreateTable(
                name: "DIEM",
                columns: table => new
                {
                    MaDiem = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHS = table.Column<int>(type: "int", nullable: false),
                    MaMonHoc = table.Column<int>(type: "int", nullable: false),
                    MaHK = table.Column<int>(type: "int", nullable: false),
                    DiemMieng = table.Column<double>(type: "float", nullable: true),
                    Diem15p = table.Column<double>(type: "float", nullable: true),
                    Diem1Tiet = table.Column<double>(type: "float", nullable: true),
                    DiemThi = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DIEM__33326025B95668FD", x => x.MaDiem);
                    table.ForeignKey(
                        name: "FK_DIEM_HOCKY",
                        column: x => x.MaHK,
                        principalTable: "HOCKY",
                        principalColumn: "MaHK");
                    table.ForeignKey(
                        name: "FK_DIEM_HOCSINH",
                        column: x => x.MaHS,
                        principalTable: "HOCSINH",
                        principalColumn: "MaHS");
                    table.ForeignKey(
                        name: "FK_DIEM_MONHOC",
                        column: x => x.MaMonHoc,
                        principalTable: "MONHOC",
                        principalColumn: "MaMonHoc");
                });

            migrationBuilder.CreateTable(
                name: "HOCPHI",
                columns: table => new
                {
                    MaHP = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHS = table.Column<int>(type: "int", nullable: false),
                    SoTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NgayDong = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaHK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HOCPHI__2725A6EC27BADE72", x => x.MaHP);
                    table.ForeignKey(
                        name: "FK_HP_HOCKY",
                        column: x => x.MaHK,
                        principalTable: "HOCKY",
                        principalColumn: "MaHK");
                    table.ForeignKey(
                        name: "FK_HP_HOCSINH",
                        column: x => x.MaHS,
                        principalTable: "HOCSINH",
                        principalColumn: "MaHS");
                });

            migrationBuilder.CreateTable(
                name: "TAIKHOAN",
                columns: table => new
                {
                    MaTK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaHS = table.Column<int>(type: "int", nullable: true),
                    MaGV = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TAIKHOAN__272500708AF1A976", x => x.MaTK);
                    table.ForeignKey(
                        name: "FK_TK_GIAOVIEN",
                        column: x => x.MaGV,
                        principalTable: "GIAOVIEN",
                        principalColumn: "MaGV");
                    table.ForeignKey(
                        name: "FK_TK_HOCSINH",
                        column: x => x.MaHS,
                        principalTable: "HOCSINH",
                        principalColumn: "MaHS");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DIEM_MaHK",
                table: "DIEM",
                column: "MaHK");

            migrationBuilder.CreateIndex(
                name: "IX_DIEM_MaHS",
                table: "DIEM",
                column: "MaHS");

            migrationBuilder.CreateIndex(
                name: "IX_DIEM_MaMonHoc",
                table: "DIEM",
                column: "MaMonHoc");

            migrationBuilder.CreateIndex(
                name: "IX_GIAOVIEN_MaMonHoc",
                table: "GIAOVIEN",
                column: "MaMonHoc");

            migrationBuilder.CreateIndex(
                name: "IX_HOCKY_MaNamHoc",
                table: "HOCKY",
                column: "MaNamHoc");

            migrationBuilder.CreateIndex(
                name: "IX_HOCPHI_MaHK",
                table: "HOCPHI",
                column: "MaHK");

            migrationBuilder.CreateIndex(
                name: "IX_HOCPHI_MaHS",
                table: "HOCPHI",
                column: "MaHS");

            migrationBuilder.CreateIndex(
                name: "IX_HOCSINH_MaLop",
                table: "HOCSINH",
                column: "MaLop");

            migrationBuilder.CreateIndex(
                name: "IX_LICHHOC_MaGV",
                table: "LICHHOC",
                column: "MaGV");

            migrationBuilder.CreateIndex(
                name: "IX_LICHHOC_MaHK",
                table: "LICHHOC",
                column: "MaHK");

            migrationBuilder.CreateIndex(
                name: "IX_LICHHOC_MaLop",
                table: "LICHHOC",
                column: "MaLop");

            migrationBuilder.CreateIndex(
                name: "IX_LICHHOC_MaMonHoc",
                table: "LICHHOC",
                column: "MaMonHoc");

            migrationBuilder.CreateIndex(
                name: "IX_LICHHOC_MaPhong",
                table: "LICHHOC",
                column: "MaPhong");

            migrationBuilder.CreateIndex(
                name: "IX_LOP_MaGVCN",
                table: "LOP",
                column: "MaGVCN");

            migrationBuilder.CreateIndex(
                name: "IX_LOP_MaNamHoc",
                table: "LOP",
                column: "MaNamHoc");

            migrationBuilder.CreateIndex(
                name: "IX_PHANCONG_GIANGDAY_MaGV",
                table: "PHANCONG_GIANGDAY",
                column: "MaGV");

            migrationBuilder.CreateIndex(
                name: "IX_PHANCONG_GIANGDAY_MaHK",
                table: "PHANCONG_GIANGDAY",
                column: "MaHK");

            migrationBuilder.CreateIndex(
                name: "IX_PHANCONG_GIANGDAY_MaLop",
                table: "PHANCONG_GIANGDAY",
                column: "MaLop");

            migrationBuilder.CreateIndex(
                name: "IX_PHANCONG_GIANGDAY_MaMonHoc",
                table: "PHANCONG_GIANGDAY",
                column: "MaMonHoc");

            migrationBuilder.CreateIndex(
                name: "IX_TAIKHOAN_MaGV",
                table: "TAIKHOAN",
                column: "MaGV");

            migrationBuilder.CreateIndex(
                name: "IX_TAIKHOAN_MaHS",
                table: "TAIKHOAN",
                column: "MaHS");

            migrationBuilder.CreateIndex(
                name: "UQ__TAIKHOAN__55F68FC092A2A6BA",
                table: "TAIKHOAN",
                column: "TenDangNhap",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DIEM");

            migrationBuilder.DropTable(
                name: "HOCPHI");

            migrationBuilder.DropTable(
                name: "LICHHOC");

            migrationBuilder.DropTable(
                name: "PHANCONG_GIANGDAY");

            migrationBuilder.DropTable(
                name: "TAIKHOAN");

            migrationBuilder.DropTable(
                name: "PHONGHOC");

            migrationBuilder.DropTable(
                name: "HOCKY");

            migrationBuilder.DropTable(
                name: "HOCSINH");

            migrationBuilder.DropTable(
                name: "LOP");

            migrationBuilder.DropTable(
                name: "GIAOVIEN");

            migrationBuilder.DropTable(
                name: "NAMHOC");

            migrationBuilder.DropTable(
                name: "MONHOC");
        }
    }
}
