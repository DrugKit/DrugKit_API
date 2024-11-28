using System.ComponentModel.DataAnnotations;

namespace DrugKitAPI.Core.CustomValidations
{
    public class PasswordComplexityAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;

            if (string.IsNullOrEmpty(password))
                return new ValidationResult("The password is required.");

            // Check minimum length
            if (password.Length < 8)
                return new ValidationResult("The password must be at least 8 characters long.");

            // Check for digit
            if (!password.Any(char.IsDigit))
                return new ValidationResult("The password must contain at least one digit.");

            // Check for non-alphanumeric character
            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                return new ValidationResult("The password must contain at least one special character.");

            // Check for uppercase letter
            if (!password.Any(char.IsUpper))
                return new ValidationResult("The password must contain at least one uppercase letter.");

            // Check for lowercase letter
            if (!password.Any(char.IsLower))
                return new ValidationResult("The password must contain at least one lowercase letter.");

            return ValidationResult.Success;
        }
    }
}
