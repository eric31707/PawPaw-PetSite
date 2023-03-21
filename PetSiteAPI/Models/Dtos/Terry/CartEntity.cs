using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Models.Dtos.Terry
{
    //購物車主檔與明細的聚合根
    public class CartEntity
    {
        //要進來這個聚合根的建構函式
        //聚合根一定要有ID,帳號，購物車明細
        public CartEntity(int id, string customerAccount, List<CartItemEntity> items)
        {
            this.Id = id;
            this.CustomerAccount = customerAccount;
            Items = items;
        }
        public int Id { get; set; }
        private string _CustomerAccount;

        public string CustomerAccount
        {
            get => _CustomerAccount;
            set => _CustomerAccount = string.IsNullOrEmpty(value) == false ? value : throw new Exception("CustomerAccount 不能是空值");
        }
        public List<CartItemEntity> Items { get; set; }
        public decimal Total => Items == null || Items.Count == 0 ? 0 : Items.Sum(x => x.SubTotal);
        public int? DiscountAmount { get; set; }
        public decimal TotalWithDiscount => Total - (DiscountAmount ?? 0);
        public bool AllowCheckOut => Items != null && Items.Count > 0;
        public IEnumerable<CartItemEntity> GetItems()
            => this.Items;
        /// <summary>
        /// 如果有相同商品的話就增加數量(從CartProductEntity增加數量進來聚合根，並在購物車明細檔做增減)
        /// </summary>
        /// <param name="product"></param>
        /// <param name="qty"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void AddItem(CartProductEntity product, int qty)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (qty <= 0) throw new ArgumentOutOfRangeException(nameof(qty));

            var item = Items.SingleOrDefault(x => x.Product.Id == product.Id);
            if (item == null)
            {
                var cartItem = new CartItemEntity(product, qty);
                this.Items.Add(cartItem);
            }
            else
            {
                item.Qty += qty;
            }
        }
        public void RemoveItem(int productId)
        {
            var item = Items.SingleOrDefault(x => x.Product.Id == productId);
            if (item == null) return;
            Items.Remove(item);
            
        }   
        public void UpdateQty(int productId,int newQty)
        {
            if(newQty<=0)
            {
                RemoveItem(productId);
                return;
            }
            var item=Items.SingleOrDefault(x=>x.Product.Id == productId);
            if (item == null) return;
            item.Qty = newQty;
        }

      
    }
    public static class CartExts
    {
        public static CartEntity ToEntity(this Cart source)
        {
            var items = source.CartItems.Select(x => x.ToEntity()).ToList();
            return new CartEntity(source.CartId, source.MemberAccount, items);
        }
        public static Cart ToEF(this CartEntity source)
        {
            var items = source.GetItems().Select(x => x.ToEF(source.Id)).ToList();
            return new Cart { CartId = source.Id, MemberAccount = source.CustomerAccount, CartItems = items };
        }
    }
}
