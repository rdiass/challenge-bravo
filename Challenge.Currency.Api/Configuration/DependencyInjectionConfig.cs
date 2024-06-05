using Challenge.Bravo.Api.Data;
using Challenge.Bravo.Api.Services;

namespace Challenge.Bravo.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<CurrencyService>();
            services.AddScoped<FiatConvertService>();
            services.AddScoped<BitCoinConvertService>();
            services.AddHttpClient<BitCoinConvertService>();
        }
    }
}
