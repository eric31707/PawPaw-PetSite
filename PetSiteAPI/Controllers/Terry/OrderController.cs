using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PetSiteAPI.Models.Dtos.Terry;
using PetSiteAPI.Models.EFModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetSiteAPI.Controllers.Terry
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase

    {
        private OrderRepository orderRepository;
        public OrderController(PetSiteContext db)
        {

            this.orderRepository = new OrderRepository(db);

        }
        public string CustomerAccount=>User.Identity.Name;
        // GET: api/<OrderController>
        [HttpGet]
        public async Task<OrderEntity> GetOrder()
        {
            return orderRepository.Load(CustomerAccount);
        }

        // GET api/<OrderController>/5
        [HttpGet("AllOrder")]
        public async Task<List<OrderEntity>> GetAllOrder()
        {
            return orderRepository.LoadAll(CustomerAccount);
        } 

        // POST api/<OrderController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
