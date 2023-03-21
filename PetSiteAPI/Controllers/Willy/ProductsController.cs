using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using PetSiteAPI.Models.Dtos;
using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Controllers.Willy
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly PetSiteContext _context;

        public ProductsController(PetSiteContext context)
        {
            _context = context;
        }
        //顯示所有產品
        
        [HttpGet]
        public IEnumerable<AllProductNameDTO> GetAllProductsName(string? name,string? Category,string? Species,int? mini,int? max)
        {
            var data = (from a in _context.ProductNames
                        join b in _context.Products on a.ProductNameId equals b.ProductNameId
                        join c in _context.ProductImages on a.ProductNameId equals c.ProductNameId
                        select new AllProductNameDTO
                        {
                            ProductNameId = a.ProductNameId,
                            Name = b.Name,
                            Price = b.Price,
                            Images = c.Images,
                            Description=b.Description,
                            Category=b.Category.Name,
                            Species=b.Species.Name,
                            
                        }).Distinct();

			if (!string.IsNullOrWhiteSpace(name))
			{
				data = data.Where(x => x.Name.Contains(name));
			}
			if (!string.IsNullOrWhiteSpace(Category))
			{
				data = data.Where(x => x.Category.Contains(Category));
			}
			if (!string.IsNullOrWhiteSpace(Species))
			{
				data = data.Where(x => x.Species.Contains(Species));
			}
			if (mini < 0)
			{
				mini = 0;
			}
			if (mini != null && max != null && mini >= 0 && max >= 0)
			{
				data = data.Where(x => x.Price <= max && x.Price >= mini);
			}
			return data;
		}

		[HttpGet("{id}")]
        public AllProductNameDTO GetProduct(int id)
        {
            var data = (from a in _context.ProductNames
                        join b in _context.Products on a.ProductNameId equals b.ProductNameId
                        join c in _context.ProductImages on a.ProductNameId equals c.ProductNameId
                        where a.ProductNameId == id
                        select new AllProductNameDTO
                        {
                            ProductNameId = a.ProductNameId,
                            Name = b.Name,
                            Price = b.Price,
                            Description = b.Description,
                            Images = c.Images,
                            Products = (from b in _context.Products
                                        where a.ProductNameId == b.ProductNameId
                                        select new ProductDTO
                                        {
                                            ProductId = b.ProductId,
                                            Name = b.Name,
                                            Price = b.Price,
                                            Description = b.Description,
                                            Category = b.Category.Name,
                                            Color = b.Color.Name,
                                            Size = b.Size.Name,
                                            Flavor = b.Flavor.Name,
                                            Quantity = b.Quantity,
                                            Species = b.Species.Name,
                                        }).ToList()
                        }).FirstOrDefault();
            return data;
        }
		
	}
}
