using CurrencyConverter.Core.Models;
using CurrencyConverter.WebApp.MVC.Models;
using CurrencyConverter.WebApp.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace CurrencyConverter.WebApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICurrencyConverterService _currencyConverterService;

        public List<CurrencyCode> _currencies =
        [
            new CurrencyCode("BTC", "bitcoin/", "BTC - Bitcoin"),
            new CurrencyCode("ETH", "ethereum/", "ETH - Ethereum"),
            new CurrencyCode("BRL", "", "BRL - Brazilian Real"),
            new CurrencyCode("USD", "", "USD - US Dollar"),
            new CurrencyCode("EUR", "", "EUR - Euro")
        ];

        public HomeController(ILogger<HomeController> logger, ICurrencyConverterService currencyConverterService)
        {
            _logger = logger;
            _currencyConverterService = currencyConverterService;
        }

        public IActionResult Index()
        {
            FillViewBag();
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CurrencyConverterViewModel currencyConverterViewModel, string? returnUrl = null)
        {
            FillViewBag();
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid) return View(currencyConverterViewModel);

            var response = await _currencyConverterService.Converter(currencyConverterViewModel);

            if (ResponseHasErrors(response.ResponseResult)) return View(currencyConverterViewModel);

            currencyConverterViewModel.Result = response.Result;

            return View(currencyConverterViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CurrencyViewModel currencyViewModel, string? returnUrl = null)
        {
            FillViewBag();
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid) return View(currencyViewModel);

            var response = await _currencyConverterService.Create(currencyViewModel);

            if (ResponseHasErrors(response.ResponseResult)) return View(currencyViewModel);

            return RedirectToAction("Index", "Home");
        }

        private void FillViewBag()
        {
            // Create a SelectListGroup
            var response = _currencyConverterService.GetAll().Result;

            foreach (var currency in response.Currencies)
            {
                _currencies.Add(new CurrencyCode(currency.Code, "", $"{currency.Code} - {currency.Name}"));
            }

            var listItems = new SelectList(_currencies, "Code", "Name", "");
            ViewBag.SelectedList = listItems;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        protected bool ResponseHasErrors(ResponseResult response)
        {
            if (response != null && response.Errors.Messages.Count != 0)
            {
                foreach (var error in response.Errors.Messages)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return true;
            }

            return false;
        }
    }
}
