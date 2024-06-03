using Expenses.Models;
using Microsoft.AspNetCore.Mvc;

public class CryptoController : Controller
{
    private readonly CoinMarketCapService _coinMarketCapService;
    private readonly MercadoBitcoinService _mercadoBitcoinService;
    private readonly CryptoDataService _cryptoDataService;
    private readonly CryptoPredictionService _cryptoPredictionService;
    private readonly SentimentAnalysisService _sentimentAnalysisService;
    private readonly CryptoFeedbackService _cryptoFeedbackService;

    public CryptoController(
        CoinMarketCapService coinMarketCapService,
        MercadoBitcoinService mercadoBitcoinService,
        CryptoDataService cryptoDataService,
        CryptoPredictionService cryptoPredictionService,
        SentimentAnalysisService sentimentAnalysisService,
        CryptoFeedbackService cryptoFeedbackService)
    {
        _coinMarketCapService = coinMarketCapService;
        _mercadoBitcoinService = mercadoBitcoinService;
        _cryptoDataService = cryptoDataService;
        _cryptoPredictionService = cryptoPredictionService;
        _sentimentAnalysisService = sentimentAnalysisService;
        _cryptoFeedbackService = cryptoFeedbackService;
    }

    public async Task<IActionResult> MarketOverview()
    {
        var overview = await _coinMarketCapService.GetMarketOverviewAsync();
        var accountBalance = await _mercadoBitcoinService.GetAccountBalanceAsync();

        var model = new
        {
            MarketOverview = overview,
            AccountBalance = accountBalance
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Buy(string coinPair, decimal quantity, decimal limitPrice)
    {
        var result = await _mercadoBitcoinService.PlaceBuyOrderAsync(coinPair, quantity, limitPrice);
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> Sell(string coinPair, decimal quantity, decimal limitPrice)
    {
        var result = await _mercadoBitcoinService.PlaceSellOrderAsync(coinPair, quantity, limitPrice);
        return Json(result);
    }

    public async Task<IActionResult> MarketSummary()
    {
        // Recolher dados de preços e notícias
        var prices = await _cryptoDataService.GetCryptoPricesAsync();
        var news = await _cryptoDataService.GetCryptoNewsAsync();
        var accountBalance = await _mercadoBitcoinService.GetAccountBalanceAsync();

        // Pré-processar os dados e calcular o sentimento
        var cryptoDataList = prices["data"]
            .Select(c => new CryptoPrice
            {
                LastUpdated = DateTime.Parse(c["last_updated"].ToString()),
                Price = (float)c["quote"]["USD"]["price"],
                SentimentScore = _sentimentAnalysisService.PredictSentiment(c["name"].ToString() + " " + c["quote"]["USD"]["price"].ToString())
            })
            .ToList();

        // Treinar o modelo com os dados coletados
        _cryptoPredictionService.TrainModel(cryptoDataList);

        // Fazer previsões com os dados atuais
        var predictions = cryptoDataList
            .Select(data => new
            {
                Date = data.LastUpdated,
                ActualPrice = data.Price,
                PredictedPrice = _cryptoPredictionService.Predict(data)
            })
            .ToList<dynamic>();

        // Armazenar feedback das previsões
        foreach (var prediction in predictions)
        {
            var cryptoFeedback = new CryptoFeedback
            {
                Date = prediction.Date,
                PredictedPrice = prediction.PredictedPrice,
                ActualPrice = prediction.ActualPrice,
                SentimentScore = cryptoDataList.First(c => c.LastUpdated == prediction.Date).SentimentScore
            };

            await _cryptoFeedbackService.StoreFeedbackAsync(cryptoFeedback, prediction.ActualPrice);
        }

        // Gerar resumo diário e dicas de investimento
        var dailySummary = GenerateDailySummary(predictions);
        var investmentTips = GenerateInvestmentTips(predictions);

        var model = new
        {
            DailySummary = dailySummary,
            InvestmentTips = investmentTips,
            News = news["articles"],
            AccountBalance = accountBalance
        };

        return View(model);
    }

    private string GenerateDailySummary(List<dynamic> predictions)
    {
        // Lógica de geração de resumo diário
        var latestPrediction = predictions.Last();
        return $"Predicted price for {latestPrediction.Date.ToShortDateString()}: ${latestPrediction.PredictedPrice:N2}";
    }

    private List<string> GenerateInvestmentTips(List<dynamic> predictions)
    {
        // Lógica de geração de dicas de investimento
        return predictions
            .Where(p => p.PredictedPrice > p.ActualPrice)
            .Select(p => $"Consider investing on {p.Date.ToShortDateString()} with predicted price: ${p.PredictedPrice:N2}")
            .ToList();
    }
}
