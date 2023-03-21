using Microsoft.Build.Framework;
using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Models.Dtos
{
	public class AdoptionPostDTO
	{
        [Required]
        public int MemberId { get; set; }
        [Required]
		public string? PostType { get; set; }
		[Required]
		public string? AdoptName { get; set; }
		[Required]
		public int AnimalGenderId { get; set; }
		[Required]
		public int AnimalTypeId { get; set; }
		[Required]
		public string? AdoptTitle { get; set; }
		[Required]
		public int AnimalSizeId { get; set; }
		[Required]
		public string? AnimalColor { get; set; }
		[Required]
		public string? AreaAddress { get; set; }
		
		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }
		[Required]
		public string? AdoptDescription { get; set; }
		
		public IFormFile? Image1 { get; set; }
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
