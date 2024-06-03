using Expenses.Data;
using Expenses.Models;

public class PriceUpdateService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public PriceUpdateService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await UpdateCryptoPrices();
                await Task.Delay(TimeSpan.FromHours(3), cancellationToken); // Atualiza a cada hora
            }
        }, cancellationToken);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task UpdateCryptoPrices()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var cryptoPriceService = scope.ServiceProvider.GetRequiredService<CryptoPriceService>();

            var cryptos = context.CryptoTransactions.Select(ct => ct.CryptoName).Distinct().ToList();
            foreach (var crypto in cryptos)
            {
                var price = await cryptoPriceService.GetCryptoPriceAsync(crypto);
                context.CryptoPriceHistories.Add(new CryptoPriceHistory
                {
                    CryptoName = crypto,
                    Price = price,
                    Date = DateTime.UtcNow
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
