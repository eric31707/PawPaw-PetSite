using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetSiteFront.Data;

namespace PetSiteFront
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

            builder.Services.AddAuthentication(option => {
                option.DefaultScheme = ".AspNetCore.Cookies";            
            }).AddCookie(".AspNetCore.Cookies",options =>
			{				           
				options.LoginPath = "/Members/LogIn";
				options.Cookie.SameSite = SameSiteMode.None;
                
			});       
			// Add services to the container.
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
         

            //builder.Services.AddDefaultIdentity<IdentityUser>(options => {
            //    options.SignIn.RequireConfirmedAccount = true;
            //    })
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            
              app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseRouting();                    
            app.UseAuthorization();
            app.MapControllers();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
            name: "room",
            pattern: "{controller=Room}/{action=Payment}/{RoomOrderId}");



            //app.MapRazorPages();

            app.Run();
        }
    }
}