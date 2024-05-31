using Expenses.Models;

public class CryptoFeedbackService
{
    private readonly DatabaseService _dbService;

    public CryptoFeedbackService(DatabaseService dbService)
    {
        _dbService = dbService;
    }

    public async Task StoreFeedbackAsync(CryptoPrice prediction, float actualPrice)
    {
        var feedback = new CryptoFeedback
        {
            Date = prediction.LastUpdated,
            PredictedPrice = prediction.Price,
            ActualPrice = actualPrice,
            SentimentScore = prediction.SentimentScore
        };

        await _dbService.StoreFeedbackAsync(feedback);
    }

    public async Task<List<CryptoFeedback>> GetFeedbackAsync()
    {
        return await _dbService.GetFeedbackAsync();
    }
}
