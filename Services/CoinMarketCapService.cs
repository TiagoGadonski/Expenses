using System.Net.Http;
using System.Threading.Tasks;
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

    public async Task<JObject> GetLatestListingsAsync(int limit = 10)
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
}
