using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;

public class CryptoPriceService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly Dictionary<string, string> _cryptoIdMap;

    public CryptoPriceService(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
        _cryptoIdMap = new Dictionary<string, string>
        {
            { "Bitcoin", "bitcoin" },
            { "Ethereum", "ethereum" }
            // Adicione mais mapeamentos conforme necessário
        };
    }

    public async Task<Dictionary<string, string>> GetCryptoIdsAsync()
    {
        var url = "https://api.coingecko.com/api/v3/coins/list";
        var response = await _httpClient.GetStringAsync(url);
        var data = JArray.Parse(response);

        var cryptoIds = new Dictionary<string, string>();
        foreach (var item in data)
        {
            var id = item["id"].Value<string>();
            var name = item["name"].Value<string>();
            cryptoIds[name] = id;
        }

        return cryptoIds;
    }

    public async Task<decimal> GetCryptoPriceAsync(string cryptoName)
    {
        if (!_cryptoIdMap.ContainsKey(cryptoName))
        {
            throw new Exception($"Crypto name '{cryptoName}' is not recognized.");
        }

        var cryptoId = _cryptoIdMap[cryptoName];

        // Try to get price from cache
        if (_cache.TryGetValue(cryptoName, out decimal cachedPrice))
        {
            return cachedPrice;
        }

        var url = $"https://api.coingecko.com/api/v3/simple/price?ids={cryptoId}&vs_currencies=brl";
        var response = await _httpClient.GetStringAsync(url);
        var data = JObject.Parse(response);

        if (data.ContainsKey(cryptoId) && data[cryptoId]["brl"] != null)
        {
            var price = data[cryptoId]["brl"].Value<decimal>();

            // Set cache with expiration time
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // Cache for 5 minutes
            };

            _cache.Set(cryptoName, price, cacheEntryOptions);

            return price;
        }
        else
        {
            throw new Exception($"Could not retrieve price for {cryptoName}");
        }
    }

    public async Task<Dictionary<string, decimal>> GetCurrentPricesAsync(List<string> cryptoNames)
    {
        var prices = new Dictionary<string, decimal>();

        foreach (var cryptoName in cryptoNames)
        {
            if (!_cryptoIdMap.ContainsKey(cryptoName))
            {
                continue; // Ignore criptos não mapeadas
            }

            var cryptoId = _cryptoIdMap[cryptoName];

            // Try to get price from cache
            if (!_cache.TryGetValue(cryptoName, out decimal price))
            {
                price = await GetCryptoPriceAsync(cryptoName);
            }

            prices[cryptoName] = price;
        }

        return prices;
    }
}
