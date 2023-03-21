using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Models.VM
{
    public class RoomVM
    {      
        public int RoomId { get; set; }
        public List<string> Image { get; set; }  
        public string Type { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }      

        public List<RoomOrderDate> DatesList { get; set; }
               
    }

    public class RoomOrderDate
    {
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string days { get; set; }
    }
}
