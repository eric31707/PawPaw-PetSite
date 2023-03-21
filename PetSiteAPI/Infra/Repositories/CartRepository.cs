using Microsoft.EntityFrameworkCore;
using PetSiteAPI.Models.Dtos.Terry;
using PetSiteAPI.Models.EFModels;
using PetSiteAPI.services.Interfaces;

namespace PetSiteAPI.Infra.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly PetSiteContext _db;
        public CartRepository(PetSiteContext db)
        {
            _db = db;
        }
        //判斷這個帳號是否有存在購物車
        public bool IsExists(string customerAccount)
        => _db.Carts.SingleOrDefault(x => x.MemberAccount == customerAccount) != null;
        //新增購物車
        public CartEntity CreateNewCart(string customerAccount)
        {
            var cart = new Cart { MemberAccount = customerAccount };
            _db.Carts.Add(cart);
            _db.SaveChanges();
            return Load(customerAccount);
        }
        //讀取現有的購物車
        public CartEntity Load(string customerAccount)
           => _db.Carts
            .Include(x => x.CartItems).ThenInclude(x => x.Product).ThenInclude(x=>x.Flavor)
            .Include(x => x.CartItems).ThenInclude(x => x.Product).ThenInclude(x=>x.Color)
            .Include(x => x.CartItems).ThenInclude(x => x.Product).ThenInclude(x=>x.Size)
             .Single(x => x.MemberAccount == customerAccount).ToEntity();
        //清空購物車
        public void EmptyCart(string customerAccount)
        {
            var cart = _db.Carts.SingleOrDefault(x => x.MemberAccount == customerAccount);
            if (cart == null) return;
            _db.Carts.Remove(cart);
            _db.SaveChanges();
        }
        /// <summary>
        /// 更新購物車紀錄主檔/明細檔
        /// </summary>
        /// <param name="cart"></param>
        public void Save(CartEntity cart)
        {
            //將cartEntity丟進去資料庫
            var cartEF = cart.ToEF();
            //找到那筆db的Cart明細檔
            var efInDb = _db.Carts.Include(x => x.CartItems).Single(x => x.CartId == cart.Id);
            //建立明細檔物件
            var efItemsInDb = efInDb.CartItems.ToList();
            //將db明細檔物件做比較  因為還沒存進去要先將記憶體的檔案跟資料庫的檔案做比較
            var deletedProducts=efItemsInDb.Select(x=>x.ProductId)
                .Except(cartEF.CartItems.Select(x=>x.ProductId)).ToList();

            foreach (var productId in deletedProducts)
            {
                var delItem = efInDb.CartItems.Single(x => x.ProductId == productId);
                // 不能直接remove item,要標註它已被刪除,才能正常執行
                _db.Entry(delItem).State = EntityState.Deleted;
            }

            // 新增商品或異動數量
            foreach (var item in cartEF.CartItems)
            {
                if (item.CartItemId == 0)
                {
                    efInDb.CartItems.Add(item);
                }
                else
                {
                    efInDb.CartItems.Single(x => x.CartItemId == item.CartItemId).Qty = item.Qty;
                }
            }

            _db.SaveChanges();

        }

       
    }
}
