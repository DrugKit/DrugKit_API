using DrugKitAPI.Core.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace DrugKitAPI.Core.DTOs.Auth
{
    public class ChangePasswordDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string ResetCode { get; set; }
        [Required, PasswordComplexity]
        public string NewPassword { get; set; }
    }
}
