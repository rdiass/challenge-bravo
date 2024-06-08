using CurrencyConverter.Api.Models;
using CurrencyConverter.Api.Services;
using CurrencyConverter.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CurrencyConverter.Api.Controllers
{
    [ApiController]
    [Route("api/converter")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService) => _currencyService = currencyService;

        [HttpGet]
        [Route("from/{from}/to/{to}/amount/{amount}")]
        public async Task<CurrencyConverterResponse> ConvertCurrency(string from, string to, double amount)
        {
            try
            {
                var result = await _currencyService.ConvertCurrency(from, to, amount);
                var responseResult = new ResponseResult { Status = (int)HttpStatusCode.OK };
                return new CurrencyConverterResponse { Result = (float)result, ResponseResult = responseResult };
            }
            catch (Exception ex)
            {
                var responseResult = new ResponseResult
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Errors = new ResponseErrorMessages
                    {
                        Messages = [ex.Message]
                    }
                };
                return new CurrencyConverterResponse { ResponseResult = responseResult };
            }
        }

        [HttpGet]
        public async Task<List<CurrencyViewModel>> Get() =>
            await _currencyService.GetAsync();

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
