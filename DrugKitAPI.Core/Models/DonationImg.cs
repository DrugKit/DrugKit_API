namespace DrugKitAPI.Core.Models
{
    public class DonationImg
    {
        public string ImagPath { get; set; }
        public int DonationId { get; set; }
        public Donation Donation { get; set; }
    }
}
