using Challenge.Bravo.Api.Models;
using System.Net.Http;

namespace Challenge.Bravo.Api.Services
{
    public class BitCoinConvertService
    {
        private readonly FiatConvertService _fiatConvertService;
        private readonly HttpClient _httpClient;

        public List<CurrencyCode> _bitCoinCurrencies =
        [
            new CurrencyCode("BTC", "bitcoin/"),
            new CurrencyCode("ETH", "ethereum/")
        ];

        public BitCoinConvertService(FiatConvertService fiatConvertService, HttpClient httpClient)
        {
            _fiatConvertService = fiatConvertService;
            httpClient.BaseAddress = new Uri("https://api.alternative.me/v2/ticker/");
            _httpClient = httpClient;
        }

        public async Task<double> ConvertBitCoinToFiat(string from, string to, double amount)
        {
            //get bitcoin (BTC or ETH) value in dollar
            var bitcoinValueInDollar = await GetBitCoinValueInDollar(from);

            //amount de bitcoins in dollar
            var amountBitCoinsInDollar = amount * bitcoinValueInDollar;

            //convert value in dollar to another fiat currency (ex: BRL)
            double amountFinal = amountBitCoinsInDollar;
            if (from != "USD")
            {
                amountFinal = _fiatConvertService.ConvertFiatToFiat("USD", to, amountBitCoinsInDollar);
            }

            return amountFinal;
        }

        public async Task<double> ConvertFiatToBitCoin(string from, string to, double amount)
        {
            //get bitcoin (BTC or ETH) value in dollar
            var bitcoinValueInDollar = await GetBitCoinValueInDollar(to);

            //convert 'from' fiat currency amount in dollar amount
            double amountInDollar = amount;
            if (from != "USD")
            {
                amountInDollar = _fiatConvertService.ConvertFiatToFiat(from, "USD", amount);
            }

            //calculate amount in dollar to bitcoin
            return amountInDollar / bitcoinValueInDollar;
        }

        public async Task<double> GetBitCoinValueInDollar(string btcSymbol)
        {
            var bitcoin = _bitCoinCurrencies.First(a => a.Symbol == btcSymbol);
            var bitcoinValueInDollar = await GetBitCoinDollarQuoteValue(bitcoin.Path);

            return bitcoinValueInDollar;
        }

        private async Task<double> GetBitCoinDollarQuoteValue(string path)
        {
            var test = await _httpClient.GetAsync(path);
            var quotes = await test.Content.ReadFromJsonAsync<BitcoinQuotes>();

            if (path == "bitcoin/")
                return quotes.Data._1.Quotes.USD.Price;

            if (path == "ethereum/")
                return quotes.Data._1027.Quotes.USD.Price;

            throw new Exception("Bitcoin not supported");
        }
    }
}
