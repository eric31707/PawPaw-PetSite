using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Models.Dtos
{
    public class AllProductNameDTO
    {
        public int ProductNameId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Images { get; set; }
        public string Category { get; set; }
        public string Species { get; set; }
        public ICollection<ProductDTO> Products { get; set; }
    }
}
