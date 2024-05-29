using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class CryptoController : Controller
{
    private readonly CoinMarketCapService _coinMarketCapService;

    public CryptoController(CoinMarketCapService coinMarketCapService)
    {
        _coinMarketCapService = coinMarketCapService;
    }

    public async Task<IActionResult> CryptoList()
    {
        var listings = await _coinMarketCapService.GetLatestListingsAsync();

        if (listings == null || listings["data"] == null)
        {
            // Handle the error or return a view with an error message
            return View("Error", "Unable to retrieve data from CoinMarketCap.");
        }

        return View(listings);
    }
}
