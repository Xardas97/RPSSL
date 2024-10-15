using Microsoft.EntityFrameworkCore;

using Mmicovic.RPSSL.Service;
using Mmicovic.RPSSL.Service.Models;

namespace Mmicovic.RPSSL.API.Initialization
{
    public class DependencyInjector
    {
        public static void SetupDefaultDependencies(IServiceCollection services)
        {
            // Inject helper dependencies
            services.AddScoped<IShapeProvider, ShapeProvider>();
            services.AddScoped<IRandomGenerator, ExternalRandomGenerator>();
            services.AddScoped<IGameResultCalculator, GameResultCalculator>();

            // Inject main services
            services.AddScoped<IGameManager, GameManager>();
            services.AddScoped<IUserManager, UserManager>();

            // Inject database repositories and contexts
            services.AddScoped<IGameRecordRepository, GameRecordRepository>();
            services.AddDbContext<GameRecordContext>(opt => opt.UseInMemoryDatabase("GameRecords"));
            services.AddDbContext<UserContext>(opt => opt.UseInMemoryDatabase("Users"));
        }
    }
}
