using DrugKitAPI.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace DrugKitAPI.Core.DTOs.Category
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
