using Expenses.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Expenses.Controllers
{
    public class CryptosController : Controller
    {
        private readonly MercadoBitcoinService _mercadoBitcoinService;

        public CryptosController(MercadoBitcoinService mercadoBitcoinService)
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

            ViewBag.BtcTicker = btcTicker;
            ViewBag.LtcTicker = ltcTicker;
            ViewBag.EthTicker = ethTicker;
            ViewBag.XmrTicker = xmrTicker;
            ViewBag.AccountInfo = accountInfo;

            return View();
        }
    }
}
