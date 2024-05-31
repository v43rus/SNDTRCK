namespace SNDTRCK.Models
{
    public class CheckoutViewModel
    {
        public decimal? OrderValue { get; set; }

        public int? OrderQuantity { get; set; }

        public OrderViewModel? Order { get; set; }
    }
}
