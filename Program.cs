using Expenses.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar serviços HTTP e Cache
builder.Services.AddHttpClient<CoinMarketCapService>(client =>
{
    client.BaseAddress = new Uri("https://pro-api.coinmarketcap.com");
});

builder.Services.AddSingleton(provider =>
{
    var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient();
    return new CoinMarketCapService(httpClient, builder.Configuration["ApiKeys:CoinMarketCapApiKey"]);
});

builder.Services.AddSingleton(provider =>
{
    var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient();
    return new MercadoBitcoinService(httpClient, "cb23ede8f8cf0b7b969938f2b90ba9a8a9bb3611ee9fa1990402b79f8d937df8", "6408ead524f515ff46555eed79f6a1ee0196bfbd5a8a57e877498275fec28df4");
});

builder.Services.AddHttpClient<CryptoDataService>(client =>
{
    client.BaseAddress = new Uri("https://pro-api.coinmarketcap.com");
});

builder.Services.AddHttpClient<CryptoPriceService>();
builder.Services.AddMemoryCache(); // Adicionar serviço de cache
builder.Services.AddTransient<CryptoPriceService>();

builder.Services.AddHostedService<PriceUpdateService>();

builder.Services.AddScoped(provider =>
{
    var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient();
    var dbService = provider.GetRequiredService<DatabaseService>();
    var coinMarketCapApiKey = builder.Configuration["ApiKeys:CoinMarketCapApiKey"];
    var newsApiKey = builder.Configuration["ApiKeys:NewsApiKey"];
    return new CryptoDataService(httpClient, coinMarketCapApiKey, newsApiKey, dbService);
});

builder.Services.AddSingleton<CryptoPredictionService>();
builder.Services.AddSingleton<SentimentAnalysisService>();
builder.Services.AddScoped<CryptoFeedbackService>();
builder.Services.AddScoped<DatabaseService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "crypto",
        pattern: "crypto/{action=CryptoList}/{id?}",
        defaults: new { controller = "Crypto" });

    endpoints.MapControllerRoute(
        name: "cryptoMarketOverview",
        pattern: "crypto/marketoverview",
        defaults: new { controller = "Crypto", action = "MarketOverview" });
});

app.Run();
