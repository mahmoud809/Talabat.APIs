using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;
using Talabat.Repository.Identity;

namespace Talabat.APIs.Extensions
{
    public static class IdentityServicesExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                //options.Password.RequireDigit = true;
                //options.Password.RequireLowercase = true;
            })
                .AddEntityFrameworkStores<AppIdentityDbContext>(); //Add Defualt configurations for AppUser and Role

            services.AddAuthentication(); // Allow DI for UserManager - RoleManger

            return services; 
        }
    }
}
