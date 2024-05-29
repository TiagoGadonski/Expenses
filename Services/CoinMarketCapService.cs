using Newtonsoft.Json.Linq;

public class CoinMarketCapService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public CoinMarketCapService(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
    }

    public async Task<JObject> GetLatestListingsAsync(int limit = 100)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest?limit={limit}&convert=BRL"),
            Headers =
            {
                { "Accepts", "application/json" },
                { "X-CMC_PRO_API_KEY", _apiKey }
            }
        };

        using (var response = await _httpClient.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseBody);
        }
    }

    public async Task<JObject> GetMarketOverviewAsync()
    {
        var data = await GetLatestListingsAsync();

        var mostExpensive = data["data"]
            .OrderByDescending(c => (decimal)c["quote"]["BRL"]["price"])
            .Take(10);

        var cheapest = data["data"]
            .OrderBy(c => (decimal)c["quote"]["BRL"]["price"])
            .Take(10);

        var topGainers = data["data"]
            .OrderByDescending(c => (decimal)c["quote"]["BRL"]["percent_change_24h"])
            .Take(10);

        var topLosers = data["data"]
            .OrderBy(c => (decimal)c["quote"]["BRL"]["percent_change_24h"])
            .Take(10);

        var marketOverview = new JObject
        {
            ["mostExpensive"] = new JArray(mostExpensive),
            ["cheapest"] = new JArray(cheapest),
            ["topGainers"] = new JArray(topGainers),
            ["topLosers"] = new JArray(topLosers),
            ["totalMarketCap"] = data["data"].Sum(c => (decimal)c["quote"]["BRL"]["market_cap"]),
            ["totalVolume24h"] = data["data"].Sum(c => (decimal)c["quote"]["BRL"]["volume_24h"])
        };

        return marketOverview;
    }
}
