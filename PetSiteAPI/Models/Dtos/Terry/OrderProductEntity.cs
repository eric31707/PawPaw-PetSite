using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Models.Dtos.Terry
{
    public class OrderProductEntity
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
    public static partial class OrderProductExts
    {
        public static OrderProductEntity ToOrderProductEntity(this Product source)
        {
            return new OrderProductEntity
            {
                Id = source.ProductId,
                ProductName = source.Name,
                Price = source.Price
            };
        }
    }
}
