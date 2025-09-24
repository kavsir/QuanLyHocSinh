using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Student_management.Validation
{
    public class FutureYearRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string yearRange)
            {
                // Sử dụng Regex để trích xuất năm đầu tiên một cách an toàn
                var match = Regex.Match(yearRange, @"^(\d{4})\s-\s\d{4}$");

                if (match.Success)
                {
                    // Lấy năm đầu tiên từ chuỗi (ví dụ: "2025" từ "2025 - 2026")
                    if (int.TryParse(match.Groups[1].Value, out int startYear))
                    {
                        int currentYear = DateTime.Now.Year;

                        // So sánh với năm hiện tại
                        if (startYear < currentYear)
                        {
                            return new ValidationResult(ErrorMessage ?? $"Năm học không được bắt đầu trong quá khứ (năm hiện tại là {currentYear}).");
                        }
                    }
                }
                // Nếu định dạng không khớp, hãy để attribute [RegularExpression] xử lý.
                // Chúng ta không báo lỗi ở đây để tránh thông báo trùng lặp.
            }

            return ValidationResult.Success;
        }
    }
}