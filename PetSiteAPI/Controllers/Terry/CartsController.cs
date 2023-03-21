using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetSiteAPI.Infra.Repositories;
using PetSiteAPI.Models.Dtos.Terry;
using PetSiteAPI.Models.EFModels;
using PetSiteAPI.Services;

namespace PetSiteAPI.Controllers.Terry
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        
        private CartService cartService;
        private OrderService orderService;
        public string CustomerAccount=>User.Identity.Name;
        public CartsController(PetSiteContext db)
        {
            
            var cartRepo = new CartRepository(db);
            var productRepo = new ProductRepository(db);
            var customerRepo = new CustomerRepository(db);
            var couponRepo = new CouponRepository(db);
            var orderRepo = new OrderRepository(db);
            this.cartService = new CartService(cartRepo, productRepo,customerRepo,couponRepo);
            this.orderService = new OrderService(orderRepo);

        }

        //PUT: api/Carts/5
        //按下加入購物車呼叫此action
        //todo action
        [HttpPost]
        public async Task<string> AddItem(AddItemObj product)
        {
            //讀取現有帳號
            
            //將viewpage的值丟進資料庫
            //1.先判斷此購物車是否有資料
            //2.如果沒有資料，則新增一筆資料

            cartService.AddItem(CustomerAccount, product.Id, product.Qty);
            return "新增商品成功";
        }
      
        // GET: api/Carts
        //檢視購物車
        [HttpGet]
         
        public async Task<CartEntity> CartInfo()
        {
        
            var cart = cartService.Current(CustomerAccount);
            return cart;
        }
        // PUT: api/Carts/5
        //修改購物車內容
        [HttpPut("{productId}")]
        public async void UpdateItem(int productId,FirstStepIndexObj obj)
        {
            
            obj.NewQty = obj.NewQty <= 0 ? 0 : obj.NewQty;
            cartService.UpdateItem(CustomerAccount, productId, obj.NewQty);
        }
        // DELETE: api/Carts/5
        //刪除購物車內容
        [HttpDelete("{productId}")]
        public async Task<string> DeleteItem(int productId)
        {
            
            cartService.DeleteItem(CustomerAccount, productId);
            return "刪除商品成功";
        }
        //結帳
        [HttpPost("checkout")]
        public async Task<string> Checkout(OrderInfo orderInfo)
        {
            //找到這個使用者的購物車資料
           
            var cart = cartService.Current(CustomerAccount);
            if (cart.AllowCheckOut==false)
            {
                return "購物車內沒有商品";
            }
            var OrderShippingPaymentInfo = orderInfo.orderShippingPaymentInfo;
            var couponCode = orderInfo.couponCode;
            var mediator = new CartMediator(this.cartService, this.orderService);

            mediator.Checkout(this.CustomerAccount, OrderShippingPaymentInfo, couponCode);
            return "訂單建立成功";

        
        }
        //判斷coupon是否存在
        //並從這裡取值
        [HttpPost("couponCode")]
        public async Task<int> CheckCoupon(SecondStepIndexObj secondStepIndexObj)
        {

            if (string.IsNullOrEmpty(secondStepIndexObj.CouponCode)) return 0;
            var coupon = cartService.CheckCoupon(secondStepIndexObj.CouponCode, CustomerAccount);
            return coupon;
        }
        [HttpGet("cartQty")]
        public async Task<int> GetCartItemAmount()
        {

            //抓到現在的購物車

            var cart = cartService.Current(CustomerAccount);
            if(cart.Items.Count == 0)
            {
                return 0;
            }
            else
            {
                return cart.Items.Count;
            }
        }
        [HttpPost("MemberCoupon")]
        public async Task<int?> CheckMemberCoupon(SecondStepIndexObj secondStepIndexObj)
        {

            if (string.IsNullOrEmpty(secondStepIndexObj.CouponCode)) return 0;
            var memberCouponId = cartService.CheckMemberCouponId(secondStepIndexObj.CouponCode, CustomerAccount);
            return memberCouponId;
        }


    }
}
