using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Models.Dtos.Terry
{
    /// <summary>
    /// 產生訂單明細檔
    /// </summary>
    public class CreateOrderItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public decimal SubTotal => Price * Qty;
    }
    public static partial class CreateOrderItemExts
    {
        public static ProductOrderItem ToEF(this CreateOrderItem source)
        {
            return new ProductOrderItem
            {
                ProductId = source.ProductId,
                ProductName = source.ProductName,
                Price = source.Price,
                Qty = source.Qty,
                SubTotal=source.SubTotal,
            };
            
        }
        
    }
}
