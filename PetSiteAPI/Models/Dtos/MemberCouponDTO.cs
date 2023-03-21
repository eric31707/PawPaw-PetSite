using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Models.Dtos
{
	public class MemberCouponDTO
	{
		public int MemberCouponId { get; set; }
		public int MemberId { get; set; }
		public int CouponId { get; set; }
		public bool UsedCoupon { get; set; }
		public bool Status { get; set; }
		public int? Qty { get; set; }
		//public virtual Coupon? Coupon { get; set; }
		//public virtual Member? Member { get; set; }
	}

	public class McpPostDTO
	{
		public int MemberCouponId { get; set; }
		public int MemberId { get; set; }
		public int CouponId { get; set; }
		public bool UsedCoupon { get; set; }
		public bool Status { get; set; }
		public int? Qty { get; set; }

		public string? DiscountCode { get; set; }
		public string? DiscountName { get; set; }
		public int? Conditions { get; set; }
		public int? DiscountAmount { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }

		//public ICollection<CouponDTO> Coupons { get; set; }
	}

}
