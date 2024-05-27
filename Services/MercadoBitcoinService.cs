using Expenses.Models;
using RestSharp;

namespace Expenses.Services
{
    public class MercadoBitcoinService
    {
        private readonly RestClient _client;
        private readonly string _apiKey;
        private readonly string _apiSecret;

        public MercadoBitcoinService(string apiKey, string apiSecret)
        {
            _client = new RestClient("https://www.mercadobitcoin.net/api");
            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        public async Task<TickerResponse> GetTickerAsync(string coin)
        {
            var request = new RestRequest($"{coin}/ticker/", Method.Get);
            request.AddHeader("ApiKey", _apiKey);
            request.AddHeader("ApiSecret", _apiSecret);
            var response = await _client.ExecuteAsync<TickerResponse>(request);

            if (response.Data == null)
            {
                throw new Exception($"Failed to get ticker for {coin}");
            }

            return response.Data;
        }

        private async Task<(string Coin, TickerResponse Ticker)> GetTickerWithCoinAsync(string coin)
        {
            var ticker = await GetTickerAsync(coin);
            return (coin, ticker);
        }

        public async Task<List<(string Coin, TickerResponse Ticker)>> GetTopGainersAsync()
        {
            var coins = new[] { "BTC", "ETH", "LTC", "XMR" };
            var tasks = coins.Select(coin => GetTickerWithCoinAsync(coin)).ToList();
            var tickers = await Task.WhenAll(tasks);
            return tickers.Where(t => t.Ticker?.Ticker != null).OrderByDescending(t => t.Ticker.Ticker.High).Take(5).ToList();
        }

        public async Task<List<(string Coin, TickerResponse Ticker)>> GetLowestPricesAsync()
        {
            var coins = new[] { "BTC", "ETH", "LTC", "XMR" };
            var tasks = coins.Select(coin => GetTickerWithCoinAsync(coin)).ToList();
            var tickers = await Task.WhenAll(tasks);
            return tickers.Where(t => t.Ticker?.Ticker != null).OrderBy(t => t.Ticker.Ticker.Low).Take(5).ToList();
        }

        public async Task<AccountInfoResponse> GetAccountInfoAsync()
        {
            var request = new RestRequest("/v3/account/", Method.Get);
            request.AddHeader("ApiKey", _apiKey);
            request.AddHeader("ApiSecret", _apiSecret);
            var response = await _client.ExecuteAsync<AccountInfoResponse>(request);

            if (response.Data == null)
            {
                throw new Exception("Failed to get account info");
            }

            return response.Data;
        }
    }
}
