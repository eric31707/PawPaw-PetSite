using PetSiteAPI.Models.Dtos.Terry;

namespace PetSiteAPI.Services.Interfaces
{
    public interface ICustomerRepository
    {
        int GetCustomerId(string customerAccount);

        CustomerDto Load(string customerAccount);
    }
}
