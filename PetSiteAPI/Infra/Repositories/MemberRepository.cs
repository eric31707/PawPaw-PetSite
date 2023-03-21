using NuGet.Protocol;
using PetSiteAPI.Infra.ExtensionMethods.MembersExtension;
using PetSiteAPI.Models.Dtos;
using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Infra.Repositories
{
	public class MemberRepository
	{
		private readonly PetSiteContext _context;
		public MemberRepository(PetSiteContext context) {
			_context= context;
		}
		public MembersDTO GetByAccount(string account)
		{
			return _context.Members
				.SingleOrDefault(x => x.Account == account)
				.ToDto();

		}
	}
}
