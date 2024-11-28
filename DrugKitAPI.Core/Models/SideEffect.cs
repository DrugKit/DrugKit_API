namespace DrugKitAPI.Core.Models
{
    public class SideEffect
    {
        public int EffectId { get; set; }
        public string Effect { get; set; }
        public int DrugId { get; set; }
        public Drug Drug { get; set; }
    }
}