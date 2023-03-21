namespace PetSiteAPI.Models.Dtos
{
	public class MembersDTO
	{
		public int MemberId { get; set; }
		public string Name { get; set; }
		public string Account { get; set; }
		public string EncryptedPassword { get; set; }
		public string Mobile { get; set; }
		public string Email { get; set; }
		public DateTime BirthDate { get; set; }
		public string Address { get; set; }
		public bool IsConfirmed { get; set; }
		public string ConfirmCode { get; set; }
		public DateTime CreateDate { get; set; }
	}
}
