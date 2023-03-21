using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Newtonsoft.Json;
using PetSiteAPI.Models.EFModels;
using PetSiteAPI.Models.VM;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.IdentityModel.Protocols;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.Protocol;

namespace PetSiteAPI.Controllers.Eric
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly PetSiteContext _context;
        public string CustomerAccount => User.Identity.Name;
        public RoomsController(PetSiteContext context)
        {
            _context = context;
        }

        // GET: api/Rooms
        [HttpGet]
        public async Task<ActionResult<string>> GetRooms()
        {
            string str = "";
            List<RoomVM> roomVMList = new List<RoomVM>();
            try
            {
                var roomList = await _context.Rooms.ToListAsync();
                var roomImageList = await _context.RoomImages.ToListAsync();
                foreach (var roomDetail in roomList)
                {
                    var photos = roomImageList.Where(x => x.RoomId == roomDetail.RoomId).Select(x => x.Image).ToList();
                    roomVMList.Add(new RoomVM
                    {
                        RoomId = roomDetail.RoomId,
                        Image = photos,
                        Type = roomDetail.Type,
                        Address = roomDetail.Address,
                        Description = roomDetail.Description,
                        Name = roomDetail.Name,
                        Price = roomDetail.Price
                    });
                }
                str = JsonConvert.SerializeObject(roomVMList);
            }
            catch (Exception ex)
            {

            }
            return str;
        }

        // GET: api/Rooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetRoom(int id)
        {
            string str = "";
            RoomVM roomVM = new RoomVM();
            try
            {
                var room = await _context.Rooms.FirstOrDefaultAsync(x => x.RoomId == id);
                var roomImageList = await _context.RoomImages.Where(x => x.RoomId == room.RoomId)
                                                             .Select(x => x.Image)
                                                             .ToListAsync();
                var roomOrderList = await _context.RoomOrderItems
                                                .Where(x => x.RoomId == room.RoomId)
                                                .Select(x => new RoomOrderDate() { startdate = x.StartDate.ToString("yyyy-MM-dd"), enddate = x.EndDate.ToString("yyyy-MM-dd"), days = x.Days.ToString() })
                                                .ToListAsync();
                roomVM = new RoomVM
                {
                    RoomId = room.RoomId,
                    Image = roomImageList,
                    Type = room.Type,
                    Address = room.Address,
                    Description = room.Description,
                    Name = room.Name,
                    Price = room.Price,
                    DatesList = roomOrderList
                };

                str = JsonConvert.SerializeObject(roomVM);
            }
            catch (Exception ex)
            {

            }
            return str;
        }

        // PUT: api/Rooms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(int id, Room room)
        {
            if (id != room.RoomId)
            {
                return BadRequest();
            }

            _context.Entry(room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/Rooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<string[]> PostRoom(RoomOrderVM roomOrderVm)
        {
            var Account = CustomerAccount;
            if (Account == null)
            {
				string[] obj1 = { "請登入", "成為會員後再進行操作" };
				return obj1;
			}
            int MemberId = _context.Members.FirstOrDefault(x => x.Account == Account).MemberId;
            string orderdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                var roomOrder = new RoomOrder()
                {
                    MemberId = MemberId,
                    CouponId = roomOrderVm.CouponId,
                    TotalPrice = roomOrderVm.Price * roomOrderVm.Days,
                    OrderDate = DateTime.Parse(orderdate),
                    Status = "未付款",

                };
                _context.RoomOrders.Add(roomOrder);
                _context.SaveChanges();

                var orderId = (from o in _context.RoomOrders
                               where o.MemberId == MemberId
                               orderby o.OrderDate descending
                               select o.RoomOrderId).ToList();

                var roomOrderItem = new RoomOrderItem()
                {
                    RoomOrderId = Convert.ToInt32(orderId[0]),
                    RoomId = roomOrderVm.RoomId,
                    StartDate = DateTime.Parse(roomOrderVm.Startdate),
                    EndDate = DateTime.Parse(roomOrderVm.Enddate),
                    Days = roomOrderVm.Days,
                    RoomPrice = roomOrderVm.Price,

                };
                _context.RoomOrderItems.Add(roomOrderItem);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {

            }
            string[] obj = { orderdate, MemberId.ToString() };
            return obj;

        }
        [Route("GetRoomOrder")]
        [HttpGet]
        public async Task<string> GetRoomOrder(int RoomOrderId, bool updateStatus = false)
        {

            string str = "";
            try
            {
                var roomOrder = _context.RoomOrders.FirstOrDefault(x => x.RoomOrderId == RoomOrderId);
                var roomOrderItem = _context.RoomOrderItems.FirstOrDefault(x => x.RoomOrderId == roomOrder.RoomOrderId);               
                var member = _context.Members.FirstOrDefault(x => x.MemberId == roomOrder.MemberId);
                var room = _context.Rooms.FirstOrDefault(x => x.RoomId == roomOrderItem.RoomId);
                var payment = new PaymentVM()
                {
                    RoomOrderId = roomOrder.RoomOrderId,                    
                    MemberEmail = member.Email,
                    MemberName = member.Name,
                    Mobile = member.Mobile,
                    RoomName = room.Name,
                    RoomAddress = room.Address,
                    RoomFee = room.Price,
                    StartDate = roomOrderItem.StartDate,
                    EndDate = roomOrderItem.EndDate,
                    Days = roomOrderItem.Days,
                    Total = roomOrderItem.RoomPrice * roomOrderItem.Days,
                };

                //var account = _context.Members.FirstOrDefault(x => x.MemberId == roomOrderVm.MemberId);
                //string path = "https://localhost:7180/";
                //string Action = "Room/Payment";
                //string webPath = "<a href='" + path + Action + "'>點此連結</a>";
                //SendEmail(account.Email, webPath);

                if (updateStatus)
                {
                    var coupon = _context.Coupons.FirstOrDefault(x => x.CouponId == roomOrder.CouponId).DiscountAmount;
                    roomOrder.Status = "已付款";
                    roomOrder.TotalPrice = roomOrder.TotalPrice- coupon;
                    _context.RoomOrders.Update(roomOrder);
                    _context.SaveChanges();
                }



                str = JsonConvert.SerializeObject(payment);

            }
            catch (Exception ex)
            {

            }
            return str;

        }

        // DELETE: api/Rooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.RoomId == id);
        }

        [Route("GetOrderId")]
        [HttpPost]
        public async Task<int> GetOrderId(MemOrdPar obj)
        {
            int roomOrderId = 0;
            try
            {
                roomOrderId = _context.RoomOrders.FirstOrDefault(x => x.MemberId == obj.MemberId && x.OrderDate == DateTime.Parse(obj.orderdate)).RoomOrderId;
            }
            catch (Exception ex)
            {

            }
            return roomOrderId;

        }


        [Route("GetMemberOrder")]
        [HttpGet]
        public async Task<IActionResult> GetMemberOrder()
        {
            var Account = CustomerAccount;

            if(Account == null)
            {
                return NoContent();
            }

            var Member = _context.Members.FirstOrDefault(x => x.Account == Account);
        
            var query = from ro in _context.RoomOrders
                        join roi in _context.RoomOrderItems
                        on ro.RoomOrderId equals roi.RoomOrderId
                        select new PaymentVM
                        {
                            MemberEmail= Member.Email,
                            MemberName= Member.Name,
                            Mobile= Member.Mobile,
                            RoomFee=roi.RoomPrice,
                            RoomOrderId = ro.RoomOrderId,
                            RoomName = roi.Room.Name,
                            RoomAddress = roi.Room.Address,
                            StartDate = roi.StartDate,
                            EndDate = roi.EndDate,
                            Days = roi.Days,
                            Total = ro.TotalPrice,
                            Status = ro.Status,
                        };

            return Ok(query);
        }

       
    }
}
