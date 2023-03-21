using Microsoft.AspNetCore.Components;
using PetSiteAPI.Models.Dtos.Terry;
using PetSiteAPI.Services.Interfaces;

namespace PetSiteAPI.Services
{
    public class OrderService
    {
       
        private readonly IOrderRepository _orderRepository;

        //public event Action<OrderService, int> OrderCreated;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        //將請求訂單的物件 建立成訂單
        public void PlaceOrder(CreateOrderRequest request)
        {
            _orderRepository.CreateOrder(request);
            //OnOrderCreated(orderId);

        }
        //protected virtual void OnOrderCreated(int orderId)
        //{
        //    if (OrderCreated != null)
        //    {
        //        OrderCreated(this, orderId);
        //    }
        //}

    }
}
