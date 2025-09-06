using Student_management.Models;
using System.Text.RegularExpressions;

namespace Student_management.Services
{
    public class NamHocService
    {
        private readonly QuanLyHocSinhContext _context;

        public NamHocService(QuanLyHocSinhContext context)
        {
            _context = context;
        }

        public (bool ok, int start, int end, string normalized, string error) ValidateAndNormalizeTenNamHoc(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return (false, 0, 0, null, "Vui lòng nhập Tên năm học (vd: 2025-2026).");

            var s = input.Trim().Replace("–", "-").Replace("—", "-");
            s = Regex.Replace(s, @"\s*", "");

            var m = Regex.Match(s, @"^(?<start>\d{4})-(?<end>\d{4})$");
            if (!m.Success)
                return (false, 0, 0, null, "Định dạng phải là YYYY-YYYY (vd: 2025-2026).");

            int start = int.Parse(m.Groups["start"].Value);
            int end = int.Parse(m.Groups["end"].Value);

            if (end != start + 1)
                return (false, start, end, null, "Tên năm học phải liên tiếp: năm sau = năm trước + 1.");

            if (start < 2000 || end > 2100)
                return (false, start, end, null, "Năm học ngoài khoảng cho phép (2000–2100).");

            return (true, start, end, $"{start}-{end}", null);
        }

        public void ValidateBusiness(Namhoc nh, (bool ok, int start, int end, string normalized, string error) yr, bool isEdit = false)
        {
            if (nh.NgayBatDau == default || nh.NgayKetThuc == default)
            {
                throw new ArgumentException("Vui lòng nhập đầy đủ ngày bắt đầu và ngày kết thúc.");
            }

            if ((nh.NgayKetThuc - nh.NgayBatDau).TotalDays < 270)
                throw new ArgumentException("Thời gian năm học phải dài hơn 9 tháng (≥ 270 ngày).");

            if (yr.ok)
            {
                if (nh.NgayBatDau.Year != yr.start || nh.NgayKetThuc.Year != yr.end)
                    throw new ArgumentException($"Năm trong ngày không khớp với tên năm học {yr.normalized}.");
            }

            bool exist = isEdit
                ? _context.Namhocs.Any(x => x.TenNamHoc == yr.normalized && x.MaNamHoc != nh.MaNamHoc)
                : _context.Namhocs.Any(x => x.TenNamHoc == yr.normalized);

            if (exist)
                throw new ArgumentException("Tên năm học đã tồn tại.");

            if (!isEdit)
            {
                var last = _context.Namhocs
                    .OrderByDescending(x => x.NgayBatDau)
                    .FirstOrDefault();

                if (last != null)
                {
                    var yrLast = ValidateAndNormalizeTenNamHoc(last.TenNamHoc);
                    if (yrLast.ok && yr.start != yrLast.end)
                        throw new ArgumentException($"Tên năm học phải nối tiếp {last.TenNamHoc}.");
                }
            }
        }
    }
}
