using MailKit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetSiteAPI.Models.EFModels;
using System.Text;
using System.Security.Policy;

namespace PetSiteAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
           
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var MyAllowOrigins = "AllowAny";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                        name: MyAllowOrigins,
                        policy => policy.WithOrigins("https://localhost:7180")
                        .WithHeaders("*")
                        .WithMethods("*")                        
                        .SetIsOriginAllowed(_=>true)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders("Name","email")     
                        );
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "Paw",
            ValidateAudience = true,
            ValidAudience ="my",
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ASDZXASDHAUISDHASDOHAHSDUAHDS"))
        };
    });
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
			{
				//未登入時會自動導到這個網址--
				option.LoginPath = new PathString("/api/Members/Redirect");
                option.Cookie.SameSite=SameSiteMode.None;
			});
			string PetSiteConnectionString = builder.Configuration.GetConnectionString("PetSite");
            builder.Services.AddDbContext<PetSiteContext>(options => options.UseSqlServer(PetSiteConnectionString));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();
            builder.Services.ConfigureApplicationCookie(config => { config.LoginPath = "/members/redirect"; });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAllOrigins");

            app.UseCors(MyAllowOrigins);

            app.UseHttpsRedirection();
			app.UseCookiePolicy();
			app.UseAuthentication();
			app.UseAuthorization();
			
            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}