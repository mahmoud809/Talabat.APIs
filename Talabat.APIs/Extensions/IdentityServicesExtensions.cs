using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;
using Talabat.Repository.Identity;
using Talabat.Service;

namespace Talabat.APIs.Extensions
{
    public static class IdentityServicesExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>(); 

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                //options.Password.RequireDigit = true;
                //options.Password.RequireLowercase = true;
            })
                .AddEntityFrameworkStores<AppIdentityDbContext>(); //Add Defualt configurations for AppUser and Role

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(); // Allow DI for UserManager - RoleManger

            return services; 
        }
    }
}
