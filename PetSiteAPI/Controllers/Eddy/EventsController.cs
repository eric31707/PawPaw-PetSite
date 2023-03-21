using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
//using Grpc.Core;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PetSiteAPI.Models.Dtos;
using PetSiteAPI.Models.EFModels;
using PetSiteAPI.Models.Extensions;

namespace PetSiteAPI.Controllers.Eddy
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly PetSiteContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public string CustomerAccount => User.Identity.Name;
        public EventsController(PetSiteContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }


        #region get
        //點擊某個Card後，給後端活動id，回傳那個活動所有資料。
        [HttpGet("{id}")]
        public async Task<IEnumerable<EventDetailDTO>> GetEvent(int id)
        {
            int memberId;//memberId(主鍵)

            //拿到帳號
            var a = CustomerAccount;
            //拿到memberId(主鍵)
            memberId = _context.Members.Where(x => x.Account == a).Select(x => x.MemberId).FirstOrDefault();

            //下面的Join是要查這個人有沒有報名過
            var data = _context.Events.Where(x => x.EventId == id).Include(x => x.Member).Select(x => new EventDetailDTO
            {
                EventId = x.EventId,
                Title = x.Title,
                Image = x.Image,
                MemberName = x.Member.Name,
                EventsCategoryName = x.EventsCategoryName,
                ApplyTimeStart = x.ApplyTimeStart.ToDateTimeStringAll(),
                ApplyTimeEnd = x.ApplyTimeEnd.ToDateTimeStringAll(),
                ProgressTimeStart = x.ProgressTimeStart.ToDateTimeStringAll(),
                ProgressTimeEnd = x.ProgressTimeEnd.ToDateTimeStringAll(),
                Location = x.Location,
                EventApplicantsQTY = x.EventApplicantsQty + "/" + x.ApplicantLimitedQty,
                EventInfo = x.EventInfo,
                Join = _context.ApplyEvents.Where(x => x.MemberId == memberId && x.EventId == id).Any()
            });

            return data;
        }

        //Card，抓所有活動資料，用Card呈現:活動ID(隱藏)、封面圖片、舉辦人、主題、舉辦時間、舉辦地點
        [HttpGet("Card")]
        public async Task<List<CardDTO>> GetEventToCard(string? EventsCategoryName, string? EventName, int skipNumber)
        {
            var data = _context.Events.AsQueryable();
            skipNumber = skipNumber * 9;
            if (EventsCategoryName != null)
            {
                data = data.Where(x => x.EventsCategoryName == EventsCategoryName);
            }

            if (EventName == null)
            {
                EventName = "";
            }

            var dataCardDTO = data.Where(x => x.Title.Contains(EventName)).Select(x => new CardDTO
            {
                EventId = x.EventId,
                MemberId = x.MemberId,
                Image = x.Image,
                Title = x.Title,
                ApplyTimeEnd = x.ApplyTimeEnd.ToDateTimeString(),
                ProgressTimeStart = x.ProgressTimeStart.ToDateTimeString(),
                Location = x.Location,
            }).Skip(skipNumber).Take(9).ToList();
            return dataCardDTO;
        }

        //查詢自己已發佈的活動，給後端一個使用者ID，查詢所有自己已發佈的活動
        [HttpGet("Release")]
        public async Task<IEnumerable<CreateEventDto>> GetEventToRelease()
        {
            string EventUrl = "https://localhost:7180/Event/Details?id=";

            int memberId;//memberId(主鍵)

            //拿到帳號
            var a = CustomerAccount;
            //拿到memberId(主鍵)
            memberId = _context.Members.Where(x => x.Account == a).Select(x => x.MemberId).FirstOrDefault();

            var data = _context.Events.Where(x => x.MemberId == memberId).OrderByDescending(x => x.EventId).Select(x => new CreateEventDto
            {
                EventId = x.EventId,
                Title = x.Title,
                EventApplicantsQTY = x.EventApplicantsQty + "/" + x.ApplicantLimitedQty,
                EventHerf = EventUrl + x.EventId,
            });
            return data;
        }

        //查詢誰報名了我的活動，給後端一個活動ID，查詢誰報名了這個活動，回傳memberID、狀態
        [HttpGet("WhoJoin")]
        public async Task<IEnumerable<WhoJoin>> GetEventToWhoJoin(int id)
        {
            var data = _context.ApplyEvents.Where(x => x.EventId == id).Include(x => x.Member).Select(x => new WhoJoin
            {
                MemberId = x.MemberId,
                Name = x.Member.Name,
                TotalPeople = x.TotalPeople,
                TotalPet = x.TotalPet,
                Status = x.Status,
            }).OrderBy(x => x.Status);
            return data;
        }

        //查詢自己已參加的活動，給後端一個使用者ID，查詢所有自己參加的活動，回傳title、status、EventsCategoryName
        [HttpGet("Join")]
        public async Task<IEnumerable<Join>> GetEventToJoin()
        {
            string EventUrl = "https://localhost:7180/Event/Details?id=";

            int memberId;//memberId(主鍵)

            //拿到帳號
            var a = CustomerAccount;
            //拿到memberId(主鍵)
            memberId = _context.Members.Where(x => x.Account == a).Select(x => x.MemberId).FirstOrDefault();
            var data = _context.ApplyEvents.Where(x => x.MemberId == memberId)
                .Include(x => x.Event)
                .Select(x => new Join
                {
                    EventId = x.EventId,
                    Title = x.Event.Title,
                    Status = x.Status,
                    EventsCategoryName = x.Event.EventsCategoryName,
                    EventHerf = EventUrl + x.EventId,
                });

            return data;
        }

        //查詢要編輯的活動，給後端一個活動ID，傳回活動的所有資料，給編輯用的
        [HttpGet("OneEvent")]
        public async Task<IEnumerable<OneEventForEdit>> GetOneEventForEdit(int eventId)
        {
            string EventUrl = "https://localhost:7180/Event/Details?id=";

            int memberId;//memberId(主鍵)

            //拿到帳號
            var a = CustomerAccount;
            //拿到memberId(主鍵)
            memberId = _context.Members.Where(x => x.Account == a).Select(x => x.MemberId).FirstOrDefault();
            var data = _context.Events.Where(x => x.EventId == eventId).Select(x => new OneEventForEdit
            {
                Title = x.Title,
                Image = "/images/" + x.Image,
                EventsCategoryName = x.EventsCategoryName,
                ApplyTimeStart = x.ApplyTimeStart,
                ApplyTimeEnd = x.ApplyTimeEnd,
                ProgressTimeStart = x.ProgressTimeStart,
                ProgressTimeEnd = x.ProgressTimeEnd,
                Location = x.Location,
                ApplicantLimitedQTY = x.ApplicantLimitedQty,
                EventInfo = x.EventInfo,
            });

            return data;
        }
        #endregion



        #region put
        //修改活動 給後端活動全部資料
        [HttpPut("PutEvent")]
        public async Task<JsonResult> PutEvent([FromForm] EventEdit EventEditDTO)
        {
            //拿到帳號
            var a = CustomerAccount;
            //拿到memberId(主鍵)
            var memberId = _context.Members.Where(x => x.Account == a).Select(x => x.MemberId).FirstOrDefault();

            var data = _context.Events.Where(x => x.EventId == EventEditDTO.EventId).FirstOrDefault();
            if (data == null)
            {
                return new JsonResult(new { Name = "找不到該筆活動" });

            }
            if (EventEditDTO.Image != null)
            {
                string photoName = Guid.NewGuid().ToString();//Guid取得圖片名稱
                                                             //string path = _hostingEnvironment.ContentRootPath +"Images";
                string extension = Path.GetExtension(EventEditDTO.Image.FileName);//取得副檔名
                string imageName = photoName + extension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../PetSiteFront/wwwroot/images", imageName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    EventEditDTO.Image.CopyTo(stream);
                }
                data.Image = imageName;
            }



            data.Title = EventEditDTO.Title;
            data.EventsCategoryName = EventEditDTO.EventsCategoryName;
            data.ApplyTimeStart = DateTime.Parse(EventEditDTO.ApplyTimeStart);
            data.ApplyTimeEnd = DateTime.Parse(EventEditDTO.ApplyTimeEnd);
            data.ProgressTimeStart = DateTime.Parse(EventEditDTO.ProgressTimeStart);
            data.ProgressTimeEnd = DateTime.Parse(EventEditDTO.ProgressTimeEnd);
            data.ApplicantLimitedQty = EventEditDTO.ApplicantLimitedQTY;
            data.Location = EventEditDTO.Location;
            data.EventInfo = EventEditDTO.EventInfo;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return new JsonResult(new { Name = "修改失敗，請重新再試" });

            }
            return new JsonResult(new { Name = "修改成功" });
        }

        //通過別人報名的審核，給後端使用者ID、活動ID、審核通過與否(bool)
        [HttpPut]
        public async Task<string> PutEventEditStatus(EditEventStatusDto EditEventStatus)
        {
            var data = _context.ApplyEvents.Where(x => x.EventId == EditEventStatus.EventId && x.MemberId == EditEventStatus.MemberId).FirstOrDefault();

            //如果按了通過審核按鈕
            if (EditEventStatus.Status == true)
            {
                //修該報名狀態
                data.Status = "審核通過";
                _context.SaveChanges();
                //修改目前報名人數
                var eve = _context.Events.Where(x => x.EventId == EditEventStatus.EventId).FirstOrDefault();
                var totalPeople = data.TotalPeople + eve.EventApplicantsQty;
                eve.EventApplicantsQty = totalPeople;
                _context.SaveChanges();
                return "審核通過";
            }
            data.Status = "審核未通過";
            _context.SaveChanges();
            return "審核未通過";
        }
        #endregion



        #region post
        //發佈活動
        [HttpPost]
        public async Task<JsonResult> PostEvent([FromForm] EventCreate eventCreateDTO)
        {
            //拿到帳號
            var a = CustomerAccount;
            //拿到memberId(主鍵)
            var memberId = _context.Members.Where(x => x.Account == a).Select(x => x.MemberId).FirstOrDefault();

            var data = eventCreateDTO;
            string photoName = Guid.NewGuid().ToString();//Guid取得圖片名稱
                                                         //string path = _hostingEnvironment.ContentRootPath +"Images";
            string extension = Path.GetExtension(eventCreateDTO.Image.FileName);//取得副檔名
            string imageName = photoName + extension;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../PetSiteFront/wwwroot/images", imageName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                eventCreateDTO.Image.CopyTo(stream);
            }
            Event eve = new Event
            {
                MemberId = memberId,
                Title = eventCreateDTO.Title,
                Image = imageName,
                EventsCategoryName = eventCreateDTO.EventsCategoryName,
                ApplyTimeStart = DateTime.Parse(eventCreateDTO.ApplyTimeStart),
                ApplyTimeEnd = DateTime.Parse(eventCreateDTO.ApplyTimeEnd),
                ProgressTimeStart = DateTime.Parse(eventCreateDTO.ProgressTimeStart),
                ProgressTimeEnd = DateTime.Parse(eventCreateDTO.ProgressTimeEnd),
                ApplicantLimitedQty = eventCreateDTO.ApplicantLimitedQTY,
                EventApplicantsQty = 0,
                Location = eventCreateDTO.Location,
                EventInfo = eventCreateDTO.EventInfo
            };
            _context.Events.Add(eve);
            await _context.SaveChangesAsync();
            var datare = new { Name = "新增成功" };
            return new JsonResult(datare);
        }

        //點擊報名，給後端活動id、報名人數、寵物數量，就會建立一筆待審核的活動報名item。
        [HttpPost("event")]
        public async Task<string> PostEventJoin(PostEventJoinDTO postEventJoinDTO)
        {
            int memberId;//memberId(主鍵)
            var a = CustomerAccount;//拿到帳號
            try
            {
                //拿到memberId(主鍵)
                memberId = _context.Members.Where(x => x.Account == a).Select(x => x.MemberId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return "拿不到帳號阿，愛拿不拿，滾";
            }
            try
            {
                //先查詢此活動有沒有在報名時間內，如果沒有回傳:"此活動報名時間已截止"
                var timeOut = _context.Events.Where(x => x.EventId == postEventJoinDTO.EventId
                                                    && x.ApplyTimeEnd >= DateTime.Now
                                                    && x.ApplyTimeStart <= DateTime.Now).Any();
                if (timeOut == false)
                {
                    return "此活動報名時間已截止";
                }
                //再查詢自己報名過了沒，如果報名過了回傳："此活動您已經報名，請去活動中心查看"
                bool joinOne = _context.ApplyEvents.Where(x => x.EventId == postEventJoinDTO.EventId && x.MemberId == memberId).Any();
                if (joinOne == true)
                {
                    return "此活動您已經報名，請去活動中心查看";
                }

                //再查詢此活動時間，有沒有其他活動報名，如果有，回傳："此活動時間您已報名其他活動"

                //如果以上都通過，那就建立一筆待審核的活動報名item。
                var data = new ApplyEvent
                {
                    MemberId = memberId,
                    EventId = postEventJoinDTO.EventId,
                    CreateDate = DateTime.Now,
                    Status = "未審核",
                    TotalPeople = postEventJoinDTO.TotalPeople,
                    TotalPet = postEventJoinDTO.TotalPet,
                };
                _context.ApplyEvents.Add(data);
                await _context.SaveChangesAsync();

                return "報名成功，等待審核";
            }
            catch (Exception ex)
            {
                return "不可預期的錯誤，愛報不報，滾";
            }

        }
        #endregion



        #region delete
        //刪除發起的活動
        [HttpDelete("DeleteEvent")]
        public async Task<string> DeleteEvent(int eventId)
        {


            int memberId;//memberId(主鍵)
                         //拿到帳號
            var a = CustomerAccount;
            try
            {
                //拿到memberId(主鍵)
                memberId = _context.Members.Where(x => x.Account == a).Select(x => x.MemberId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return "拿不到帳號阿，愛拿不拿，滾";
            }

            var data = await _context.Events.Where(x => x.MemberId == memberId && x.EventId == eventId).FirstOrDefaultAsync();

            if (data == null)
            {
                return "查無此資料!";
            }

            try
            {
                _context.Events.Remove(data);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return "刪除失敗";
            }

            return "刪除成功";
        }

        //退出已報名的活動，給後端一個自己報名的活動ID和使用者ID，退出報名，修改目前報名人數
        [HttpDelete("DeleteJoin")]
        public async Task<string> DeleteEventJoin(int eventId)
        {

            int memberId;//memberId(主鍵)

            //拿到帳號
            var a = CustomerAccount;
            //拿到memberId(主鍵)
            memberId = _context.Members.Where(x => x.Account == a).Select(x => x.MemberId).FirstOrDefault();

            var data = await _context.ApplyEvents.Where(x => x.EventId == eventId && x.MemberId == memberId).FirstOrDefaultAsync();
            if (data == null)
            {
                return "查無此資料!";
            }
            _context.ApplyEvents.Remove(data);
            await _context.SaveChangesAsync();

            //修改目前報名人數
            if (data.Status == "審核通過")
            {
                var eve = await _context.Events.Where(x => x.EventId == eventId).FirstOrDefaultAsync();
                var ApplicantLimitedQTY = eve.EventApplicantsQty - data.TotalPeople;
                eve.EventApplicantsQty = ApplicantLimitedQTY;
                _context.SaveChanges();
            }

            return "退出成功";
        }
        #endregion



        #region 測試開發用

        //ckeditor5給一張照片，回傳一個圖片路徑
        [HttpPost("ckeditor")]
        public async Task<string> CkediotrImage(IFormFile? file)
        {
            var data = file;
            return "hello";
        }
        #endregion

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}
