using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrugKitAPI.Core.Models
{
    public class Drug
    {
        [Key]
        public int? Id { get; set; }
        public string Name { get; set; } = "NA";
        public decimal Price { get; set; } = 0.0m;
        public string Company { get; set; } = "NA";
        public string Description { get; set; } = "NA";
        public string Dosage_form { get; set; } = "NA";
        public string? Barcode { get; set; } = null;
        public string Entire_comp { get; set; } = "NA";
        public string Primary_comp { get; set; } = "NA";
        public string Side_effects { get; set; } = "NA";
        public string Value { get; set; } = "NA";
        public List<SideEffect> SideEffects { get; set; }
        public List<DrugCategory> DrugCategories { get; set; }
        public List<DrugImg> DrugImgs { get; set; }
    }
}
