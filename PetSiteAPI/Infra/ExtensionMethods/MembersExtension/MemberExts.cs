using PetSiteAPI.Models.Dtos;
using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Infra.ExtensionMethods.MembersExtension
{
	public static class MemberExts
	{
		public static MembersDTO ToDto(this Member entity)
		{
			return entity == null
				? null
				: new MembersDTO
				{
					MemberId = entity.MemberId,
					Account = entity.Account,
					EncryptedPassword = entity.EncryptedPassword,
					Email = entity.Email,
					Name = entity.Name,
					Mobile = entity.Mobile,
					IsConfirmed = entity.IsConfirmed,
					ConfirmCode = entity.ConfirmCode
				};
		}
	}
}
