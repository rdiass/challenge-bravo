using Challenge.Bravo.Api.Data;
using Challenge.Bravo.Api.Settings;
using Microsoft.EntityFrameworkCore;

namespace Challenge.Bravo.Api.Configuration
{
    public static class MongoDbConfiguration
    {
        public static IServiceCollection AddMongoDbConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoDBSettings = configuration.GetSection("CurrencyDatabase").Get<CurrencyDatabaseSettings>();

            services.Configure<CurrencyDatabaseSettings>(configuration.GetSection("CurrencyDatabase"));

            services.AddDbContext<CurrencyDbContext>(options =>
                options.UseMongoDB(mongoDBSettings.ConnectionString, mongoDBSettings.DatabaseName));

            return services;
        }
    }
}
