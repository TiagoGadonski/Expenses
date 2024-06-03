using Newtonsoft.Json.Linq;
using System.Text;

public class MercadoBitcoinService
{
    private readonly string _apiKey;
    private readonly string _apiSecret;
    private readonly HttpClient _httpClient;

    public MercadoBitcoinService(HttpClient httpClient, string apiKey, string apiSecret)
    {
        _apiKey = apiKey;
        _apiSecret = apiSecret;
        _httpClient = httpClient;
    }

    public async Task<JObject> GetAccountBalanceAsync()
    {
        string endpoint = "/tapi/v3/account_info";
        string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        string signature = CreateSignature(endpoint, timestamp);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"https://www.mercadobitcoin.net{endpoint}"),
            Headers =
            {
                { "TAPI-ID", _apiKey },
                { "TAPI-MAC", signature }
            }
        };

        var content = new StringContent($"tapi_method=get_account_info&tapi_nonce={timestamp}", Encoding.UTF8, "application/x-www-form-urlencoded");
        request.Content = content;

        using (var response = await _httpClient.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseBody);
        }
    }

    public async Task<JObject> PlaceBuyOrderAsync(string coinPair, decimal quantity, decimal limitPrice)
    {
        string endpoint = "/tapi/v3/order";
        string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        string signature = CreateSignature(endpoint, timestamp);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"https://www.mercadobitcoin.net{endpoint}"),
            Headers =
            {
                { "TAPI-ID", _apiKey },
                { "TAPI-MAC", signature }
            }
        };

        var content = new StringContent($"tapi_method=place_order&tapi_nonce={timestamp}&coin_pair={coinPair}&quantity={quantity}&limit_price={limitPrice}&order_type=buy", Encoding.UTF8, "application/x-www-form-urlencoded");
        request.Content = content;

        using (var response = await _httpClient.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseBody);
        }
    }

    public async Task<JObject> PlaceSellOrderAsync(string coinPair, decimal quantity, decimal limitPrice)
    {
        string endpoint = "/tapi/v3/order";
        string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        string signature = CreateSignature(endpoint, timestamp);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"https://www.mercadobitcoin.net{endpoint}"),
            Headers =
            {
                { "TAPI-ID", _apiKey },
                { "TAPI-MAC", signature }
            }
        };

        var content = new StringContent($"tapi_method=place_order&tapi_nonce={timestamp}&coin_pair={coinPair}&quantity={quantity}&limit_price={limitPrice}&order_type=sell", Encoding.UTF8, "application/x-www-form-urlencoded");
        request.Content = content;

        using (var response = await _httpClient.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseBody);
        }
    }

    private string CreateSignature(string endpoint, string timestamp)
    {
        // Implement your signature creation logic here based on Mercado Bitcoin API requirements
        return "";
    }
}
