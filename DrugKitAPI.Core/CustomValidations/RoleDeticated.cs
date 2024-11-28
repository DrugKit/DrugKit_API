using DrugKitAPI.Core.Const;
using System.ComponentModel.DataAnnotations;

namespace DrugKitAPI.Core.CustomValidations
{
    public class RoleDeticated : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("null value");
            }
            if (value.ToString() == Roles.User.ToString() || value.ToString() == Roles.Admin.ToString())
                return ValidationResult.Success;
            return new ValidationResult("should only User or Admin!");
        }
    }
}
