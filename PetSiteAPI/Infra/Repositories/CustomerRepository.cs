using PetSiteAPI.Models.Dtos.Terry;
using PetSiteAPI.Models.EFModels;
using PetSiteAPI.Services.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace PetSiteAPI.Infra.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly PetSiteContext _db;
        public CustomerRepository(PetSiteContext db) 
        { 
         _db = db;
        }
        public int GetCustomerId(string customerAccount)
        => _db.Members.SingleOrDefault(x => x.Account == customerAccount).MemberId;


        public CustomerDto Load(string customerAccount)
        {
            return _db.Members.Single(x => x.Account == customerAccount).ToCustomerEntity();
        }
    }
}
