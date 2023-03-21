namespace PetSiteAPI.Models.VM
{
    public class  RoomOrderListVM
    {
        public int RoomOrderId { get; set; }
        public string? RoomName { get; set; }
        public string? RoomAddress { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Days { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
    }
}
