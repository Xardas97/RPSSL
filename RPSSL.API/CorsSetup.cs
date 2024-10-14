using Microsoft.Net.Http.Headers;

namespace Mmicovic.RPSSL.API
{
    public class CorsSetup
    {
        public const string CORS_POLICY_TEST_UI = "_corsPolicyTestUI";

        private const string TEST_UI_URL = "https://codechallenge.boohma.com";
        private const string FRONTEND_URL_CONFIG = "FrontendUrl";

        public static void AddCorsPolicies(IServiceCollection services, IConfiguration configuration)
        {
            var frontendUrl = configuration.GetConnectionString(FRONTEND_URL_CONFIG)!;

            services.AddCors(options =>
            {
                // Polcy to allow the online test ui and the frontend to use this service
                options.AddPolicy(name: CORS_POLICY_TEST_UI,
                                  policy => policy.WithOrigins(frontendUrl)
                                                  .WithOrigins(TEST_UI_URL)
                                                  .WithHeaders(HeaderNames.ContentType));

                // Allow frontend in the default policy as well
                options.AddDefaultPolicy(policy => policy.WithOrigins(frontendUrl)
                                                         .WithHeaders(HeaderNames.ContentType));
            });
        }
    }
}
