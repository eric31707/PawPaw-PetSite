using Microsoft.EntityFrameworkCore;
using PetSiteAPI.Models.Dtos.Terry;
using PetSiteAPI.Models.EFModels;
using PetSiteAPI.services.Interfaces;

namespace PetSiteAPI.Infra.Repositories
{
    public class ProductRepository:IProductRepository
    {
        private readonly PetSiteContext _db;
        public ProductRepository(PetSiteContext db)
        {
            _db = db;
        }
        public Product Load(int productId,bool?status)
        {
            var query= _db.Products
                .Include(x=>x.Color)
                .Include(x=>x.Flavor)
                .Include(x=>x.Size)
                .Include(x=>x.ProductName)
                .Where(x=>x.ProductId==productId);
            
            var product = query.FirstOrDefault();
            return product;
        }
      
    }
}
