using Expenses.Services;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.Controllers
{
    public class MarketController : Controller
    {
        private readonly MercadoBitcoinService _service;

        public MarketController(MercadoBitcoinService service)
        {
            _service = service;
        }

        [HttpGet("{coin}/ticker")]
        public async Task<IActionResult> GetTicker(string coin)
        {
            var ticker = await _service.GetTickerAsync(coin);
            if (ticker == null)
            {
                return NotFound();
            }
            return Ok(ticker);
        }
    }
}
