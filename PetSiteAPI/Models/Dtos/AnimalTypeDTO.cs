using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Models.Dtos
{
	public class AnimalTypeDTO
	{
		public int AnimalTypeId { get; set; }
		public string? AnimalType1 { get; set; }
		public virtual ICollection<Adoption>? Adoptions { get; set; }
	}
	
}
