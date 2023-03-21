using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Models.Dtos
{
    public class CouponDTO
    {
		public int CouponId { get; set; }
		public string? DiscountCode { get; set; }
		public string? DiscountName { get; set; }
		public int? Conditions { get; set; }
		public int? DiscountAmount { get; set; }
		public string? UserType { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public int? Qty { get; set; }
		public bool Status { get; set; }

		//public virtual ICollection<MemberCoupon>? MemberCoupons { get; set; }
		//public virtual ICollection<ProductOrder>? ProductOrders { get; set; }
		//public virtual ICollection<RoomOrder>? RoomOrders { get; set; }
	}
}
