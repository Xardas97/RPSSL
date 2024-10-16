using Asp.Versioning;

namespace Mmicovic.RPSSL.API.Initialization
{
    public class ApiVersioningSetup
    {
        private const string API_VERSION_HEADER = "X-Api-Version";

        public static void Setup(IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                // Allow the client not to specify the version
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;

                // Report versions to the clients
                options.ReportApiVersions = true;

                // Api version is specified in the URL
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                // Swagger support
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }
    }
}
