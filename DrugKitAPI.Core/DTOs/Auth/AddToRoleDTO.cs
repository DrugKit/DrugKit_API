using System.ComponentModel.DataAnnotations;

namespace DrugKitAPI.Core.DTOs.Auth
{
    public class AddToRoleDTO
    {
        [Required]
        public string userId { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
