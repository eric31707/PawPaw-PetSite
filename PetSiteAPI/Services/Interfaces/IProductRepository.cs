using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.services.Interfaces
{
    public interface IProductRepository
    {
        Product Load(int productId, bool? status);
    }
}
