using Microsoft.AspNetCore.Builder;

namespace Talabat.APIs.Extensions
{
    public static class SwaggerServicesExtensions
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }

        public static WebApplication UseSwaggerMiddlewares(this WebApplication webApplication)
        {
            webApplication.UseSwagger();
            webApplication.UseSwaggerUI();

            return webApplication;
        }
    }
}
