using Microsoft.Net.Http.Headers;

namespace Mmicovic.RPSSL.API
{
    public class CorsSetup
    {
        public const string CORS_POLICY_TEST_UI = "_corsPolicyTestUI";

        private const string TEST_UI_URL = "https://codechallenge.boohma.com";

        public static void AddCorsPolicies(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                // Allow the online test ui to use this service
                options.AddPolicy(name: CORS_POLICY_TEST_UI,
                                  policy => policy.WithOrigins(TEST_UI_URL)
                                                  .WithHeaders(HeaderNames.ContentType));
            });
        }
    }
}
