using System.ComponentModel.DataAnnotations;

namespace DrugKitAPI.Core.Models
{
    public class UserRequestedDonation
    {
        public int MobileUserId { get; set; }
        public MobileUser MobileUser { get; set; }
        public int DonationId { get; set; }
        public Donation Donation { get; set; }
        [Required]
        public string IdentityPath { get; set; }
        [Required,MaxLength(1000)]
        public string Message { get; set; }
        [Required]
        public string MedicalReportPath { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required,Phone]
        public string PhoneNumber { get; set; }
    }
}
