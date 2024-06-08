using CurrencyConverter.Api.Models;
using freecurrencyapi;
using System.Text.Json;

namespace CurrencyConverter.Api.Services
{
    public class FiatConvertService : IFiatConvertService
    {
        private readonly Freecurrencyapi _freecurrencyapi;

        public FiatConvertService(HttpClient httpClient) => _freecurrencyapi = new Freecurrencyapi("fca_live_0u1MIS050NxSH7kYGzc6cgtfsp7nQzXGunhOv0xd");

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
