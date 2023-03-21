namespace PetSiteAPI.Models.Dtos
{
	public class EventDTO
	{
		public int EventId { get; set; }
		public int MemberId { get; set; }
		public string? Title { get; set; }
		public string? Image { get; set; }
		public string? EventsCategoryName { get; set; }
		public DateTime ApplyTimeStart { get; set; }
		public DateTime ApplyTimeEnd { get; set; }
		public DateTime ProgressTimeStart { get; set; }
		public DateTime ProgressTimeEnd { get; set; }
		public string? Location { get; set; }
		public int ApplicantLimitedQTY { get; set; }
		public int EventApplicantsQTY { get; set; }
		public string? EventInfo { get; set; }
		
	}
	public class EventDetailDTO
	{
        public int EventId { get; set; }
		public string? Image { get; set; }
        public string? Title { get; set; }
        public string? MemberName { get; set; }
        public string? EventsCategoryName { get; set; }
		//參加人數 例如20/50
        public string? EventApplicantsQTY { get; set; }
        public string? ApplyTimeStart { get; set; }
        public string? ApplyTimeEnd { get; set; }
        public string? ProgressTimeStart { get; set; }
        public string? ProgressTimeEnd { get; set; }
        public string? Location { get; set; }
        public string? EventInfo { get; set; }
		//看他有沒有參加過
		public bool Join { get; set; }
	}
}
