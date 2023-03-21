using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using PetSiteAPI.Infra.ExtensionMethods.MembersExtension;
using PetSiteAPI.Models.Dtos;
using PetSiteAPI.Models.EFModels;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Collections.Immutable;
using Microsoft.Build.Construction;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json.Linq;
using Google.Apis.Auth;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.Metrics;
using Google.Apis.Util;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System.Runtime.Serialization.Json;
using MimeKit.Utils;
using Org.BouncyCastle.Crypto.Macs;

namespace PetSiteAPI.Controllers.Evan
{
	[EnableCors("AllowAny")]
	[Route("api/[controller]")]
	[ApiController]
	public class MembersController : ControllerBase
	{
		private readonly PetSiteContext _context;
		private readonly IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public MembersController(PetSiteContext context,IConfiguration configuration,IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_configuration= configuration;
			_httpContextAccessor= httpContextAccessor;
		}


		[HttpPost("Register")]
		public async Task<string> Register(RegisterDTO register)
		{
			if (!ModelState.IsValid)
			{
				return "輸入錯誤";
			}
			if (_context.Members.Any(x => x.Account == register.Account))
			{
				return "帳號已存在";
			}
			DateTime? Birth;
			if (!DateTime.TryParse(register.BirthDate, out DateTime result))
			{
				Birth = null;
			}
			else { 
				Birth =Convert.ToDateTime(register.BirthDate);
			}
			Member member = new Member
			{
				Account = register.Account,
				Name = register.Name,
				EncryptedPassword = register.EncryptedPassword,
				Mobile = register.Mobile,
				Email = register.Email,
				BirthDate = Birth,
				Address = register.Address,
				IsConfirmed = false,
				ConfirmCode = Guid.NewGuid().ToString("N"),

			};
			_context.Members.Add(member);
			_context.SaveChanges();
			SendEmail(member);
			return "註冊成功";
		}

		[HttpGet("IsAccountExist")]
		public async Task<bool> IsAccountExist(string account)
		{
			bool result = await _context.Members.AnyAsync(m => m.Account == account);
			return result;
		}
		

		/// 登入		
		[HttpPost("LogIn")]
		public string LogIn(LogInDTO value)
		{
			var member = _context.Members.FirstOrDefault(x => x.Account == value.Account);

			if (member == null)
			{
				return ("帳密有誤");
			}

			if (member.IsConfirmed == false)
			{
				return ("會員資格尚未確認");
			}

			string encryptedPwd = HashUtility.ToSHA256(value.Password, RegisterDTO.SALT);

			if (String.CompareOrdinal(member.EncryptedPassword, encryptedPwd) != 0)
			{
				return "帳號或密碼錯誤";
			}
			else
			{

				//var claims = new List<Claim>
				//{
				//	new Claim(JwtRegisteredClaimNames.NameId,member.MemberId.ToString()),
				//	new Claim(JwtRegisteredClaimNames.Email,member.Email),
				//	new Claim("Account",member.Account)
				//	// new Claim(ClaimTypes.Role, "Administrator")
				//};
				
    //            //取出appsettings.json裡的KEY處理
    //            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:KEY"]));

    //            //設定jwt相關資訊
    //            var jwt = new JwtSecurityToken
    //            (
    //                issuer: _configuration["JWT:Issuer"],
    //                audience: _configuration["JWT:Audience"],
    //                claims: claims,
    //                expires: DateTime.Now.AddDays(7),
    //                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
    //            );

    //            //產生JWT Token
    //            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

				////回傳JWT Token給認證通過的使用者

				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name,member.Account),
					new Claim(ClaimTypes.NameIdentifier,member.MemberId.ToString()),
					new Claim("id",member.MemberId.ToString()),
					new Claim("email",member.Email),
				};

				var cookieOptions = new CookieOptions
				{
					HttpOnly = true,
					SameSite = SameSiteMode.None,
					Secure = true,	Expires= DateTime.UtcNow.AddDays(7),
					Path= "Members/LogIn",
				};
				var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
				return "登入成功";
			};

		}

		[HttpPost("LogOut")]
		public void LogOut()
		{
			HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		}

		/// 寄發驗證信
		[HttpPost("SendEmail")]
		public string SendEmail(Member member) {

			var message = new MimeMessage();

			// 添加寄件者
			message.From.Add(new MailboxAddress("PawPaw!", "pawpawpetSite@gmail.com"));

			// 添加收件者
			message.To.Add(new MailboxAddress("New Member", $"{member.Email}"));

			// 設定郵件標題
			message.Subject = "歡迎使用PawPaw";

			// 使用 BodyBuilder 建立郵件內容
			var bodyBuilder = new BodyBuilder();

			string result = Request.Scheme + "://" + Request.Host + $"/api/Members/EmailConfirm/?account={member.Account}&confirmCode={member.ConfirmCode}";

			// 設定 HTML 內容
			bodyBuilder.HtmlBody =	$"<img src=\"cid:logo\" style='display: block; margin: 0 auto; width:30%;' alt='網站LOGO' >" +
									"<h1 style='text-align: center; color:darkgreen; font-size:60px;'>PawPaw!</h1>" +
									"<br>"+
									"<h3 style='text-align: center;font-size:30px;'>歡迎使用PawPaw!!</h3>" +
									"<h3 style='text-align: center;font-size:30px;'>請點擊下方連結以驗證信箱</h3>" +
									"<p style='text-align: center;'>"+
									$"<a style= font-size:20px;' href=\"{result}\">-----點此連結驗證------</a>"+
									"</p>" +
									"<br>" 
									;
			var a = Directory.GetCurrentDirectory();
			var file = Path.Combine(Directory.GetCurrentDirectory(), "../PetSiteFront/wwwroot/images/paw paw.png");
			var image = bodyBuilder.LinkedResources.Add(file);
			image.ContentId = MimeUtils.GenerateMessageId();
			bodyBuilder.HtmlBody = bodyBuilder.HtmlBody.Replace("cid:logo", $"cid:{image.ContentId}");
			// 設定郵件內容
			message.Body = bodyBuilder.ToMessageBody();

			using (var client = new SmtpClient())
			{
				client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
				client.Authenticate("pawpawpetSite@gmail.com", "fbqolajjiyshkxyg");
				client.Send(message);
				client.Disconnect(true);
			}
			return "成功";
		}

		[HttpPost("SendForgetPasswordEmail")]
		public string SendForgetPasswordEmail(Member member,string newPassword)
		{

			var message = new MimeMessage();

			// 添加寄件者
			message.From.Add(new MailboxAddress("PawPaw", "pawpawpetSite@gmail.com"));

			// 添加收件者
			message.To.Add(new MailboxAddress("New Member", $"{member.Email}"));

			// 設定郵件標題
			message.Subject = "PawPaw會員新密碼";

			// 使用 BodyBuilder 建立郵件內容
			var bodyBuilder = new BodyBuilder();

			string result = Request.Scheme + "://" + Request.Host + $"/api/Members/LogIn";

			// 設定 HTML 內容
			bodyBuilder.HtmlBody = "<h1 style='text-align: center; color:darkgreen; font-size:60px;'>PawPaw!</h1>" +
									"<br>"+
								   "<h3 style='text-align: center;font-size:30px;'>感謝您使用PawPaw!，以下是您的新密碼，請使用新密碼登入後至個人資料修改密碼<h3>" +
								   "<h3 style='text-align: center;font-size:30px;'>以下是您的新密碼，請使用新密碼登入後至個人資料修改密碼<h3>" +
								   $"<p  style='text-align: center;font-size:30px;'>新密碼:{newPassword}</p>" +
								   "<p style='text-align: center;'>" +
									$"<a style= 'font-size:20px;' href=\"{result}\">-----點此連結驗證------</a>" +
									"</p>" ;

			// 設定郵件內容
			message.Body = bodyBuilder.ToMessageBody();

			using (var client = new SmtpClient())
			{
				client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
				client.Authenticate("pawpawpetSite@gmail.com", "fbqolajjiyshkxyg");
				client.Send(message);
				client.Disconnect(true);
			}
			return "成功";
		}

		/// 驗證信 啟動帳號
		[HttpGet("EmailConfirm")]
		public IActionResult EmailConfirm(string account, string confirmCode) {

			var emailmember = _context.Members.FirstOrDefault(x => x.Account == account);
			if (emailmember == null)
			{
				return Redirect("https://localhost:7180/Home/Index");
			}
			else if (string.Compare(confirmCode, emailmember.ConfirmCode) != 0) { return Redirect("https://localhost:7180/Home/Index"); }
			else
			{
				emailmember.IsConfirmed = true;

				emailmember.ConfirmCode = null;
				_context.SaveChanges();
				return Redirect("https://localhost:7180/Members/EmailConfirm");
			}
		} 

		[HttpPost("ForgetPassword")]
		public string ForgetPassword(ForgetPasswordDTO forgetPasseordMember) {
			if (ModelState.IsValid == false)
			{
				return "帳號或信箱輸入錯誤";
			}
			var member = _context.Members.SingleOrDefault(x => x.Account == forgetPasseordMember.Account);
			if (member == null) { return "帳號或信箱輸入錯誤"; }
			if (string.Compare(forgetPasseordMember.Email,member.Email)==0) {
				string newpassword = Guid.NewGuid().ToString("N").Substring(0, 8);
				SendForgetPasswordEmail(member, newpassword);
				member.EncryptedPassword = HashUtility.ToSHA256(newpassword, RegisterDTO.SALT);
				_context.SaveChanges();
				return "電子郵件已寄出";
			}else
			{
				return "帳號或信箱輸入錯誤";
			}
		}

		[HttpPost("ResetPassword")]
		public string ResetPassword(ResetPasswordDTO reseter) {

			var member = _context.Members.SingleOrDefault(x => x.Account ==reseter.Account );
			var encryptedPassword = HashUtility.ToSHA256(reseter.OldPassword, RegisterDTO.SALT);
			if (member == null) {
				return "帳號或密碼輸入錯誤";
			}			
			else if (string.Compare(member.EncryptedPassword,encryptedPassword)==0 )
			{
				member.EncryptedPassword = HashUtility.ToSHA256(reseter.NewPassword, RegisterDTO.SALT);
				_context.SaveChanges();
				return "修改成功";
			}
			else {
				return "帳號或密碼輸入錯誤";
			}
		}


		// GET: api/Members/5
		[HttpGet("GetMember")]
		public EditMemberDTO GetMember()
		{
			var account = User.Identity.Name;			
			var member =  _context.Members.FirstOrDefault(x=>x.Account == account);			
			EditMemberDTO thisMember = new EditMemberDTO() { 
				Name = member.Name,
				Address = member.Address,
				BirthDate = member.BirthDate.Value.ToString("yyyy-MM-dd"),
				Mobile = member.Mobile,
				Account = member.Account,
				Email= member.Email,
			};
			
			return thisMember;
		}

		// PUT: api/Members/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost("EditProfile")]
		public string EditProfile(EditMemberDTO editMember)
		{
			var account = User.Identity.Name;
			DateTime? birth;
			if (!DateTime.TryParse(editMember.BirthDate, out DateTime result))
			{
				birth = null;
			}
			else { 
				birth = Convert.ToDateTime(editMember.BirthDate);
			}
			Member member = _context.Members.FirstOrDefault(x => x.Account == account);
			member.Address= editMember.Address;
			member.BirthDate=birth;
			member.Mobile = editMember.Mobile;
			member.Name = editMember.Name;
			_context.SaveChanges();
			return "修改成功";
		}

		

		private bool MemberExists(int id)
		{
			return _context.Members.Any(e => e.MemberId == id);
		}

		/// <summary>
		/// 驗證 Google 登入授權
		/// </summary>
		/// <returns></returns>
		[HttpPost("ValidGoogleLogin")]
        public IActionResult ValidGoogleLogin()
        {
            string? formCredential = Request.Form["credential"]; //回傳憑證
            string? formToken = Request.Form["g_csrf_token"]; //回傳令牌
            string? cookiesToken = Request.Cookies["g_csrf_token"]; //Cookie 令牌

            // 驗證 Google Token
            GoogleJsonWebSignature.Payload? payload = VerifyGoogleToken(formCredential, formToken, cookiesToken).Result;
            if (payload == null)
            {
				return Redirect("https://localhost:7180/Home/Index");
            }
            else

            {
				bool isExist = _context.Members.Any(x=>x.Account==payload.Email);

				if (isExist == false)
				{
					Member member = new Member
					{
						Name = payload.Name,
						Address = "",
						BirthDate =DateTime.Now,
						EncryptedPassword = "",
						Mobile = "",
						Account = payload.Email,
						Email = payload.Email,
						IsConfirmed = true,
						ConfirmCode = null,
						CreateDate = DateTime.Now,
					};
					_context.Members.Add(member);
					_context.SaveChanges();
				}
				
				var claims = new List<Claim>
				{
                    new Claim(ClaimTypes.Name,payload.Email),					
                    new Claim("email",payload.Email),					
					
					
				};

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Secure = true,
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));


                return Redirect("https://localhost:7180/Home/Index");
            }

			
        }

		/// <summary>
		/// 驗證 Google Token
		/// </summary>
		/// <param name="formCredential"></param>
		/// <param name="formToken"></param>
		/// <param name="cookiesToken"></param>
		/// <returns></returns>
		[HttpPost("VerifyGoogle")]
        public async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleToken(string? formCredential, string? formToken, string? cookiesToken)
        {
            // 檢查空值
            if (formCredential == null || formToken == null && cookiesToken == null)
            {
                return null;
            }

            GoogleJsonWebSignature.Payload? payload;
            try
            {
                // 驗證 token
                if (formToken != cookiesToken)
                {
                    return null;
                }

                // 驗證憑證
                IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
                string GoogleApiClientId = Config.GetSection("GoogleApiClientId").Value;
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { GoogleApiClientId }
                };
                payload = await GoogleJsonWebSignature.ValidateAsync(formCredential, settings);
                if (!payload.Issuer.Equals("accounts.google.com") && !payload.Issuer.Equals("https://accounts.google.com"))
                {
                    return null;
                }
                if (payload.ExpirationTimeSeconds == null)
                {
                    return null;
                }
                else
                {
                    DateTime now = DateTime.Now.ToUniversalTime();
                    DateTime expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
                    if (now > expiration)
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
            return payload;
        }

		
		[Authorize]
		[HttpGet("Read")]
		public string Read() {

			var user = User.Identity.Name;									
			var isAuth = User.Identity.IsAuthenticated;		///是否登入	 
			
			
			
			return user;
		
		}
		[HttpGet("IsAuth")]
		public bool IsAuth()
		{			
			var isAuth = User.Identity.IsAuthenticated;     ///是否登入		
			return isAuth;

		}

		
    }
}
