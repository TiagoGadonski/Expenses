using Expenses.Data;
using Expenses.Models;
using Microsoft.EntityFrameworkCore;

public class DatabaseService
{
    private readonly ApplicationDbContext _context;

    public DatabaseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task StorePricesAsync(IEnumerable<CryptoPrice> prices)
    {
        await _context.CryptoPrices.AddRangeAsync(prices);
        await _context.SaveChangesAsync();
    }

    public async Task StoreNewsAsync(IEnumerable<CryptoNews> news)
    {
        await _context.CryptoNews.AddRangeAsync(news);
        await _context.SaveChangesAsync();
    }

    public async Task StoreFeedbackAsync(CryptoFeedback feedback)
    {
        await _context.CryptoFeedbacks.AddAsync(feedback);
        await _context.SaveChangesAsync();
    }

    public async Task<List<CryptoFeedback>> GetFeedbackAsync()
    {
        return await _context.CryptoFeedbacks.ToListAsync();
    }
}
