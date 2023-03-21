using Microsoft.JSInterop;

namespace PetSiteAPI.Models.Dtos.Terry
{
   //將訂單先放在這個物件存放(記憶體)
    public class CreateOrderRequest
    {
        public int  CustomerId { get; set; }    
        public List<CreateOrderItem> Items { get; set; }
        public int? Membercoupon_Id { get; set; }
        public int? Discount { get; set; }
        public decimal Total => (decimal)(Items.Sum(x => x.SubTotal) - Discount);

        public OrderShippingPaymentInfo OrderShippingPaymentInfo { get; set; }

    }
}
