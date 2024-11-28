using Microsoft.AspNetCore.Identity;
using DrugKitAPI.Core.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace DrugKitAPI.Core.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required,MaxLength(50),FullName]
        public string Name {  get; set; }
        public virtual SystemAdmin SystemAdmin { get; set; }
        public virtual MobileUser MobileUser { get; set; }
        public string? ResetPasswordCode { get; set; }

        // Add the ResetCodeExpiry property
        public DateTime? ResetCodeExpiry { get; set; }
        public string? VerificationCode { get; set; }

    }
}