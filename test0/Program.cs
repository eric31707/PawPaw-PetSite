using Microsoft.AspNetCore.Authentication.Cookies;

namespace test0
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			string MyAllowOrigins = "AllowAny";
			builder.Services.AddCors(options =>
			{
				options.AddPolicy(
						name: MyAllowOrigins,
						policy => policy.WithOrigins("https://localhost:7150")
						.WithHeaders("*")
						.WithMethods("*")
						.SetIsOriginAllowed(_ => true)
						.AllowAnyHeader()
						.AllowAnyMethod()
						.AllowCredentials()
						.WithExposedHeaders("Name", "email")
						);
			});

			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
			{
				//���n�J�ɷ|�۰ʾɨ�o�Ӻ��}              
				options.LoginPath = "/Members/LogIn";
				options.Cookie.SameSite = SameSiteMode.None;
				

			});
			

			builder.Services.AddControllersWithViews();

			var app = builder.Build();


			
		

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			//app.MapRazorPages();

			app.Run();
		}
	}

}