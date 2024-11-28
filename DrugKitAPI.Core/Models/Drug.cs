using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrugKitAPI.Core.Models
{
    public class Drug
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; } = "Unknown";
        public string arabic { get; set; }
        public decimal price { get; set; } = 0.0m;
        public string? active { get; set; } = "Not specified";
        public string company { get; set; } = "Unknown company";
        public string description { get; set; } = "No description";
        public int units { get; set; } = 0;
        public string dosage_form { get; set; } = "Not specified";
        public string? barcode { get; set; } = null;
        public string imported { get; set; } = "No";
        public decimal? oldprice { get; set; } = null;
        public List<SideEffect> SideEffects { get; set; }
        public List<DrugCategory> DrugCategories { get; set; }
        public List<DrugImg> DrugImgs { get; set; }
    }
}
