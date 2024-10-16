using AutoMapper;

using Mmicovic.RPSSL.Service;
using Mmicovic.RPSSL.Service.Models;

namespace Mmicovic.RPSSL.API.Initialization
{
    public class DependencyInjector
    {
        public static void SetupDefaultDependencies(IServiceCollection services, IMapper mapper)
        {
            // Inject helper dependencies
            services.AddSingleton(mapper);
            services.AddScoped<IShapeProvider, ShapeProvider>();
            services.AddScoped<IRandomGenerator, ExternalRandomGenerator>();
            services.AddScoped<IGameResultCalculator, GameResultCalculator>();
            services.AddScoped<IGameRecordRepository, GameRecordRepository>();

            // Inject main services
            services.AddScoped<IGameManager, GameManager>();
            services.AddScoped<IUserManager, UserManager>();
        }
    }
}
