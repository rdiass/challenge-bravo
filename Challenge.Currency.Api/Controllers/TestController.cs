using Challenge.Bravo.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Challenge.Bravo.Api.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly CurrencyService _currencyService;

        public TestController(CurrencyService currencyService) => _currencyService = currencyService;

        [HttpGet]
        public async Task<string> Get() => "Test";
    }
}
