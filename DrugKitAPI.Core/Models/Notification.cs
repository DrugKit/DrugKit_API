using System.ComponentModel.DataAnnotations;

namespace DrugKitAPI.Core.Models
{
    public class Notification
    {
        public int Id { get; set; }
        [Required,MaxLength(200)]
        public string Message { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int MobileUserId { get; set; }
        public MobileUser MobileUser { get; set; }
    }
}