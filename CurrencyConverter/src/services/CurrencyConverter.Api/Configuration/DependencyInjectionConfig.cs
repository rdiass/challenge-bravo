using CurrencyConverter.Api.Data;
using CurrencyConverter.Api.Services;

namespace CurrencyConverter.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<FiatConvertService>();
            services.AddScoped<BitCoinConvertService>();
            services.AddScoped<FictionCurrencyService>();
            services.AddScoped<CurrencyService>();
            services.AddHttpClient<BitCoinConvertService>();
        }
    }
}
