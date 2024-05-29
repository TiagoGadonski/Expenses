using Microsoft.AspNetCore.Mvc;

public class CryptoController : Controller
{
    private readonly CoinMarketCapService _coinMarketCapService;

    public CryptoController(CoinMarketCapService coinMarketCapService)
    {
        _coinMarketCapService = coinMarketCapService;
    }

    public async Task<IActionResult> MarketOverview()
    {
        var overview = await _coinMarketCapService.GetMarketOverviewAsync();
        return View(overview);
    }
}
