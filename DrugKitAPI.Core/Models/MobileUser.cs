using System.ComponentModel.DataAnnotations;

namespace DrugKitAPI.Core.Models
{
    public class MobileUser
    {
        public int Id { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<Report> Reports { get; set; }
        public List<Notification> Notifications { get; set; }
        public BannedUser BannedUser { get; set; }
        public List<Donation> Donations { get; set; }
        public List<UserRequestedDonation> UserRequestedDonations { get; set; }
    }
}