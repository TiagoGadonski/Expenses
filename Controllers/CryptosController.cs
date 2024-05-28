using Expenses.Services;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.Controllers
{
    public class CryptosController : Controller
    {
        private readonly MercadoBitcoinService _mercadoBitcoinService;

        public CryptosController(IConfiguration configuration)
        {
            var apiKey = configuration["MercadoBitcoin:ApiKey"];
            var apiSecret = configuration["MercadoBitcoin:ApiSecret"];
            _mercadoBitcoinService = new MercadoBitcoinService(apiKey, apiSecret);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var allTickers = await _mercadoBitcoinService.GetAllTickersAsync();
                var accountInfo = await _mercadoBitcoinService.GetAccountInfoAsync();

                ViewBag.AllTickers = allTickers;
                ViewBag.AccountInfo = accountInfo;

                var topGainers = allTickers.OrderByDescending(t => t.Ticker.Ticker.High).Take(5).ToList();
                var lowestPrices = allTickers.OrderBy(t => t.Ticker.Ticker.Low).Take(5).ToList();

                ViewBag.TopGainers = topGainers;
                ViewBag.LowestPrices = lowestPrices;
                ViewBag.GetCoinName = new Func<string, string>(_mercadoBitcoinService.GetCoinName);

                return View();
            }
            catch (Exception ex)
            {
                // Log the error (uncomment ex variable name and write a log.)
                Console.WriteLine($"An error occurred: {ex.Message}");
                return View();
            }
        }
    }
}
