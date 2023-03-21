namespace PetSiteAPI.Models.VM
{
	public class ForECpay
	{
		public int RoomOrderId { get; set; }

		public string MemberName { get; set; }
		public string MemberEmail { get; set; }
		public string Mobile { get; set; }
        public string RoomName { get; set; }
        public string RoomAddress { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }		
		public int Total { get; set; }
	}

    public class MemOrdPar
    {
        public int MemberId { get; set; }
        public string orderdate { get; set; }
    }
}
