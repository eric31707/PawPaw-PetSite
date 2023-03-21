namespace PetSiteAPI.Infra.ExtensionMethods.MembersExtension
{
    public class CorsExtension
    {
        public static IServiceCollection AddCorsServices(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Dev", policy =>
                {
                    policy
                    .AllowAnyOrigin()  // Access-Control-Allow-Origin: *
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });

                // ... 略 ...

            });

            return services;
        }
}
