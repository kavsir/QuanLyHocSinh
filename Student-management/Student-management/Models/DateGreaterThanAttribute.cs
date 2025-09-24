using System.ComponentModel.DataAnnotations;

namespace Student_management.Validation
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
            {
                return new ValidationResult($"Không tìm thấy thuộc tính có tên '{_comparisonProperty}'.");
            }

            // Lấy giá trị của thuộc tính cần so sánh (ví dụ: NgayBatDau)
            var comparisonValueObject = property.GetValue(validationContext.ObjectInstance);

            // Kiểm tra kiểu dữ liệu của cả hai giá trị một cách an toàn
            if (value is not DateTime currentValue || comparisonValueObject is not DateTime comparisonValue)
            {
                // Nếu một trong hai không phải là DateTime, chúng ta bỏ qua việc xác thực ở đây
                return ValidationResult.Success!;
            }

            // Thực hiện so sánh
            if (currentValue.Date <= comparisonValue.Date)
            {
                return new ValidationResult(ErrorMessage ?? "Ngày kết thúc phải sau ngày bắt đầu.");
            }

            return ValidationResult.Success!;
        }
    }
}