using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DrugKitAPI.Core.DTOs.Donation
{
    public class UserRequestDonationDTO
    {
        [Required]
        public int MobileUserId { get; set; }

        [Required]
        public int DonationId { get; set; }

        [Required, MaxLength(500)]
        public string Message { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public IFormFile IdentityImage { get; set; }

        [Required]
        public IFormFile MedicalReportImage { get; set; }
    }
}
