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
                var btcTicker = await _mercadoBitcoinService.GetTickerAsync("BTC");
                var ltcTicker = await _mercadoBitcoinService.GetTickerAsync("LTC");
                var ethTicker = await _mercadoBitcoinService.GetTickerAsync("ETH");
                var xmrTicker = await _mercadoBitcoinService.GetTickerAsync("XMR");
                var accountInfo = await _mercadoBitcoinService.GetAccountInfoAsync();

                ViewBag.BtcTicker = btcTicker;
                ViewBag.LtcTicker = ltcTicker;
                ViewBag.EthTicker = ethTicker;
                ViewBag.XmrTicker = xmrTicker;
                ViewBag.AccountInfo = accountInfo;

                var topGainers = await _mercadoBitcoinService.GetTopGainersAsync();
                var lowestPrices = await _mercadoBitcoinService.GetLowestPricesAsync();

                ViewBag.TopGainers = topGainers;
                ViewBag.LowestPrices = lowestPrices;

                return View();
            }
            catch (Exception ex)
            {
                // Log the error (uncomment ex variable name and write a log.)
                Console.WriteLine($"An error occurred: {ex.Message}");
                return View("Error");
            }
        }
    }
}
