using CurrencyConverter.Api.Models;
using CurrencyConverter.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverter.Api.Controllers
{
    [ApiController]
    [Route("api/currency")]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyService _currencyService;

        public CurrencyController(CurrencyService currencyService) => _currencyService = currencyService;

        [HttpGet]
        [Route("from/{from}/to/{to}/amount/{amount}")]
        public async Task<double> ConvertCurrency(string from, string to, double amount)
        {

            return await _currencyService.ConvertCurrency(from, to, amount);
        }

        [HttpGet]
        public async Task<List<CurrencyViewModel>> Get() =>
            await _currencyService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<CurrencyViewModel>> Get(string id)
        {
            var currency = await _currencyService.GetAsync(id);

            if (currency is null)
            {
                return NotFound();
            }

            return currency;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CurrencyViewModel newCurrency)
        {
            var response = await _currencyService.CreateAsync(newCurrency);

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _currencyService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            await _currencyService.RemoveAsync(id);

            return NoContent();
        }
    }
}
