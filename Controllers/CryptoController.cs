using Expenses.Services;
using Microsoft.AspNetCore.Mvc;

public class CryptoController : Controller
{
    private readonly CoinMarketCapService _coinMarketCapService;
    private readonly MercadoBitcoinService _mercadoBitcoinService;

    public CryptoController(CoinMarketCapService coinMarketCapService, MercadoBitcoinService mercadoBitcoinService)
    {
        _coinMarketCapService = coinMarketCapService;
        _mercadoBitcoinService = mercadoBitcoinService;
    }

    public async Task<IActionResult> MarketOverview()
    {
        var overview = await _coinMarketCapService.GetMarketOverviewAsync();
        var accountBalance = await _mercadoBitcoinService.GetAccountBalanceAsync();

        var model = new
        {
            MarketOverview = overview,
            AccountBalance = accountBalance
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Buy(string coinPair, decimal quantity, decimal limitPrice)
    {
        var result = await _mercadoBitcoinService.PlaceBuyOrderAsync(coinPair, quantity, limitPrice);
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> Sell(string coinPair, decimal quantity, decimal limitPrice)
    {
        var result = await _mercadoBitcoinService.PlaceSellOrderAsync(coinPair, quantity, limitPrice);
        return Json(result);
    }
}
