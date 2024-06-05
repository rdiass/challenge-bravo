using Challenge.Bravo.Api.Models;
using Challenge.Bravo.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Challenge.Bravo.Api.Controllers
{
    [ApiController]
    [Route("api/currency")]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyService _currencyService;

        public CurrencyController(CurrencyService currencyService) => _currencyService = currencyService;

        [HttpGet]
        [Route("from/{from}/to/{to}/amount/{amount}")]
        public async Task<double> ConvertCurrency(string from, string to, double amount) {

            return await _currencyService.ConvertCurrency(from, to, amount);
        }

        [HttpGet]
        public async Task<List<Currency>> Get() =>
            await _currencyService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Currency>> Get(string id)
        {
            var motorcycle = await _currencyService.GetAsync(id);

            if (motorcycle is null)
            {
                return NotFound();
            }

            return motorcycle;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Currency newMotorcycle)
        {
            var response = await _currencyService.CreateAsync(newMotorcycle);

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Currency updatedMotorcycle)
        {
            var book = await _currencyService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            updatedMotorcycle.Id = book.Id;

            await _currencyService.UpdateAsync(id, updatedMotorcycle);

            return NoContent();
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
