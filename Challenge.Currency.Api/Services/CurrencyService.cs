using Challenge.Bravo.Api.Data;
using Challenge.Bravo.Api.Helpers;
using Challenge.Bravo.Api.Models;

namespace Challenge.Bravo.Api.Services
{
    public class CurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly FiatConvertService _fiatConvertService;
        private readonly BitCoinConvertService _bitcoinConvertService;

        public CurrencyService(ICurrencyRepository currencyRepository, BitCoinConvertService bitcoinConvertService, FiatConvertService fiatConvertService)
        {
            _fiatConvertService = fiatConvertService;
            _bitcoinConvertService = bitcoinConvertService;
        }

        public async Task<double> ConvertCurrency(string from, string to, double amount)
        {
            if (CheckCurrencyType.IsBitCoinCurrency(from) || CheckCurrencyType.IsBitCoinCurrency(to))
            {
                if (CheckCurrencyType.IsBitCoinCurrency(from) && CheckCurrencyType.IsFiatCurrency(to))
                {
                    return await _bitcoinConvertService.ConvertBitCoinToFiat(from, to, amount);
                }

                if (CheckCurrencyType.IsFiatCurrency(from) && CheckCurrencyType.IsBitCoinCurrency(to))
                {
                    return await _bitcoinConvertService.ConvertFiatToBitCoin(from, to, amount);

                }
            }
            return _fiatConvertService.ConvertFiatToFiat(from, to, amount);
        }

        public async Task<List<Currency>> GetAsync() =>
        await _currencyRepository.GetAsync();

        public async Task<Currency?> GetAsync(string id) =>
            await _currencyRepository.GetAsync(id);

        public async Task<Currency> CreateAsync(Currency newCurrency) =>
            await _currencyRepository.CreateAsync(newCurrency);

        public async Task UpdateAsync(string id, Currency updatedCurrency) =>
            await _currencyRepository.UpdateAsync(id, updatedCurrency);

        public async Task RemoveAsync(string id) =>
            await _currencyRepository.RemoveAsync(id);
    }
}
