using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DrugKitAPI.Core.DTOs.Donation
{
    public class DonationDTO
    {
        [Required, MaxLength(50)]
        public string DrugName { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        [Required]
        public int MobileUserId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public IFormFile IdentityImage { get; set; }
        public List<IFormFile> DonationImages { get; set; } = new List<IFormFile>();
    }
}
