using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Asp.Versioning.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerGen;

// Taken from: https://medium.com/@celery_liu/asp-net-core-web-api-with-swagger-api-versioning-for-dotnet-8-c8ce2fd7808c
namespace Mmicovic.RPSSL.API.Initialization
{
    public class ApiVersioningSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = "API",
                Version = description.ApiVersion.ToString(),
                Description = description.IsDeprecated ? "This version is deprecated." : null
            }; return info;
        }
    }

    public class SwaggerSetup
    {
        public static void AddSwaggerGen(IServiceCollection services)
        {
            services.AddSwaggerGen();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ApiVersioningSwaggerOptions>();
        }

        public static void UseSwagger(WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var descriptions = app.DescribeApiVersions();
                foreach (var description in descriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
        }
    }
}
