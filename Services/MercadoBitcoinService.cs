using Expenses.Models;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace Expenses.Services
{
    public class MercadoBitcoinService
    {
        private readonly RestClient _client;
        private readonly string _apiKey;
        private readonly string _apiSecret;

        public MercadoBitcoinService(IOptions<MercadoBitcoinSettings> settings)
        {
            _apiKey = settings.Value.ApiKey;
            _apiSecret = settings.Value.ApiSecret;
            _client = new RestClient("https://www.mercadobitcoin.net/api");
        }

        private RestRequest AddAuthentication(RestRequest request)
        {
            request.AddHeader("Authorization", $"Basic {Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{_apiKey}:{_apiSecret}"))}");
            return request;
        }

        public async Task<TickerResponse> GetTickerAsync(string coin)
        {
            var request = new RestRequest($"{coin}/ticker/", Method.Get);
            var response = await _client.ExecuteAsync<TickerResponse>(request);
            return response.Data;
        }

        public async Task<AccountInfoResponse> GetAccountInfoAsync()
        {
            var request = new RestRequest("/v3/account/", Method.Get);
            AddAuthentication(request);
            var response = await _client.ExecuteAsync<AccountInfoResponse>(request);
            return response.Data;
        }
    }
}
