namespace PetSiteAPI.Models.Dtos
{
	public class CardDTO
	{
		public int EventId { get; set; }
		public int MemberId { get; set; }
		public string? Image { get; set; }
		public string? Title { get; set; }
		public string? Location { get; set; }
        public string? ApplyTimeEnd { get; set; }
        public string? ProgressTimeStart { get; set; }
	}
	public class PostEventJoinDTO
	{
		public int EventId { get; set; }
		public int TotalPeople { get; set; }
		public int TotalPet { get; set; }
		public string? Status { get; set; }
		public DateTime CreateDate { get; set; }
	}
	//get
	public class WhoJoin
	{
		public int MemberId { get; set; }
		public string? Name { get; set; }
		public int TotalPeople { get; set; }
		public int TotalPet { get; set; }
		public string? Status { get; set; }
	}
	//post 活動新增用的
    public class EventCreate
    {	
        public string? Title { get; set; }
        public IFormFile? Image { get; set; }
		public string? EventsCategoryName { get; set; }
		public string? ApplyTimeStart { get; set; }
		public string? ApplyTimeEnd { get; set; }
		public string? ProgressTimeStart { get; set; }
		public string? ProgressTimeEnd { get; set; }
		public string? Location { get; set; }
		public int ApplicantLimitedQTY { get; set; }
		public string? EventInfo { get; set; }
	}
    //get
    public class Join
	{
		public int EventId { get; set; }
		public string? Title { get; set; }
		public string? Status { get; set; }
		public string? EventsCategoryName { get; set; }
		public string? EventHerf { get; set; }
	}

    //get請求，活動中心--發起的活動
    public class CreateEventDto
	{
		public int EventId { get; set; }
        public string? Title { get; set; }
        //參加人數 例如20/50
        public string? EventApplicantsQTY { get; set; }
        public string? EventHerf { get; set; }
    }
    //put 前端傳入使用者ID、活動ID、bool報名成功與否
    public class EditEventStatusDto
    {
        public int EventId { get; set; }
        public int MemberId { get; set; }
		public bool? Status { get; set; }
	}
	//delete 前端傳入活動ID和使用者ID，刪除自己的參加活動
	public class DeleteEventJoinDto
	{
		public int EventId { get; set; }

	}

	//get 活動編輯用的
	public class OneEventForEdit
	{
		public string? Title { get; set; }
		public string? Image { get; set; }
		public string? EventsCategoryName { get; set; }
		public DateTime? ApplyTimeStart { get; set; }
		public DateTime? ApplyTimeEnd { get; set; }
		public DateTime? ProgressTimeStart { get; set; }
		public DateTime? ProgressTimeEnd { get; set; }
		public string? Location { get; set; }
		public int ApplicantLimitedQTY { get; set; }
		public string? EventInfo { get; set; }
	}
    //put 活動編輯用的
    public class EventEdit
    {
		public int EventId { get; set; }
		public string? Title { get; set; }
        public IFormFile? Image { get; set; }
        public string? EventsCategoryName { get; set; }
        public string? ApplyTimeStart { get; set; }
        public string? ApplyTimeEnd { get; set; }
        public string? ProgressTimeStart { get; set; }
        public string? ProgressTimeEnd { get; set; }
        public string? Location { get; set; }
        public int ApplicantLimitedQTY { get; set; }
        public string? EventInfo { get; set; }
    }

}
