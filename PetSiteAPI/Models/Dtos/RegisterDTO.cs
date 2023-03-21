using NuGet.Common;
using PetSiteAPI.Infra.ExtensionMethods.MembersExtension;
using System;

namespace PetSiteAPI.Models.Dtos
{
	public class RegisterDTO
	{
		public const string SALT = "!@#$$DGTEGYT";
		public string Name { get; set; }
		public string Account { get; set; }
		public string Password { get; set; }
		public string EncryptedPassword
		{
			get
			{
				string salt = SALT;
				string result = HashUtility.ToSHA256(this.Password, salt);
				return result;
			}
		}
		public string? Mobile { get; set; }
		public string Email { get; set; }
		public string? BirthDate { get; set; }
		public string? Address { get; set; }


	}
}
