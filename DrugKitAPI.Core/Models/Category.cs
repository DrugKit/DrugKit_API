using System.ComponentModel.DataAnnotations;

namespace DrugKitAPI.Core.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required,MaxLength(200)]
        public string Name { get; set; }
        public List<DrugCategory> DrugCategories { get; set; }
    }
}
