using Microsoft.EntityFrameworkCore;
using PetSiteAPI.Models.Dtos.Terry;
using PetSiteAPI.Models.EFModels;
using PetSiteAPI.Services.Interfaces;

namespace PetSiteAPI.Infra.Repositories
{
    public class CouponRepository : ICouponRepository
    {

        private readonly PetSiteContext _db;
        public CouponRepository(PetSiteContext db)
        {
            _db = db;
        }


        public bool isExist(string couponCode, string customerAccount)
        {
            var data = _db.MemberCoupons.Include(x => x.Member).Include(x => x.Coupon);
            return data.Where(x => x.Member.Account == customerAccount).Where(x => x.Coupon.DiscountCode == couponCode).SingleOrDefault() != null;
        }
        public int? GetDiscountId(string couponCode)
        {
            if (string.IsNullOrEmpty(couponCode))
            {
                return null;
            }
            return _db.Coupons.Where(x => x.DiscountCode == couponCode).SingleOrDefault().CouponId;

        }

        public int GetDiscountAmount(string couponCode)
        {
            if (string.IsNullOrEmpty(couponCode))
            {
                return 0;
            }

            var data = _db.MemberCoupons.Include(x => x.Coupon).Where(x => x.Coupon.DiscountCode == couponCode).FirstOrDefault();
            if (data != null)
            {
                return data.Coupon.DiscountAmount;
            }
            return 0;
            
        }

        public int? GetMemberCouponId(string couponCode)
        {
            if (string.IsNullOrEmpty(couponCode))
            {
                return null;
            }

            var data = _db.MemberCoupons.Include(x => x.Coupon).Where(x => x.Coupon.DiscountCode == couponCode).FirstOrDefault();
            if (data != null)
            {
                return data.CouponId;
            }
            
            return null;
        }

        public int GetMemberCouponAmount(string couponCode)
        {
            throw new NotImplementedException();
        }
    }
}
