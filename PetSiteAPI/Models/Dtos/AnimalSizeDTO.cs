using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Models.Dtos
{
	public class AnimalSizeDTO
	{
		public int AnimalSizeId { get; set; }
		public string? AnimalSize1 { get; set; }
		public virtual ICollection<Adoption>? Adoptions { get; set; }
	}
}
