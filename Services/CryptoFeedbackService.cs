using Expenses.Models;

public class CryptoFeedbackService
{
    private readonly DatabaseService _dbService;

    public CryptoFeedbackService(DatabaseService dbService)
    {
        _dbService = dbService;
    }

    public async Task StoreFeedbackAsync(CryptoFeedback feedback, float actualPrice)
    {
        feedback.ActualPrice = actualPrice;
        await _dbService.StoreFeedbackAsync(feedback);
    }

    public async Task<List<CryptoFeedback>> GetFeedbackAsync()
    {
        return await _dbService.GetFeedbackAsync();
    }
}
