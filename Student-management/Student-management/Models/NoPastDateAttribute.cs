using System.ComponentModel.DataAnnotations;

namespace Student_management.Validation
{
    public class NoPastDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            // Sử dụng pattern matching để kiểm tra kiểu dữ liệu một cách an toàn
            if (value is DateTime dateValue)
            {
                if (dateValue.Date < DateTime.Now.Date)
                {
                    return new ValidationResult(ErrorMessage ?? "Ngày không được là một ngày trong quá khứ.");
                }
            }
            // Nếu giá trị không phải là DateTime, hãy để các attribute khác (như [Required]) xử lý.
            return ValidationResult.Success!;
        }
    }
}