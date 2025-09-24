using System.ComponentModel.DataAnnotations;

namespace Student_management.Validation
{
    public class NoFutureDateOnlyAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateOnly dateValue)
            {
                if (dateValue > DateOnly.FromDateTime(DateTime.Now))
                {
                    return new ValidationResult(ErrorMessage ?? "Ngày không được ở tương lai.");
                }
            }
            return ValidationResult.Success;
        }
    }
}