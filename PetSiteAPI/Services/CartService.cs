using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using NuGet.Protocol.Core.Types;
using PetSiteAPI.Infra.Repositories;
using PetSiteAPI.Models.Dtos.Terry;
using PetSiteAPI.Models.EFModels;
using PetSiteAPI.services.Interfaces;
using PetSiteAPI.Services.Interfaces;

namespace PetSiteAPI.Services
{
    public class CartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICouponRepository _couponRepository;
        public CartService(ICartRepository cartRepository, IProductRepository productRepository, ICustomerRepository customerRepository, ICouponRepository couponRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _couponRepository = couponRepository;

        }
        /// <summary>
        /// 結帳時觸發此事件
        /// </summary>
        public event Action<CartService, string> RequestCheckout;
        private OrderShippingPaymentInfo orderShippingPaymentInfo;
        private CouponDto coupon;
        public void Checkout(string customerAccount, OrderShippingPaymentInfo OrderShippingPaymentInfo, CouponDto couponCode)
        {
            if (string.IsNullOrEmpty(customerAccount)) throw new ArgumentNullException(nameof(customerAccount));
            if (OrderShippingPaymentInfo == null) throw new Exception("ShippingInfo not allow null");
            this.orderShippingPaymentInfo = OrderShippingPaymentInfo; // 先暫存,ToCreateOrderRequest會用到
            this.coupon = couponCode;
            OnRequestCheckout(customerAccount); // 觸發 RequestCheckout 事件
        }

        protected virtual void OnRequestCheckout(string customerAccount)
        {
            if (RequestCheckout != null)
            {
                RequestCheckout(this, customerAccount);
            }
        }
        /// <summary>
        /// 取得目前使用者的購物車
        /// </summary>
        /// <param name="customerAccount"></param>
        /// <returns></returns>
        public CartEntity Current(string customerAccount)
        {

            if (_cartRepository.IsExists(customerAccount))
            {
                return _cartRepository.Load(customerAccount);
            }
            else
            {
                return _cartRepository.CreateNewCart(customerAccount);
            }
        }
        /// <summary>
        /// 在購物車加入一項商品
        /// </summary>
        /// <param name="customerAccount"></param>
        /// <param name="productId"></param>
        /// <param name="qty"></param>
        public void AddItem(string customerAccount, int productId, int qty )
        {
            var cart = Current(customerAccount);
            var product = _productRepository.Load(productId, true);
            //從資料庫將product放在記憶體
            var cartProd = product.ToCartProduct();
            //從記憶體操作
            cart.AddItem(cartProd, qty);
            _cartRepository.Save(cart);
        }
        /// <summary>
        /// 更新購物車商品
        /// </summary>
        /// <param name="customerAccount"></param>
        /// <param name="productId"></param>
        /// <param name="newQty"></param>
        public void UpdateItem(string customerAccount, int productId, int newQty)
        {
            //取得使用者的購物車
            var cart = Current(customerAccount);
            //在這筆cartEntity更新
            cart.UpdateQty(productId, newQty);

            _cartRepository.Save(cart);
        }
        /// <summary>
        /// 從購物車刪除一項商品
        /// </summary>
        /// <param name="customerAccount"></param>
        /// <param name="productId"></param>
        public void RemoveItem(string customerAccount, int productId)
        {
            var cart = Current(customerAccount);
            cart.RemoveItem(productId);
            _cartRepository.Save(cart);
        }
        /// <summary>
        /// 清空購物車
        /// </summary>
        /// <param name="customerAccount"></param>
        public void EmptyCart(string customerAccount)
        {
            _cartRepository.EmptyCart(customerAccount);
        }
        /// <summary>
        /// 收集購物車資訊，建立一個請求建立一筆訂單的物件
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public CreateOrderRequest ToCreateOrderRequest(CartEntity cart)
        {
            //將cartItems存在 createOrderRequest
            List<CreateOrderItem> items = cart.GetItems()
                .Select(x => new CreateOrderItem
                {
                    ProductId = x.Product.Id,
                    ProductName = x.Product.Name,
                    Price = x.Product.Price,
                    Qty = x.Qty
                }).ToList();
            return new CreateOrderRequest
            {
                CustomerId = _customerRepository.GetCustomerId(cart.CustomerAccount),
                Items = items,
                Discount= _couponRepository.GetDiscountAmount(coupon.DiscountCode),
                Membercoupon_Id =_couponRepository.GetMemberCouponId(coupon.DiscountCode),
                OrderShippingPaymentInfo = this.orderShippingPaymentInfo
            };

        }

        public int CheckCoupon(string couponCode,string customerAccount)
        {

            var data = _couponRepository.isExist(couponCode, customerAccount);
            if (data == true)
            {
                return _couponRepository.GetDiscountAmount(couponCode);
            }
            else
            {
                return 0;
            }
             
        }
        public int? CheckMemberCouponId(string couponCode, string customerAccount)
        {

            var data = _couponRepository.isExist(couponCode, customerAccount);
            if (data == true)
            {
                return _couponRepository.GetMemberCouponId(couponCode);
            }
            else
            {
                return 0;
            }

        }
        public void DeleteItem(string customerAccount, int productId)
        {
           var cart = Current(customerAccount);
            cart.RemoveItem(productId);
            _cartRepository.Save(cart);
        }
    }
}
