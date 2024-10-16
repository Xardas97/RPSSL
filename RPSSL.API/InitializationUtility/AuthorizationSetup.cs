using System.Text;

using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Mmicovic.RPSSL.API.Initialization
{
    public class AuthorizationSetup
    {
        public const string JWT_SETTINGS_SECTION_NAME = "JwtSettings";
        public const string ISSUER_CONFIG = "Issuer";
        public const string AUDIENCE_CONFIG = "Audience";
        public const string SECRET_KEY_CONFIG = "Key";

        public const string AUTHORIZATION_COOKIE = "Authorization";

        public static void AddJwtAuthenticationAndAuthorization(IServiceCollection services, IConfiguration configuration)
        {
            // Set JWT schemas
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                // Configure which info is validated on the JWT token
                var jwtConfig = configuration.GetSection(JWT_SETTINGS_SECTION_NAME);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtConfig[ISSUER_CONFIG],
                    ValidAudience = jwtConfig[AUDIENCE_CONFIG],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig[SECRET_KEY_CONFIG]!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
                // Allow sending the token as a cookie in addition to the header
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies[AUTHORIZATION_COOKIE];
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization();
        }
    }
}
