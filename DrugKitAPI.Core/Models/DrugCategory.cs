namespace DrugKitAPI.Core.Models
{
    public class DrugCategory
    {
        public int DrugId { get; set; }
        public Drug Drug { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
