using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Text;
using PetSiteAPI.Models.EFModels;
using System.Collections.Generic;
using PetSiteAPI.Models.VM;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;


namespace PetSiteFront.Controllers.Eric
{
    [Route("Room/[Action]")]
    public class RoomController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Room()
        {
            return View(CalledAPI<IEnumerable<RoomVM>>("https://localhost:7150/api/Rooms"));
        }
        public IActionResult RoomDetail(int id)
        {
                       
            return View(CalledAPI<RoomVM>("https://localhost:7150/api/Rooms/"+id));
        }
		public IActionResult RoomOrder()
		{

			return View();
		}
		static public T CalledAPI<T>(string uri)
        {           
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri); //request請求
                req.Timeout = 10000000; //request逾時時間
                req.Method = "GET"; //request方式           
                HttpWebResponse respone = (HttpWebResponse)req.GetResponse(); //接收respone
                StreamReader streamReader = new StreamReader(respone.GetResponseStream(), Encoding.UTF8); //讀取respone資料
                string result = streamReader.ReadToEnd(); //讀取到最後一行
                respone.Close();
                streamReader.Close();
                var jsondata = JsonConvert.DeserializeObject<T>(result); //將資料轉為json物件
                return jsondata; //回傳json陣列
            }
            catch (WebException ex)
            {
                string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }
            return default(T);
        }

        public IActionResult Payment(int RoomOrderId)
        {
            return View(CalledAPI<PaymentVM>("https://localhost:7150/api/Rooms/GetRoomOrder?RoomOrderId=" + RoomOrderId));
        }
        public IActionResult SendEmail(int RoomOrderId,int Total )
        {
			
			var forECpay =  CalledAPI<PaymentVM>("https://localhost:7150/api/Rooms/GetRoomOrder?RoomOrderId=" + RoomOrderId+ "&updateStatus=true"+"");
			
			MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(System.Text.Encoding.UTF8, "Eric", "我的寵物網頁"));
            message.To.Add(new MailboxAddress(System.Text.Encoding.UTF8, forECpay.MemberEmail, forECpay.MemberEmail));
            message.Subject = "訂房確認信件";
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<div>****此信件為系統發出信件，請勿直接回覆，感謝您的配合。謝謝****</div>";
            bodyBuilder.HtmlBody = $"<div>親愛的  {forECpay.MemberName}  您好：</div><hr>";
            bodyBuilder.HtmlBody += $"<div>您於{DateTime.Now.ToString("yyyy-MM-dd tt HH:mm:ss")}於本網站訂房</div><br>";
            bodyBuilder.HtmlBody += $"<div>如您未訂房請忽略此郵件</div><br>";
            bodyBuilder.HtmlBody += $"感謝你的於本網站訂房<br>";
            bodyBuilder.HtmlBody += $"----------------------------------------------------------------------------------------------------------------<br>";
            bodyBuilder.HtmlBody += $"<br> " +
                $"<p>訂購人:    {forECpay.MemberName} </p>\r\n" +
                $"<p>旅館地址:  {forECpay.RoomAddress}</p>\r\n" +              
                $"<p>入住時間:  {forECpay.StartDate.ToString("yyyy-MM-dd")}</p>\r\n" +
                $"<p>退房時間:  {forECpay.EndDate.ToString("yyyy-MM-dd")}</p>\r\n"+
                $"<p>入住天數:  {forECpay.Days} 天</p>\r\n" +
                $"<p>總金額:   {Total} 元</p>\r\n";
            bodyBuilder.HtmlBody += $"----------------------------------------------------------------------------------------------------------------<br>";
            bodyBuilder.HtmlBody += $"<br>";
            bodyBuilder.HtmlBody += $"<br>";
            bodyBuilder.HtmlBody += $"<br>";
            bodyBuilder.HtmlBody += $"<br>";
            bodyBuilder.HtmlBody += $"<div>如有任何問題請隨時與我們聯繫</div><br>";
            bodyBuilder.HtmlBody += $"<div>Tel : 0800-092-000</div><br>";
            bodyBuilder.HtmlBody += $"<div>地址 : 兵庫県神戸市兵庫区湊町26番25号</div><br>";
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                var hostUrl = "smtp.gmail.com";
                var port = 587;
                //var useSsl = true;
                client.Connect(hostUrl, port, false);
                client.Authenticate("eric31707@gmail.com", "jifivdrcgyaazldn");
                client.Send(message);
                client.Disconnect(true);
            }
            message.Dispose();
            return RedirectToAction("RoomOrder", "Room");
        }
    }
}
