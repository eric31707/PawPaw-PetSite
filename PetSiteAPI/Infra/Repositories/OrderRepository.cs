using Microsoft.EntityFrameworkCore;
using PetSiteAPI.Models.EFModels;
using PetSiteAPI.Services.Interfaces;

namespace PetSiteAPI.Models.Dtos.Terry
{
    public class OrderRepository : IOrderRepository
    {
        private readonly PetSiteContext _db;
        public OrderRepository(PetSiteContext db)
        {
            _db = db;
        }
        public void CreateOrder(CreateOrderRequest request)
        {

            var order = new ProductOrder
            {
                MemberId = request.CustomerId,
                Total = request.Total,
                CreateDate = DateTime.Now,
                OrderStatus = "未處理",
                PaymentStatus = "已付款",
                ShipmentStatus = "備貨中",
                MembercouponId = request.Membercoupon_Id,
                Receiver = request.OrderShippingPaymentInfo.Receiver,
                Address = request.OrderShippingPaymentInfo.Address,
                Mobile = request.OrderShippingPaymentInfo.Mobile,
                PaymentId = request.OrderShippingPaymentInfo.PaymentMethod,
                ShipmentId = request.OrderShippingPaymentInfo.ShippingMethod,
                ProductOrderItems=request.Items.Select(x=>x.ToEF()).ToList(),
            };
            _db.ProductOrders.Add(order);
            _db.SaveChanges();
           
        }

        public OrderEntity Load(string CustomerAccount)
        {
            var latestOrder = _db.ProductOrders
                .Include(x=>x.ProductOrderItems).ThenInclude(x=>x.Product)
                .Include(x=>x.Shipment)
                .Include(x=>x.Member)
                .Include(x=>x.Payment)
                .Where(po => po.Member.Account == CustomerAccount).OrderByDescending(po => po.CreateDate).FirstOrDefault();
            return latestOrder.ToOrderEntity();
        }
        public List<OrderEntity> LoadAll(string CustomerAccount)
        {
            var AllOrder = _db.ProductOrders
                .Include(x => x.ProductOrderItems).ThenInclude(x => x.Product)
                .Include(x => x.Shipment)
                .Include(x => x.Member)
                .Include(x => x.Payment)
                .Where(x => x.Member.Account == CustomerAccount)
                .Select(x => x.ToOrderEntity());

            return AllOrder.ToList();
            
            
          
        }

        public List<OrderEntity> LoadAll()
        {
            throw new NotImplementedException();
        }
    }
}
