using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Models.Dtos
{
	public class GenderDTO
	{
		public int GenderId { get; set; }
		public string? GenderType { get; set; }
		public virtual ICollection<Adoption>? Adoptions { get; set; }
	}
}
