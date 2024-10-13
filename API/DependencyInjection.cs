using Mmicovic.RPSSL.Service;

namespace Mmicovic.RPSSL.API
{
    public class DependencyInjection
    {
        public static void SetupDefaultDependencies(IServiceCollection services)
        {
            services.AddScoped<IGameManager, GameManager>();
            services.AddScoped<IRandomGenerator, ExternalRandomGenerator>();
        }
    }
}
