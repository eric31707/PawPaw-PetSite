using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using PetSiteAPI.Models.Dtos;
using PetSiteAPI.Models.EFModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace PetSiteAPI.Controllers.ibubl
{
	[EnableCors("AllowAny")]
	[Route("api/[controller]")]
    [ApiController]
    public class AdoptionsController : ControllerBase
    {
        private readonly PetSiteContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
		public string CustomerAccount => User.Identity.Name;

		public AdoptionsController(PetSiteContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
		{
			_context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/Adoptions
        [HttpGet]
        public async Task<IEnumerable<AdoptionDetailDTO>> GetAdoptions()
        {
			return _context.Adoptions.Select(x => new AdoptionDetailDTO
			{
				AdoptionId = x.AdoptionId,
				MemberName = x.Member.Name,
				Membermail = x.Member.Email,
				MemberAccount = x.Member.Account,
                PostType = x.PostType,
				AdoptName = x.AdoptName,
				AdoptTitle = x.AdoptTitle,
				AnimalColor = x.AnimalColor,
				AdoptDescription = x.AdoptDescription,
				AreaAddress = x.AreaAddress,
				//Latitude = x.Latitude,
				//Longitude = x.Longitude,
				Image1 = x.Image1,
				Image2 = x.Image2,
				Image3 = x.Image3,
				Image4 = x.Image4,
				Image5 = x.Image5,
				Gender = x.AnimalGender.GenderType,
				Size = x.AnimalSize.AnimalSize1,
				Type = x.AnimalType.AnimalType1,
				ContainNumber = x.ContainNumber,
				OpenAdopt = x.OpenAdopt,
				Ligation = x.Ligation,
				Deworming = x.Deworming,
				Vaccination = x.Vaccination,
				Triple = x.Triple,
				AnimalChip = x.AnimalChip,
				Fiv = x.Fiv,
				FeLv = x.FeLv,
			});
		}

		[HttpGet("Filter")]
		public IEnumerable<AdoptionDetailDTO> GetFilterAdoptions(string? title,string? postType, string? gender, string? size, string? type, string? areaAddress, string? color)
		{
			var data = _context.Adoptions.OrderByDescending(x => x.AdoptionId).Include(x => x.Member)
				.Include(x => x.AnimalGender).Include(x => x.AnimalSize)
				.Include(x => x.AnimalType).Select(x => new AdoptionDetailDTO
				{
					AdoptionId = x.AdoptionId,
					MemberName = x.Member.Name,
					Membermail = x.Member.Email,
					MemberAccount = x.Member.Account,
                    PostType = x.PostType,
					AdoptName = x.AdoptName,
					AdoptTitle = x.AdoptTitle,
					AnimalColor = x.AnimalColor,
					AdoptDescription = x.AdoptDescription,
					AreaAddress = x.AreaAddress,
					Latitude = x.Latitude,
					Longitude = x.Longitude,
					Image1 = x.Image1,
					Image2 = x.Image2,
					Image3 = x.Image3,
					Image4 = x.Image4,
					Image5 = x.Image5,
					Gender = x.AnimalGender.GenderType,
					Size = x.AnimalSize.AnimalSize1,
					Type = x.AnimalType.AnimalType1,
					ContainNumber = x.ContainNumber,
					OpenAdopt = x.OpenAdopt,
					Ligation = x.Ligation,
					Deworming = x.Deworming,
					Vaccination = x.Vaccination,
					Triple = x.Triple,
					AnimalChip = x.AnimalChip,
					Fiv = x.Fiv,
					FeLv = x.FeLv,

				});

			if (!string.IsNullOrWhiteSpace(postType))
			{
				data = data.Where(x => x.PostType.Contains(postType));
			}
			if (!string.IsNullOrWhiteSpace(areaAddress))
			{
				data = data.Where(x => x.AreaAddress.Contains(areaAddress));
			}
            if (!string.IsNullOrWhiteSpace(title))
            {
                data = data.Where(x => x.AdoptTitle.Contains(title));
            }
            if (!string.IsNullOrWhiteSpace(size))
			{
				data = data.Where(x => x.Size.Contains(size));
			}
			if (!string.IsNullOrWhiteSpace(type))
			{
				data = data.Where(x => x.Type.Contains(type));
			}
			if (!string.IsNullOrWhiteSpace(gender))
			{
				data = data.Where(x => x.Gender.Contains(gender));
			}
			if (!string.IsNullOrWhiteSpace(color))
			{
				data = data.Where(x => x.AnimalColor.Contains(color));
			}
			return data;
		}

		// GET: api/Adoptions/5
		[HttpGet("{id}")]
		//每筆領養的詳細Detail資訊
		public IEnumerable<AdoptionDetailDTO> AdoptionsDetail(int id)
		{
            var a = CustomerAccount;
            //拿到memberId(主鍵)
            var memberId = _context.Members.Where(x => x.Account == a).Select(x => x.MemberId).FirstOrDefault();


            var data = _context.Adoptions.Where(x => x.AdoptionId == id).Include(x => x.Member)
				.Include(x=>x.AnimalGender).Include(x => x.AnimalSize)
				.Include(x => x.AnimalType).Select(x => new AdoptionDetailDTO
            {
                AdoptionId = x.AdoptionId,
				Membermail=x.Member.Email,
				MemberName = x.Member.Name,
                MemberAccount=x.Member.Account,
                PostType = x.PostType,
				AdoptName= x.AdoptName,
				AdoptTitle= x.AdoptTitle,
				AnimalColor= x.AnimalColor,
				AdoptDescription= x.AdoptDescription,
				AreaAddress= x.AreaAddress,
				Latitude = x.Latitude,
				Longitude = x.Longitude,
				Image1 = x.Image1,
				Image2 = x.Image2,
				Image3 = x.Image3,
				Image4 = x.Image4,
				Image5 = x.Image5,
                    Gender = x.AnimalGender.GenderType,  
                    Size = x.AnimalSize.AnimalSize1,
                    Type = x.AnimalType.AnimalType1,
				ContainNumber = x.ContainNumber,
				OpenAdopt = x.OpenAdopt,
				Ligation = x.Ligation,
				Deworming = x.Deworming,
				Vaccination = x.Vaccination,
				Triple = x.Triple,
				AnimalChip = x.AnimalChip,
				Fiv = x.Fiv,
				FeLv = x.FeLv,
			});
			return data;
			
		}
		// GET: api/Adoptions/5
		[HttpGet("Edit")]
		//每筆領養的詳細Detail資訊
		public IEnumerable<AdoptionDTO> EditAdoptionsDetail(int id)
		{

			var data = _context.Adoptions.Where(x => x.AdoptionId == id).Include(x => x.Member)
				.Include(x => x.AnimalGender).Include(x => x.AnimalSize)
				.Include(x => x.AnimalType).Select(x => new AdoptionDTO
				{
					AdoptionId = x.AdoptionId,
					MemberId = x.MemberId,
					PostType = x.PostType,
					AdoptName = x.AdoptName,
					AdoptTitle = x.AdoptTitle,
					AnimalColor = x.AnimalColor,
					AdoptDescription = x.AdoptDescription,
					AreaAddress = x.AreaAddress,
					Latitude = x.Latitude,
					Longitude = x.Longitude,
					Image1 = x.Image1,
					Image2 = x.Image2,
					Image3 = x.Image3,
					Image4 = x.Image4,
					Image5 = x.Image5,
					AnimalGenderId = x.AnimalGenderId,
					AnimalSizeId = x.AnimalSizeId,
					AnimalTypeId = x.AnimalTypeId,
					ContainNumber = x.ContainNumber,
					OpenAdopt = x.OpenAdopt,
					Ligation = x.Ligation,
					Deworming = x.Deworming,
					Vaccination = x.Vaccination,
					Triple = x.Triple,
					AnimalChip = x.AnimalChip,
					Fiv = x.Fiv,
					FeLv = x.FeLv,
				});
			return data;

		}
		// GET: api/Adoptions/5
		[HttpGet("SelfPost")]
		//每筆領養的詳細Detail資訊
        public  IEnumerable<AdoptionDetailDTO> GetAdoptions(int id)
        {
			var a = CustomerAccount;
			//拿到memberId(主鍵)
			var memberId = _context.Members.Where(x => x.Account == a).Select(x => x.MemberId).FirstOrDefault();

			var data = _context.Adoptions.Where(x => x.MemberId == memberId).Include(x => x.Member)
                .Include(x => x.AnimalGender).Include(x => x.AnimalSize)
                .Include(x => x.AnimalType).Select(x => new AdoptionDetailDTO
                {
                    AdoptionId = x.AdoptionId,
					Membermail = x.Member.Email,
					MemberName = x.Member.Account,
                    MemberAccount = x.Member.Account,
                    PostType = x.PostType,
                    AdoptName = x.AdoptName,
                    AdoptTitle = x.AdoptTitle,
                    AnimalColor = x.AnimalColor,
                    AdoptDescription = x.AdoptDescription,
                    AreaAddress = x.AreaAddress,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Image1 = x.Image1,
                    Image2 = x.Image2,
                    Image3 = x.Image3,
                    Image4 = x.Image4,
                    Image5 = x.Image5,
                    Gender = x.AnimalGender.GenderType,
                    Size = x.AnimalSize.AnimalSize1,
                    Type = x.AnimalType.AnimalType1,
                    ContainNumber = x.ContainNumber,
                    OpenAdopt = x.OpenAdopt,
                    Ligation = x.Ligation,
                    Deworming = x.Deworming,
                    Vaccination = x.Vaccination,
                    Triple = x.Triple,
                    AnimalChip = x.AnimalChip,
                    Fiv = x.Fiv,
                    FeLv = x.FeLv,
                });
            return data;
           

            //var adoptions = await _context.Adoptions.FindAsync(id);

            //         if (adoptions == null)
            //         {
            //             return null;
            //         }
            //AdoptionDTO EmpDTO = new AdoptionDTO
            //{
            //	AdoptionId = adoptions.AdoptionId,
            //	MemberId= adoptions.MemberId,
            //	PostType = adoptions.PostType,
            //	AdoptName = adoptions.AdoptName,
            //	AdoptTitle = adoptions.AdoptTitle,
            //	AnimalColor = adoptions.AnimalColor,
            //	AreaAddress = adoptions.AreaAddress,
            //	Latitude = adoptions.Latitude,
            //	Longitude = adoptions.Longitude,
            //	AdoptDescription = adoptions.AdoptDescription,
            //	Image1 = adoptions.Image1,
            //	Image2 = adoptions.Image2,
            //	Image3 = adoptions.Image3,
            //	Image4 = adoptions.Image4,
            //	Image5 = adoptions.Image5,
            //	AnimalGenderId = adoptions.AnimalGenderId,
            //	AnimalSizeId = adoptions.AnimalSizeId,
            //	AnimalTypeId = adoptions.AnimalTypeId,
            //	ContainNumber = adoptions.ContainNumber,
            //	OpenAdopt = adoptions.OpenAdopt,
            //	Ligation = adoptions.Ligation,
            //	Deworming = adoptions.Deworming,
            //	Vaccination = adoptions.Vaccination,
            //	Triple = adoptions.Triple,
            //	AnimalChip = adoptions.AnimalChip,
            //	Fiv = adoptions.Fiv,
            //	FeLv = adoptions.FeLv,
            //};
            //return data;
        }

        // PUT: api/Adoptions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutAdoption(int id, AdoptionDTO adoptionDTO)
        {
            if (id != adoptionDTO.AdoptionId)
            {
                return "欲修改的領養資訊不正確";
            }
			Adoption adp = await _context.Adoptions.FindAsync(id);
			if (adp == null)
			{
				return "欲修改的領養資訊不存在";  
			}
			
			//adp.MemberId = adoptionDTO.MemberId;
			adp.PostType = adoptionDTO.PostType;
			adp.AdoptName = adoptionDTO.AdoptName;
			adp.AdoptTitle = adoptionDTO.AdoptTitle;
			adp.AnimalColor = adoptionDTO.AnimalColor;
			adp.AreaAddress = adoptionDTO.AreaAddress;
			//adp.Latitude = adoptionDTO.Latitude;
			//adp.Longitude = adoptionDTO.Longitude;
			adp.AdoptDescription = adoptionDTO.AdoptDescription;
			//adp.Image1 = adoptionDTO.Image1;
			//adp.Image2 = adoptionDTO.Image2;
			//adp.Image3= adoptionDTO.Image3;
			//adp.Image4 = adoptionDTO.Image4;
			//adp.Image5 = adoptionDTO.Image5;
			adp.AnimalGenderId = adoptionDTO.AnimalGenderId;
			adp.AnimalSizeId = adoptionDTO.AnimalSizeId;
			adp.AnimalTypeId = adoptionDTO.AnimalTypeId;
			//adp.OpenAdopt = adoptionDTO.OpenAdopt;
			//adp.Ligation = adoptionDTO.Ligation;
			//adp.Deworming = adoptionDTO.Deworming;
			//adp.Vaccination = adoptionDTO.Vaccination;
			//adp.Triple = adoptionDTO.Triple;
			//adp.AnimalChip = adoptionDTO.AnimalChip;
			//adp.Fiv = adoptionDTO.Fiv;
			//adp.FeLv = adoptionDTO.FeLv;
			//adp.Triple = adoptionDTO.Triple; 
			//adp.ContainNumber = adoptionDTO.ContainNumber;
			_context.Entry(adp).State = EntityState.Modified;

			try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!AdoptionExists(id))
                //{
                //    return "欲修改的領養資訊不存在";
                //}
                //else
                //{
                    throw;
                //}
            }
            return "修改成功";
        }
		[HttpPost("EditAdoption")]
		public async Task<string> EditProfile(int id, AdoptionDTO adoptionDTO)
		{
			var account = User.Identity.Name;
			Adoption adp = _context.Adoptions.FirstOrDefault(x => x.AdoptionId == id);
			adp.PostType = adoptionDTO.PostType;
			adp.AdoptName = adoptionDTO.AdoptName;
			adp.AdoptTitle = adoptionDTO.AdoptTitle;
			adp.AnimalColor = adoptionDTO.AnimalColor;
			adp.AreaAddress = adoptionDTO.AreaAddress;
			//adp.Latitude = adoptionDTO.Latitude;
			//adp.Longitude = adoptionDTO.Longitude;
			adp.AdoptDescription = adoptionDTO.AdoptDescription;
			//adp.Image1 = adoptionDTO.Image1;
			//adp.Image2 = adoptionDTO.Image2;
			//adp.Image3= adoptionDTO.Image3;
			//adp.Image4 = adoptionDTO.Image4;
			//adp.Image5 = adoptionDTO.Image5;
			adp.AnimalGenderId = adoptionDTO.AnimalGenderId;
			adp.AnimalSizeId = adoptionDTO.AnimalSizeId;
			adp.AnimalTypeId = adoptionDTO.AnimalTypeId;
			_context.SaveChanges();
			return "修改成功";
		}
		// POST: api/Adoptions
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
        public async Task<string> PostAdoption([FromForm] AdoptionPostDTO adoption)
        {

			var a = CustomerAccount;
			//拿到memberId(主鍵)
			var memberId = _context.Members.Where(x => x.Account == a).Select(x => x.MemberId).FirstOrDefault();

			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../PetSiteFront/wwwroot/AdoptionImages", adoption.Image1.FileName);
			
			var filename= Path.GetFileName(adoption.Image1.FileName);
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				adoption.Image1.CopyTo(stream);
			}
			Adoption adp = new Adoption
			{
				
				MemberId= memberId,
				PostType = adoption.PostType,
				AdoptName = adoption.AdoptName,
				AdoptTitle = adoption.AdoptTitle,
				AnimalColor = adoption.AnimalColor,
				AreaAddress = adoption.AreaAddress,
				//Latitude = adoption.Latitude,
				//Longitude = adoption.Longitude,
				AdoptDescription = adoption.AdoptDescription,
				Image1 = filename,
				//Image2 = adoption.Image2,
				//Image3 = adoption.Image3,
				//Image4 = adoption.Image4,
				//Image5 = adoption.Image5,
				AnimalGenderId = adoption.AnimalGenderId,
				AnimalSizeId = adoption.AnimalSizeId,
				AnimalTypeId = adoption.AnimalTypeId,
				//ContainNumber = adoption.ContainNumber,
				//OpenAdopt = adoption.OpenAdopt,
				//Ligation = adoption.Ligation,
				//Deworming = adoption.Deworming,
				//Vaccination = adoption.Vaccination,
				//Triple = adoption.Triple,
				//AnimalChip = adoption.AnimalChip,
				//Fiv = adoption.Fiv,
				//FeLv = adoption.FeLv,
			};

			_context.Adoptions.Add(adp);
            await _context.SaveChangesAsync();

            return "新增成功!";
        }

        // DELETE: api/Adoptions/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteAdoption(int id)
        {
            var adoption = await _context.Adoptions.FindAsync(id);
            if (adoption == null)
            {
				return "查無此領養貼文!";
			}

            _context.Adoptions.Remove(adoption);
            await _context.SaveChangesAsync();

			return "刪除成功!";
		}

        private bool AdoptionExists(int id)
        {
            return _context.Adoptions.Any(e => e.AdoptionId == id);
        }

		//[HttpPost("Filter")] //Uri=>api/Employees/Filter
		//public async Task<IEnumerable<AdoptionDTO>> FilterEmployee([FromBody] AdoptionDTO adp)
		//{

		//	return _context.Adoptions
		//		.Where(a => a.AnimalColor.Contains(adp.AdoptTitle) || a.AdoptTitle.Contains(adp.AdoptTitle) || 
		//		a.AnimalType.AnimalType1.Contains(adp.AdoptTitle) || a.AnimalSize.AnimalSize1.Contains(adp.AdoptTitle) ||
		//		a.AnimalGender.GenderType.Contains(adp.AdoptTitle))
		//		.Select(a => new AdoptionDTO
		//		{
		//			AdoptionId = a.AdoptionId,
		//			MemberId = a.MemberId,
		//			AdoptTitle=a.AdoptTitle,
		//			AnimalColor = a.AnimalColor,
		//			AnimalGenderId = a.AnimalGenderId,
		//			AnimalSizeId = a.AnimalSizeId,
		//			AnimalTypeId = a.AnimalTypeId,
		//			AreaAddress =a.AreaAddress,
		//		});
		//}
	}
}
