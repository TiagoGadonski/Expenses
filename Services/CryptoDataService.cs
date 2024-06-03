using Expenses.Models;
using Newtonsoft.Json.Linq;

public class CryptoDataService
{
    private readonly HttpClient _httpClient;
    private readonly string _coinMarketCapApiKey;
    private readonly string _newsApiKey;
    private readonly DatabaseService _dbService;

    public CryptoDataService(HttpClient httpClient, string coinMarketCapApiKey, string newsApiKey, DatabaseService dbService)
    {
        _httpClient = httpClient;
        _coinMarketCapApiKey = coinMarketCapApiKey;
        _newsApiKey = newsApiKey;
        _dbService = dbService;
    }

    public async Task<JObject> GetCryptoPricesAsync()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest?convert=USD"),
            Headers =
            {
                { "X-CMC_PRO_API_KEY", _coinMarketCapApiKey }
            }
        };

        using (var response = await _httpClient.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseBody);
        }
    }

    public async Task<JObject> GetCryptoNewsAsync()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://newsapi.org/v2/everything?q=cryptocurrency&apiKey={_newsApiKey}")
        };

        request.Headers.Add("User-Agent", "Mozilla/5.0");

        using (var response = await _httpClient.SendAsync(request))
        {
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Log the response body to understand the error
                Console.WriteLine($"Error fetching news: {response.StatusCode}");
                Console.WriteLine(responseBody);
            }

            response.EnsureSuccessStatusCode();
            return JObject.Parse(responseBody);
        }
    }

    public async Task StoreCryptoDataAsync(JObject prices, JObject news)
    {
        // Extract relevant data and store it in the database
        var priceData = prices["data"]
            .Select(c => new CryptoPrice
            {
                Name = c["name"].ToString(),
                Symbol = c["symbol"].ToString(),
                Price = (float)c["quote"]["USD"]["price"],
                LastUpdated = DateTime.Parse(c["last_updated"].ToString())
            })
            .ToList();

        var newsData = news["articles"]
            .Select(a => new CryptoNews
            {
                Title = a["title"].ToString(),
                Description = a["description"].ToString(),
                Url = a["url"].ToString(),
                PublishedAt = DateTime.Parse(a["publishedAt"].ToString())
            })
            .ToList();

        await _dbService.StorePricesAsync(priceData);
        await _dbService.StoreNewsAsync(newsData);
    }
}
