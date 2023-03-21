using PetSiteAPI.Models.Dtos.Terry;

namespace PetSiteAPI.services.Interfaces
{
    public interface ICartRepository
    {
        /// <summary>
        /// 傳回這個帳戶是否有存在的購物車
        /// </summary>
        /// <param name="customerAccount"></param>
        /// <returns></returns>
        bool IsExists(string customerAccount);
        /// <summary>
        /// 建立新的購物車主檔紀錄
        /// </summary>
        /// <param name="customerAccount"></param>
        /// <returns></returns>
        CartEntity CreateNewCart(string customerAccount);
        
        CartEntity Load(string customerAccount);
        void EmptyCart(string customerAccount);
        void Save(CartEntity cart);
    }
}
