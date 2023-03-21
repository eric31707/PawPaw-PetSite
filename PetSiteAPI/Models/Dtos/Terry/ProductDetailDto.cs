namespace PetSiteAPI.Models.Dtos.Terry
{
    public class ProductDetailDto
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }

        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? Flavor { get; set; }

        public string? Description { get; set; }
    }
   
}
