
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;

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

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen(); 

            webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //Allow DI For All Controllers that Implement "IGenericRepository"
            webApplicationBuilder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //Allow DI For AutoMapper [typeof(MappingProfiles)] => means Apply All Config in this AutoMapping class.
            webApplicationBuilder.Services.AddAutoMapper(typeof(MappingProfiles));
            #endregion

            var app = webApplicationBuilder.Build();

            #region Update-Database [Apply All migrations before "Run"] & Data Seeding
            
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _dbContext = services.GetRequiredService<StoreContext>(); // ASK CLR For Creating Object From DbContext Explicitly

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await _dbContext.Database.MigrateAsync(); // Update-Database
                await StoreContextSeed.SeedAsync(_dbContext); // Data Seeding
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error has been occured during apply the migration");

            } 

            #endregion

            #region Configure Kestrel Middlewares [Start Creating Pipelines That Request Will Go Through It]

            // Configure the HTTP request pipeline. [Ordering of piplines is important!]
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapControllers(); //This doesn't make any assumptions about routing and will rely on the user doing attribute routing (most commonly used in WebAPI controllers) to get requests to the right place.

            #endregion

            app.Run();
        }
    }
}
