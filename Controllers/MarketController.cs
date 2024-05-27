using Expenses.Services;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.Controllers
{
    public class MarketController : Controller
    {
        private readonly MercadoBitcoinService _mercadoBitcoinService;

        public MarketController(MercadoBitcoinService mercadoBitcoinService)
        {
            _mercadoBitcoinService = mercadoBitcoinService;
        }

        public async Task<IActionResult> Index()
        {
            var btcTicker = await _mercadoBitcoinService.GetTickerAsync("BTC");
            var ltcTicker = await _mercadoBitcoinService.GetTickerAsync("LTC");
            var ethTicker = await _mercadoBitcoinService.GetTickerAsync("ETH");
            var xmrTicker = await _mercadoBitcoinService.GetTickerAsync("XMR");
            var accountInfo = await _mercadoBitcoinService.GetAccountInfoAsync();
            var topGainers = await _mercadoBitcoinService.GetTopGainersAsync();
            var lowestPrices = await _mercadoBitcoinService.GetLowestPricesAsync();

            ViewBag.BtcTicker = btcTicker;
            ViewBag.LtcTicker = ltcTicker;
            ViewBag.EthTicker = ethTicker;
            ViewBag.XmrTicker = xmrTicker;
            ViewBag.AccountInfo = accountInfo;
            ViewBag.TopGainers = topGainers;
            ViewBag.LowestPrices = lowestPrices;

            return View();
        }
    }
}
