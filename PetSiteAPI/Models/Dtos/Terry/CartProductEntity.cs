using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Models.Dtos.Terry
{
    /// <summary>
    /// 供購物車使用的類別，僅包含必要資訊
    /// </summary>
    public class CartProductEntity
    {
        public int Id { get; set; }
        //public string Image { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Flavor { get; set; }
        public decimal Price { get; set; }
        
        
    }
    public static partial class ProductExts
    {
        public static CartProductEntity ToCartProduct(this Product source)
        => new CartProductEntity
        {
            Id = source.ProductId,
            //Image = source.ProductImages.FirstOrDefault(x => x.ProductId == source.ProductId).Images,
            Name = source.Name,
            Size =source.Size.Name,
            Color=source.Color.Name,
            Flavor=source.Flavor.Name,
            Price=source.Price,
           
        };
    }
}
