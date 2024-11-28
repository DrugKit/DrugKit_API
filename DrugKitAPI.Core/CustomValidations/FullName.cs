using System.ComponentModel.DataAnnotations;

namespace DrugKitAPI.Core.CustomValidations
{
    public class FullName : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("null value");
            }
            string name = value.ToString();
            string[] arr = name.Split(' ');
            if (arr.Length >= 3)
                return ValidationResult.Success;

            return new ValidationResult("Name must be Full Name");
        }
    }
}