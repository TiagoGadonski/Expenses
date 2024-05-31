using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Expenses.Services
{
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

        private string CreateSignature(string queryString)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_apiSecret)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(queryString));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        private async Task<JObject> SendRequestAsync(string method, string queryString)
        {
            string endpoint = "/tapi/v3/";
            string nonce = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string signature = CreateSignature(queryString);

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

            var content = new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded");
            request.Content = content;

            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return JObject.Parse(responseBody);
            }
        }

        public async Task<JObject> GetAccountBalanceAsync()
        {
            string queryString = $"tapi_method=get_account_info&tapi_nonce={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
            return await SendRequestAsync("POST", queryString);
        }

        public async Task<JObject> PlaceBuyOrderAsync(string coinPair, decimal quantity, decimal limitPrice)
        {
            string queryString = $"tapi_method=place_buy_order&tapi_nonce={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}&coin_pair={coinPair}&quantity={quantity}&limit_price={limitPrice}";
            return await SendRequestAsync("POST", queryString);
        }

        public async Task<JObject> PlaceSellOrderAsync(string coinPair, decimal quantity, decimal limitPrice)
        {
            string queryString = $"tapi_method=place_sell_order&tapi_nonce={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}&coin_pair={coinPair}&quantity={quantity}&limit_price={limitPrice}";
            return await SendRequestAsync("POST", queryString);
        }
    }
}
