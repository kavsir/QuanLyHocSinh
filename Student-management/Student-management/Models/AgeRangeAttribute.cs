using System.ComponentModel.DataAnnotations;

namespace Student_management.Validation
{
    public class AgeRangeAttribute : ValidationAttribute
    {
        private readonly int _minAge;
        private readonly int _maxAge;

        public AgeRangeAttribute(int minAge, int maxAge)
        {
            _minAge = minAge;
            _maxAge = maxAge;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateOnly birthDate)
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                var age = today.Year - birthDate.Year;

                // Điều chỉnh lại tuổi nếu chưa đến sinh nhật trong năm nay
                if (birthDate > today.AddYears(-age))
                {
                    age--;
                }

                if (age < _minAge || age > _maxAge)
                {
                    return new ValidationResult(ErrorMessage ?? $"Tuổi phải nằm trong khoảng từ {_minAge} đến {_maxAge}.");
                }
            }

            return ValidationResult.Success;
        }
    }
}