using Microsoft.EntityFrameworkCore;

using Mmicovic.RPSSL.Service;
using Mmicovic.RPSSL.Service.Models;

namespace Mmicovic.RPSSL.API
{
    public class DependencyInjection
    {
        public static void SetupDefaultDependencies(IServiceCollection services)
        {
            services.AddScoped<IShapeProvider, ShapeProvider>();
            services.AddScoped<IGameResultCalculator, GameResultCalculator>();
            services.AddScoped<IGameManager, GameManager>();
            services.AddScoped<IRandomGenerator, ExternalRandomGenerator>();

            // Inject database repositories and contexts
            services.AddScoped<IGameRecordRepository, GameRecordRepository>();
            services.AddDbContext<GameRecordContext>(opt => opt.UseInMemoryDatabase("GameRecords"));
        }
    }
}
