namespace PetSiteAPI.Models.Dtos.Terry
{
    public class OrderInfo
    {
        public OrderShippingPaymentInfo? orderShippingPaymentInfo { get; set; }
        public CouponDto? couponCode { get; set; }
    }
}
