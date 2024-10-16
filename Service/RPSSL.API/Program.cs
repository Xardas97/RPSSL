using AutoMapper;
using Mmicovic.RPSSL.API.Initialization;
using Mmicovic.RPSSL.API.InitializationUtility;

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
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Run custom setups for: CORS, JWT Auth, Dependency injection, AutoMapper, Database
            CorsSetup.AddCorsPolicies(builder.Services, builder.Configuration);
            AuthorizationSetup.AddJwtAuthenticationAndAuthorization(builder.Services, builder.Configuration);
            var mapper = AutoMapperSetup.CreateMapper();
            DependencyInjector.SetupDefaultDependencies(builder.Services, mapper);
            DatabaseSetup.SetupSQLDatabase(builder.Services, builder.Configuration);

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
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
