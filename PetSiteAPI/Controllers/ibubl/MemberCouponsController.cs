using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetSiteAPI.Models.Dtos;
using PetSiteAPI.Models.EFModels;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace PetSiteAPI.Controllers.ibubl
{
	[EnableCors("AllowAny")]
	[Route("api/[controller]")]
    [ApiController]
    public class MemberCouponsController : ControllerBase
    {
        private readonly PetSiteContext _context;
		public string CustomerAccount => User.Identity.Name;

		public MemberCouponsController(PetSiteContext context)
        {
            _context = context;
        }

        // GET: api/MemberCoupons
        // 該會員可使用全部折價券列表
        //所有可兌換折價券在Coupon Controller
        [HttpGet]
        public IEnumerable<McpPostDTO> GetMemberCoupons(int memberid)
        {
			var a = CustomerAccount;
			//拿到memberId(主鍵)
			var memberId = _context.Members.Where(x => x.Account == a).Select(x => x.MemberId).FirstOrDefault();

			var data = (from m in _context.MemberCoupons
                            join c in _context.Coupons on m.CouponId  equals c.CouponId
                            where m.MemberId == memberId && m.UsedCoupon==false && m.Status== false&&
									c.Status == true && c.EndDate > DateTime.Today
						select new McpPostDTO
                            {
								MemberCouponId = m.MemberCouponId,
								MemberId = m.MemberId,
								CouponId = m.CouponId,
								UsedCoupon = m.UsedCoupon,
								Status = m.Status,
								Qty = m.Qty,
                                DiscountCode=c.DiscountCode,
                                DiscountName=c.DiscountName,
                                Conditions=c.Conditions,
                                DiscountAmount = c.DiscountAmount,
                                StartDate = c.StartDate,
                                EndDate= c.EndDate

                            });
            return data;
        }
		// GET: api/MemberCoupons/5
		// 在修改(使用折價券?!)特定會員折價券的時候會用到嗎?
		//     [HttpGet("{id}")]
		//     public async Task<ActionResult<MemberCouponDTO>> GetMemberCoupon(int id)
		//     {
		//         var memberCoupon = await _context.MemberCoupons.FindAsync(id);

		//         if (memberCoupon == null)
		//         {
		//             return null;
		//         }
		//MemberCouponDTO mbcouponDTO = new MemberCouponDTO
		//{
		//	MemberCouponId = memberCoupon.MemberCouponId,
		//	MemberId = memberCoupon.MemberId,
		//	CouponId = memberCoupon.CouponId,
		//	UsedCoupon = memberCoupon.UsedCoupon,
		//	Status = memberCoupon.Status,
		//	Qty = memberCoupon.Qty,

		//};
		//return mbcouponDTO;
		//     }
		[HttpGet("Used")]
		public IEnumerable<McpPostDTO> UsedMemberCoupons(int memberid)
		{

			var a = CustomerAccount;
			//拿到memberId(主鍵)
			var memberId = _context.Members.Where(x => x.Account == a).Select(x => x.MemberId).FirstOrDefault();


			var data = (from m in _context.MemberCoupons
						join c in _context.Coupons on m.CouponId equals c.CouponId
						where m.MemberId == memberId && m.UsedCoupon==true
						select new McpPostDTO
						{
							MemberCouponId = m.MemberCouponId,
							MemberId = m.MemberId,
							CouponId = m.CouponId,
							UsedCoupon = m.UsedCoupon,
							Status = m.Status,
							DiscountCode = c.DiscountCode,
							DiscountName = c.DiscountName,
							Conditions = c.Conditions,
							DiscountAmount = c.DiscountAmount,
							StartDate = c.StartDate,
							EndDate = c.EndDate

						});
			return data;
		}

		[HttpGet("Expired")]
        //過期 
		public IEnumerable<McpPostDTO> ExpiredMemberCoupons(int memberid)
		{
			var a = CustomerAccount;
			//拿到memberId(主鍵)
			var memberId = _context.Members.Where(x => x.Account == a).Select(x => x.MemberId).FirstOrDefault();


			var data = (from m in _context.MemberCoupons
						join c in _context.Coupons on m.CouponId equals c.CouponId
						where m.MemberId == memberId && m.Status == false && c.EndDate < DateTime.Today
						select new McpPostDTO
						{
							//MemberCouponId = m.MemberCouponId,
							//MemberId = m.MemberId,
							//CouponId = m.CouponId,
							//UsedCoupon = m.UsedCoupon,
							//Status = m.Status,
							//Qty = m.Qty,
							MemberCouponId = m.MemberCouponId,
							MemberId = m.MemberId,
							CouponId = m.CouponId,
							UsedCoupon = m.UsedCoupon,
							Status = m.Status,
							DiscountCode = c.DiscountCode,
							DiscountName = c.DiscountName,
							Conditions = c.Conditions,
							DiscountAmount = c.DiscountAmount,
							StartDate = c.StartDate,
							EndDate = c.EndDate

						});
			return data;
		}
        [HttpPost("UseMemberCoupon")]
        public async Task<string> EditProfile(string? couponcode, McpPostDTO membercouponDTO)
        {
            var a = CustomerAccount;
            var couponid = _context.Coupons.Where(x => x.DiscountCode == couponcode).Select(x => x.CouponId).FirstOrDefault();

            var memberId = _context.Members.Where(x => x.Account == a).Select(x => x.MemberId).FirstOrDefault();
            var mbcouponId = _context.MemberCoupons.Where(x => x.MemberId == memberId && x.CouponId == couponid).Select(x => x.MemberCouponId).FirstOrDefault();

			if (string.IsNullOrEmpty(couponcode))
			{
				return "無此優惠券";
			}

            MemberCoupon adp = _context.MemberCoupons.FirstOrDefault(x => x.MemberCouponId == mbcouponId);

			if (adp == null)
			{
                return "無此優惠券";
            }
            //MemberCoupon adp = _context.MemberCoupons.FirstOrDefault(x => x.MemberCouponId == MbCid);
            //adp.MemberCouponId = membercouponDTO.MemberCouponId;
            adp.CouponId = couponid;
            adp.MemberId = memberId;
            adp.UsedCoupon = true;
            adp.Status = false;
            adp.Qty = null;
            _context.SaveChanges();
            return "修改成功";
        }


        // PUT: api/MemberCoupons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        //修改已歸戶折價券狀態
        //-->已過期 EndDate
        //-->已使用UsedCoupon
        //--> 超過使用數量Qty
        public async Task<string> PutMemberCoupon(int Mbcid, MemberCouponDTO MbCouponDTO)
        {
			
            if (Mbcid != MbCouponDTO.MemberCouponId)
            {
                return "欲使用的折價券不正確";
            }

			var a = CustomerAccount;
			//拿到memberId(主鍵)
			var memberId = _context.Members.Where(x => x.Account == a).Select(x => x.MemberId).FirstOrDefault();
			var couponid = _context.MemberCoupons.Where(x => x.MemberCouponId == Mbcid).Select(x => x.CouponId).FirstOrDefault();

			MemberCoupon mbc = await _context.MemberCoupons.FindAsync(Mbcid);
			if (mbc == null)
			{
				return "欲使用的折價券不存在";
			}
			mbc.MemberId = memberId;
			mbc.CouponId = couponid;
			
			mbc.UsedCoupon = MbCouponDTO.UsedCoupon;
			mbc.Status = MbCouponDTO.Status;
			mbc.Qty = MbCouponDTO.Qty;

			_context.Entry(mbc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberCouponExists(Mbcid))
                {
                    return "欲使用的折價券不存在";
                }
                else
                {
                    throw;
                }
            }

            return "使用成功";
        }

        // POST: api/MemberCoupons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
		
		//滿額、 折扣金、起始與結束日期、Status、QTY
		//折扣代碼
		public async Task<string> PostMemberCoupon(MemberCouponDTO memberCoupon)
		{
			//todo list
			//確認memberId 跟CouponId 資料，再新增歸戶
			var a = CustomerAccount;
			//拿到memberId(主鍵)
			var memberId = _context.Members.Where(x => x.Account == a).Select(x => x.MemberId).FirstOrDefault();

			var cId = _context.Coupons.Where(x => x.CouponId == memberCoupon.CouponId).FirstOrDefault();
			var couponId = _context.Coupons.Where(x => x.CouponId == memberCoupon.CouponId).Select(x => x.CouponId).FirstOrDefault();
			var mbcid = _context.MemberCoupons.Where(x => x.MemberId == memberId && x.CouponId == couponId).FirstOrDefault();
			if (mbcid != null)
			{
				return "已兌換相同折價券";
			}
			if (cId == null)
			{
				return "欲兌換折價券錯誤";
			}
			//if (memberCoupon.CouponId== couponId && memberCoupon.MemberId==memberId)
			//{
			//	return "已有相同折價券";
			//}
			MemberCoupon mbcoupon = new MemberCoupon
			{
				MemberCouponId = memberCoupon.MemberCouponId,
				MemberId = memberId,
				CouponId = memberCoupon.CouponId,
				UsedCoupon = false,
				Status = false,
				Qty = null,

			};
			_context.MemberCoupons.Add(mbcoupon);
			await _context.SaveChangesAsync();

			return "折價券兌換成功";
		}



		private bool MemberCouponExists(int id)
        {
            return _context.MemberCoupons.Any(e => e.MemberCouponId == id);
        }

        //todo list
        //篩選折價券==> 兌換折價券,全部折價券,已使用,已過期
    }
}
