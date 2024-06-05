using Challenge.Bravo.Api.Models;

namespace Challenge.Bravo.Api.Helpers
{
    public static class CheckCurrencyType
    {
        public static bool IsBitCoinCurrency(string currencyCode)
        {
            List<CurrencyCode> _bitCoinCurrencies =
            [
                new CurrencyCode("BTC", "bitcoin/"),
                new CurrencyCode("ETH", "ethereum/")
            ];
            return _bitCoinCurrencies.Any(a => a.Symbol == currencyCode);
        }
        public static bool IsFiatCurrency(string currencyCode)
        {
            List<CurrencyCode> _fiatCurrencies =
            [
                new CurrencyCode("USD", "Us Dollar"),
                new CurrencyCode("BRL", "Brazilian Real"),
                new CurrencyCode("EUR", "Euro")
            ];
            return _fiatCurrencies.Any(a => a.Symbol == currencyCode);
        }
    }
}
