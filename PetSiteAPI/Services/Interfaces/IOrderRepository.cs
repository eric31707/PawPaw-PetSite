using PetSiteAPI.Models.Dtos.Terry;

namespace PetSiteAPI.Services.Interfaces
{
    public interface IOrderRepository
    {
        //建立一筆新的訂單及明細檔
        void CreateOrder(CreateOrderRequest request);
        //傳回訂單
        OrderEntity Load(string CustomerAccount);

        List<OrderEntity> LoadAll();
    }
}
