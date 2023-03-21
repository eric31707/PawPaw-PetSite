namespace PetSiteAPI.Models.Dtos.Terry
{
    public class OrderShippingPaymentInfo
    {
        public string Receiver { get; set; }
        public string Address { get; set; }
        public string  Mobile { get; set; }
        public int ShippingMethod { get; set; }
        public int PaymentMethod { get; set; }
    }
}
