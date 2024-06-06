using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

public class MercadoBitcoinService
{
    private readonly string _apiKey;
    private readonly string _apiSecret;
    private readonly HttpClient _httpClient;
    private readonly Dictionary<string, string> _cryptoSymbols;

    public MercadoBitcoinService(HttpClient httpClient, string apiKey, string apiSecret) // Corrigido aqui
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
        _apiSecret = apiSecret;
        _cryptoSymbols = new Dictionary<string, string>
        {
            { "Bitcoin", "BTC" },
            { "Ethereum", "ETH" },
            { "Shiba Inu", "SHIB" },
            { "BitTorrent", "BTT" },
            { "APENFT", "NFT" },
            { "Memecoin", "MEME" },
            { "Bonk", "BONK" },
            { "Pepe", "PEPE" },
            { "Dogecoin", "DOGE" },
            { "Dogewifhat", "DOGEWIFHAT" },
            { "Gods Unchained", "GODS" }
            // Adicione outras criptomoedas aqui conforme necessário
        };
    }

    public async Task<decimal> GetCryptoPriceAsync(string cryptoName)
    {
        if (!_cryptoSymbols.ContainsKey(cryptoName))
        {
            throw new Exception($"Crypto name '{cryptoName}' is not recognized.");
        }

        string symbol = _cryptoSymbols[cryptoName];
        string endpoint = $"https://api.somecryptoservice.com/v1/ticker/{symbol}";

        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error fetching price for {cryptoName}: {response.StatusCode}");
        }

        string content = await response.Content.ReadAsStringAsync();
        JObject json = JObject.Parse(content);
        decimal price = json["price"].Value<decimal>();

        return price;
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
            var responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching account balance: {response.StatusCode}. Content: {responseBody}");
            }
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
            var responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error placing sell order: {response.StatusCode}. Content: {responseBody}");
            }
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
            var responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error placing buy order: {response.StatusCode}. Content: {responseBody}");
            }
            return JObject.Parse(responseBody);
        }
    }

    private string CreateSignature(string endpoint, string timestamp)
    {
        var preHash = $"{_apiKey}{endpoint}{timestamp}";
        var keyBytes = Encoding.UTF8.GetBytes(_apiSecret);
        var preHashBytes = Encoding.UTF8.GetBytes(preHash);
        using (var hmac = new HMACSHA512(keyBytes))
        {
            var hashBytes = hmac.ComputeHash(preHashBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
