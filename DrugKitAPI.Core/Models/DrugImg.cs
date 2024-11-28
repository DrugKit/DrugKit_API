namespace DrugKitAPI.Core.Models
{
    public class DrugImg
    {
        public string ImagePath { get; set; }
        public int DrugId { get; set; }
        public Drug Drug { get; set; }
    }
}
