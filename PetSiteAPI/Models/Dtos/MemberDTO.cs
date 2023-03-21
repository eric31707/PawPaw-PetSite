using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Models.Dtos
{
	public class MemberDTO
	{
		public int MemberId { get; set; }
		public string? Name { get; set; }
		public string? Account { get; set; }
		public string? EncryptedPassword { get; set; }
		public string? Mobile { get; set; }
		public string? Email { get; set; }
		public string? BirthDate { get; set; }
		public string? Address { get; set; }
		public bool IsConfirmed { get; set; }
		public string? ConfirmCode { get; set; }
		public DateTime CreateDate { get; set; }

		public virtual ICollection<Adoption>? Adoptions { get; set; }
		public virtual ICollection<ApplyEvent>? ApplyEvents { get; set; }
		public virtual ICollection<MemberCoupon>? MemberCoupons { get; set; }
		public virtual ICollection<MemberTag>? MemberTags { get; set; }
		public virtual ICollection<ProductOrder>? ProductOrders { get; set; }
		public virtual ICollection<RoomCartItem>? RoomCartItems { get; set; }
		public virtual ICollection<RoomOrder>? RoomOrders { get; set; }

	}
}
