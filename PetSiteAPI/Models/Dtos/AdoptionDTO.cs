using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Models.Dtos
{
	public class AdoptionDTO
	{
		public int AdoptionId { get; set; }
		public int MemberId { get; set; }
		public string? PostType { get; set; }
		public string? AdoptName { get; set; }
		public int AnimalGenderId { get; set; }
		public int AnimalTypeId { get; set; }
		public string? AdoptTitle { get; set; }
		public int AnimalSizeId { get; set; }
		public string? AnimalColor { get; set; }
		public string? AreaAddress { get; set; }
		public decimal? Latitude { get; set; }
		public decimal? Longitude { get; set; }
		public string? AdoptDescription { get; set; }
		public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public string? Image4 { get; set; }
        public string? Image5 { get; set; }
        public int? ContainNumber { get; set; }
		public bool? OpenAdopt { get; set; }
		public bool? Ligation { get; set; }
		public bool? Deworming { get; set; }
		public bool? Vaccination { get; set; }
		public bool? Triple { get; set; }
		public bool? AnimalChip { get; set; }
		public bool? Fiv { get; set; }
		public bool? FeLv { get; set; }

	}

    public class AdoptionDetailDTO
    {
        public int AdoptionId { get; set; }
        public string? Membermail { get; set; }

		public string? MemberName { get; set; }
        public string? MemberAccount { get; set; }
        public string? PostType { get; set; }
        public string? AdoptName { get; set; }
        public string? Gender { get; set; }
        public string? Type { get; set; }
        public string? AdoptTitle { get; set; }
        public string? Size { get; set; }
        public string? AnimalColor { get; set; }
        public string? AreaAddress { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? AdoptDescription { get; set; }
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public string? Image4 { get; set; }
        public string? Image5 { get; set; }
        public int? ContainNumber { get; set; }
        public bool? OpenAdopt { get; set; }
        public bool? Ligation { get; set; }
        public bool? Deworming { get; set; }
        public bool? Vaccination { get; set; }
        public bool? Triple { get; set; }
        public bool? AnimalChip { get; set; }
        public bool? Fiv { get; set; }
        public bool? FeLv { get; set; }

    }



}
