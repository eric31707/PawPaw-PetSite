namespace PetSiteAPI.Models.VM
{
    public class PaymentVM
    {
        public int RoomOrderId { get; set; }   
        public string? MemberName { get; set; }
        public string? MemberEmail { get; set; }       
        public string? Mobile { get; set; }
        public string? RoomName { get; set; }
        public string? RoomAddress { get; set; }
        public decimal RoomFee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Days { get; set; }
        public decimal Total { get; set; }
        public string? Status { get; set; }

    }
}
