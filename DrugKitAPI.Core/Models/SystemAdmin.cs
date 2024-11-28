using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrugKitAPI.Core.Models
{
    public class SystemAdmin
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public List<BannedUser> BannedUsers { get; set; }
        public List<Donation> Donations { get; set; }
    }
}