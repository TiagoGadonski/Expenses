using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;

public class CryptoPriceService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _apiSecret;
    private readonly Dictionary<string, string> _cryptoSymbols;
    private readonly IMemoryCache _cache;

    public CryptoPriceService(HttpClient httpClient, IMemoryCache cache, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _cache = cache;
        _apiKey = configuration["CryptoApi:ApiKey"];
        _apiSecret = configuration["CryptoApi:ApiSecret"];
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
        };
    }

    public async Task<decimal> GetCryptoPriceAsync(string cryptoName)
    {
        if (!_cryptoSymbols.ContainsKey(cryptoName))
        {
            throw new Exception($"Crypto name '{cryptoName}' is not recognized.");
        }

        string symbol = _cryptoSymbols[cryptoName];
        string endpoint = $"https://api.coingecko.com/api/v3/simple/price?ids={symbol}&vs_currencies=usd";

        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error fetching price for {cryptoName}: {response.StatusCode}");
        }

        string content = await response.Content.ReadAsStringAsync();
        JObject json = JObject.Parse(content);

        if (json[symbol] == null || json[symbol]["usd"] == null)
        {
            throw new Exception($"Price information for {cryptoName} is not available.");
        }

        decimal price = json[symbol]["usd"].Value<decimal>();
        return price;
    }

    public async Task<Dictionary<string, decimal>> GetCurrentPricesAsync(List<string> cryptoNames)
    {
        var prices = new Dictionary<string, decimal>();

        foreach (var cryptoName in cryptoNames)
        {
            if (!_cryptoSymbols.ContainsKey(cryptoName))
            {
                continue; // Ignore criptos não mapeadas
            }

            var cryptoId = _cryptoSymbols[cryptoName];

            // Try to get price from cache
            try
            {
                if (!_cache.TryGetValue(cryptoName, out decimal price))
                {
                    price = await GetCryptoPriceAsync(cryptoName);
                }

                prices[cryptoName] = price;
            }
            catch (Exception ex)
            {
                // Log error or handle it accordingly
                Console.WriteLine($"Error fetching price for {cryptoName}: {ex.Message}");
                prices[cryptoName] = 0; // You can choose to set a default value or handle it differently
            }
        }

        return prices;
    }

    public async Task<Dictionary<string, string>> GetCryptoIdsAsync()
    {
        // Retorna apenas os símbolos que foram configurados
        return await Task.FromResult(_cryptoSymbols);
    }
}
