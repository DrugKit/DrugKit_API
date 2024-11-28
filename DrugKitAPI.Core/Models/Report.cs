using System.ComponentModel.DataAnnotations;

namespace DrugKitAPI.Core.Models
{
    public class Report
    {
        public int Id { get; set; }
        [Required,MaxLength(1000)]
        public string Feedback { get; set; }
        [Required]
        public int MobileUserId { get; set; }
        public MobileUser MobileUser { get; set; }
    }
}
