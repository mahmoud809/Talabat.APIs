
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.APIs
{
    public class Program
    {
        //Entry Point
        public static async Task Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.

            //Allow Dependancy Injection [DI] For Web APIs Services
            webApplicationBuilder.Services.AddControllers();

            webApplicationBuilder.Services.AddSwaggerServices();

            webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
            });

            webApplicationBuilder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
            });

            //In-MemeoryDb [Redis] for BasketModule
            webApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>(options => 
            {
                var connection = webApplicationBuilder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });

            webApplicationBuilder.Services.AddApplicationServices(); //"AddApplicationServices()" => Custom Extension Method.

            webApplicationBuilder.Services.AddIdentityServices(webApplicationBuilder.Configuration);

            #endregion

            var app = webApplicationBuilder.Build();

            #region Update-Database [Apply All migrations before "Run"] & Data Seeding
            
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            
            var _dbContext = services.GetRequiredService<StoreContext>(); // ASK CLR For Creating Object From DbContext Explicitly
            
            var _identityDbContext = services.GetRequiredService<AppIdentityDbContext>(); // ASK CLR For Creating Object From AppIdentityDbContext Explicitly

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await _dbContext.Database.MigrateAsync(); // Update-Database
                await StoreContextSeed.SeedAsync(_dbContext); // Data Seeding

                //Identity Db [DI]
                await _identityDbContext.Database.MigrateAsync(); // Update-Database
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error has been occured during apply the migration");

            }

            #endregion

            #region Configure Kestrel Middlewares [Start Creating Pipelines That Request Will Go Through It]

            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline. [Ordering of piplines is important!]
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }

            app.UseStatusCodePagesWithRedirects("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers(); //This doesn't make any assumptions about routing and will rely on the user doing attribute routing (most commonly used in WebAPI controllers) to get requests to the right place.

            #endregion

            app.Run();
        }
    }
}
