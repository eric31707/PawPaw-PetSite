using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetSiteAPI.Models.Dtos;
using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Controllers.ibubl
{
	[EnableCors("AllowAny")]
	[Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly PetSiteContext _context;
		public string CustomerAccount => User.Identity.Name;

		public CouponsController(PetSiteContext context)
        {
            _context = context;
        }

		// GET: api/Coupons
		//這邊尚需篩選出 可兌換(未過期、未到達使用上限) 才可顯示在全部折價券的清單頁
		//     [HttpGet]
		//     public IEnumerable<CouponDTO> GetCoupons()
		//     {
		//         var data= (from coupon in _context.Coupons
		//                    where coupon.Status == true && coupon.EndDate>DateTime.Today
		//                    select new CouponDTO
		//{
		//             CouponId = coupon.CouponId,
		//             DiscountCode = coupon.DiscountCode,
		//             DiscountName = coupon.DiscountName,
		//             Conditions = coupon.Conditions,
		//             DiscountAmount = coupon.DiscountAmount,
		//             UserType = coupon.UserType,
		//             StartDate = coupon.StartDate,
		//             EndDate = coupon.EndDate,   
		//             Qty = coupon.Qty,
		//             Status = coupon.Status,
		//         });

		//         return data;

		//     }
		[HttpGet]
		public IEnumerable<CouponDTO> GetCoupons()
		{
			var a = CustomerAccount;
			//拿到memberId(主鍵)
			var memberId = _context.Members.Where(x => x.Account == a).Select(x => x.MemberId).FirstOrDefault();

			var data = (
				//from coupon in _context.Coupons
				//		 join mc in _context.MemberCoupons
				//				   on coupon.CouponId equals mc.CouponId into gj
				//		from submc in gj.DefaultIfEmpty()
				//		where 
				//		submc == null && coupon.Status == true && coupon.EndDate > DateTime.Today||
				//		 submc.MemberId == memberId && coupon.Status == true && coupon.EndDate > DateTime.Today
				from coupon in _context.Coupons
				join memberCoupon in _context.MemberCoupons
					on new { CouponId = coupon.CouponId, MemberId = memberId } equals new { memberCoupon.CouponId, memberCoupon.MemberId } into temp
				from mc in temp.DefaultIfEmpty()
				where mc == null && coupon.Status == true && coupon.EndDate > DateTime.Today
				select new CouponDTO
						{
							CouponId = coupon.CouponId,
							DiscountCode = coupon.DiscountCode,
							DiscountName = coupon.DiscountName,
							Conditions = coupon.Conditions,
							DiscountAmount = coupon.DiscountAmount,
							UserType = coupon.UserType,
							StartDate = coupon.StartDate,
							EndDate = coupon.EndDate,
							Qty = coupon.Qty,
							Status = coupon.Status,
						});

			return data;

		}
		
        private bool CouponExists(int id)
        {
            return _context.Coupons.Any(e => e.CouponId == id);
        }
    }
}
