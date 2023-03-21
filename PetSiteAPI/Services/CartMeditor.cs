using PetSiteAPI.Models.Dtos.Terry;

namespace PetSiteAPI.Services
{
    public class CartMediator
    {
        private readonly CartService _cart;
        private readonly OrderService _order;
        public CartMediator(CartService cart,OrderService order)
        {
            _cart = cart;
            _order = order;
            _cart.RequestCheckout += _cart_RequestCheckout;
            
            
        }

        public void Checkout(string customerAccount, OrderShippingPaymentInfo orderShippingPaymentInfo, CouponDto couponCode)
        {
            _cart.Checkout(customerAccount, orderShippingPaymentInfo, couponCode);
        }

        private void _cart_RequestCheckout(CartService sender, string customerAccount)
        {
            CartEntity cart = _cart.Current(customerAccount);

            CreateOrderRequest request = _cart.ToCreateOrderRequest(cart);
            _order.PlaceOrder(request);
            _cart.EmptyCart(customerAccount);

        }
       
        
       

    }
}
