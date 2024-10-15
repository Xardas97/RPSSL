using Microsoft.OpenApi.Models;

namespace Mmicovic.RPSSL.API.Initialization
{
    public class SwaggerSetup
    {
        public static void AddSwaggerGeneration(IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                // Generate button to add a JWT token
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                // Add JWT token to all API calls
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {{
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme,
                        }
                    },
                    Array.Empty<string>()
                }});
            });
        }
    }
}
