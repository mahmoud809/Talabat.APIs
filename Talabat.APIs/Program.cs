
namespace Talabat.APIs
{
    public class Program
    {
        //Entry Point
        public static void Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.

            //Allow Dependancy Injection [DI] For Web APIs Services
            webApplicationBuilder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen(); 

            #endregion

            var app = webApplicationBuilder.Build();

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
