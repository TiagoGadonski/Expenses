using Newtonsoft.Json.Linq;

public class CryptoPriceService
{
    private readonly HttpClient _httpClient;

    public CryptoPriceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal> GetCryptoPriceAsync(string cryptoId)
    {
        var url = $"https://api.coingecko.com/api/v3/simple/price?ids={cryptoId}&vs_currencies=brl";
        var response = await _httpClient.GetStringAsync(url);
        var data = JObject.Parse(response);
        return data[cryptoId]["brl"].Value<decimal>();
    }
}
