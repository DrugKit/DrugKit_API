using System.ComponentModel.DataAnnotations;

namespace DrugKitAPI.Core.Models
{
    public class Donation
    {
        public int Id { get; set; }
        [Required,MaxLength(50)]
        public string DrugName { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        [Required]
        public int MobileUserId { get; set; }
        public MobileUser MobileUser { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string IdentityPath { get; set; }
        public List<DonationImg> DonationImgs { get; set; }
        public List<UserRequestedDonation> UserRequestedDonations { get; set; }
        public int? SystemAdminId { get; set; }
        public SystemAdmin SystemAdmin { get; set; }
        public DateTime? AdminActionDate { get; set; }
        [Required]
        public bool IsConfirmed { get; set; }=false;
    }
}