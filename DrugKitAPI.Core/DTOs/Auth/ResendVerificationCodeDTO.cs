using System.ComponentModel.DataAnnotations;

namespace DrugKitAPI.Core.DTOs.Auth
{
    public class ResendVerificationCodeDTO
    {
        [Required]
        public string Email { get; set; }
    }
}
