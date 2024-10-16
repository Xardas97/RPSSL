using Microsoft.EntityFrameworkCore;

using Mmicovic.RPSSL.Service.Models;

namespace Mmicovic.RPSSL.API.Initialization
{
    public class DatabaseSetup
    {
        private const string DATABASE_CONNECTION_STRING = "Database";

        public static void SetupSQLDatabase(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(DATABASE_CONNECTION_STRING)!;

            // Inject database contexts
            services.AddDbContext<UserContext>(opt =>
                opt.UseSqlServer(connectionString, b => b.MigrationsAssembly("RPSSL.API")));
            services.AddDbContext<GameRecordContext>(opt =>
                opt.UseSqlServer(connectionString, b => b.MigrationsAssembly("RPSSL.API")));
        }
    }
}
