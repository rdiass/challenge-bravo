using CurrencyConverter.Api.Models;
using CurrencyConverter.Api.Settings;
using freecurrencyapi;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CurrencyConverter.Api.Services
{
    public class FiatConvertService : Service, IFiatConvertService
    {
        private readonly Freecurrencyapi _freecurrencyapi;

        public FiatConvertService(HttpClient httpClient, IOptions<AppSettings> appSettings)
        {
            _freecurrencyapi = new Freecurrencyapi(appSettings.Value.FreecurrencyapiKey);
        }

        public double ConvertFiatToFiat(string from, string to, double amount)
        {
            try
            {
                var currency = _freecurrencyapi.Latest(from, to);
                var quote = JsonSerializer.Deserialize<Quote>(currency);
                var quoteValue = quote.data[to];
                var response = amount * quoteValue;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Not supported currency: " + ex.Message);
            }
        }
    }
}
