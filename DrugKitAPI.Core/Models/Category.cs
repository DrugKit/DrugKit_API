using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrugKitAPI.Core.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required,MaxLength(200)]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public List<DrugCategory> DrugCategories { get; set; }
    }
}
