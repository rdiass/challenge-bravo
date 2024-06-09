using CurrencyConverter.Api.Models;
using CurrencyConverter.Api.Services;
using CurrencyConverter.Core.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CurrencyConverter.Api.Controllers
{
    [ApiController]
    [Route("api/converter")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;
        private readonly HttpClient _client;
        private readonly IDatabase _redis;

        public CurrencyController(ICurrencyService currencyService, HttpClient client, IConnectionMultiplexer muxer)
        {
            _currencyService = currencyService;
            _client = client;
            _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("weatherCachingApp", "1.0"));
            _redis = muxer.GetDatabase();
        } 

        [HttpGet]
        [Route("from/{from}/to/{to}/amount/{amount}")]
        public async Task<CurrencyConverterResponse> ConvertCurrency(string from, string to, double amount)
        {
            try
            {
                var keyName = $"converter:{from},{to}";
                var json = await _redis.StringGetAsync(keyName);
                var response = new CurrencyConverterResponse();

                if (string.IsNullOrEmpty(json))
                {
                    var convertion = await _currencyService.ConvertCurrency(from, to, amount);
                    response.Result = convertion;
                    json = JsonSerializer.Serialize(response);
                    var setTask = _redis.StringSetAsync(keyName, json);
                    var expireTask = _redis.KeyExpireAsync(keyName, TimeSpan.FromSeconds(3600));
                    await Task.WhenAll(setTask, expireTask);
                }

                response = JsonSerializer.Deserialize<CurrencyConverterResponse>(json);
                response.ResponseResult = new ResponseResult { Status = (int)HttpStatusCode.OK };
                return response;
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
