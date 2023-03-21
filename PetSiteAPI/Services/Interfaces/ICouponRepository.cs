namespace PetSiteAPI.Services.Interfaces
{
    public interface ICouponRepository
    {
        bool isExist(string couponCode,string customerAccount);
        int? GetDiscountId(string couponCode);

        int GetDiscountAmount(string couponCode);
        int? GetMemberCouponId(string couponCode);
        int GetMemberCouponAmount(string couponCode);
    }
}
