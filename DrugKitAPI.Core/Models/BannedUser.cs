using System.ComponentModel.DataAnnotations;

namespace DrugKitAPI.Core.Models
{
    public class BannedUser
    {
        [Key]
        public int MobileUserId { get; set; }
        public MobileUser MobileUser { get; set; }
        public int SystemAdminId { get; set; }
        public SystemAdmin SystemAdmin { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}