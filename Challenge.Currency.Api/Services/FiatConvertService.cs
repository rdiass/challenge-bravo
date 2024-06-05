using Challenge.Bravo.Api.Models;
using freecurrencyapi;
using System.Text.Json;

namespace Challenge.Bravo.Api.Services
{
    public class FiatConvertService
    {
        private readonly Freecurrencyapi _freecurrencyapi;

        public FiatConvertService(HttpClient httpClient) => _freecurrencyapi = new Freecurrencyapi("fca_live_0u1MIS050NxSH7kYGzc6cgtfsp7nQzXGunhOv0xd");

        public double ConvertFiatToFiat(string from, string to, double amount)
        {
            var currency = _freecurrencyapi.Latest(from, to);

            var quote = JsonSerializer.Deserialize<Quote>(currency);
            var quoteValue = quote.data[to];
            var response = amount * quoteValue;

            return response;
        }
    }
}
