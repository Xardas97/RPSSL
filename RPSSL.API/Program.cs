
namespace Mmicovic.RPSSL.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add the configuration files
            builder.Configuration.AddJsonFile("appsettings.json", optional: false);
            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddJsonFile($"appsettings.{Environments.Development}.json", optional: false);
            }

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add custom CORS policies
            CorsSetup.AddCorsPolicies(builder.Services);
            // Setup all default dependencies
            DependencyInjection.SetupDefaultDependencies(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseExceptionHandler("/error-dev");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.UseCors();

            app.Run();
        }
    }
}