using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Models.Dtos.Terry
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string CustomerAccount { get; set; }
    }
    public static class CustomerExts
    {
        public static CustomerDto ToCustomerEntity(this Member source)
            => new CustomerDto { Id = source.MemberId, CustomerAccount = source.Account };
    }
}
