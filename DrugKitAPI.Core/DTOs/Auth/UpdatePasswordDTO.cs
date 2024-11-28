using DrugKitAPI.Core.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace DrugKitAPI.Core.DTOs.Auth
{
    public class UpdatePasswordDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string CurrentPassword { get; set; }
        [Required, PasswordComplexity]
        public string NewPassword { get; set; }
    }
}