using Expenses.Models;
using RestSharp;

namespace Expenses.Services
{
    public class MercadoBitcoinService
    {
        private readonly RestClient _client;
        private readonly string _apiKey;
        private readonly string _apiSecret;

        private readonly Dictionary<string, string> _coinNames = new Dictionary<string, string>
        {
            { "BTC", "Bitcoin" },
            { "ETH", "Ethereum" },
            { "XRP", "Ripple" },
            { "ADA", "Cardano" },
            { "PEPE", "PepeCoin" },
            { "DOGE", "Dogecoin" },
            { "DOT", "Polkadot" },
            { "UNI", "Uniswap" },
            { "LTC", "Litecoin" },
            { "BCH", "Bitcoin Cash" }
            // Adicione mais moedas conforme necessário
        };

        public MercadoBitcoinService(string apiKey, string apiSecret)
        {
            _client = new RestClient("https://www.mercadobitcoin.net/api");
            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        public async Task<TickerResponse> GetTickerAsync(string coin)
        {
            var request = new RestRequest($"{coin}/ticker/", Method.Get);
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

        public async Task<List<(string Coin, TickerResponse Ticker)>> GetAllTickersAsync()
        {
            var coins = await GetAvailableCoinsAsync();
            var tasks = coins.Select(coin => GetTickerWithCoinAsync(coin)).ToList();
            var tickers = await Task.WhenAll(tasks);
            return tickers.Where(t => t.Ticker != null && t.Ticker.Ticker != null).ToList();
        }

        private async Task<List<string>> GetAvailableCoinsAsync()
        {
            // Simulating a call to an endpoint that would return all available coins.
            // In a real scenario, replace this with an actual API call if available.
            return _coinNames.Keys.ToList();
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

        public string GetCoinName(string coin)
        {
            return _coinNames.TryGetValue(coin, out var name) ? name : coin;
        }
    }
}
