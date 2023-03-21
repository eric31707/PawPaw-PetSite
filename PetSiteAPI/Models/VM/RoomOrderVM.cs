using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Models.VM
{
    public class RoomOrderVM
    {
 
        public int RoomId { get; set; }
        public decimal Price { get; set; }
        public string Startdate { get; set; }
        public string Enddate { get; set; }
        public int Days { get; set; }

        public int CouponId { get; set; }
        public int MemberId { get; set; }

    }
}

   