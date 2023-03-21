using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Models.Dtos
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Flavor { get; set; }
        public int Quantity { get; set; }
        public string Species { get; set; }
    }
}
