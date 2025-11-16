using StackExchange.Redis;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

public class HistoricalDataJson
{
    [JsonPropertyName("rates")]
    required public Dictionary<string, Dictionary<string, decimal>> Rates { get; set; }
}

public class InitializationService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<InitializationService> _logger;


    public InitializationService(IServiceProvider serviceProvider, ILogger<InitializationService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Initialization started");
        using var scope = _serviceProvider.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();
        var redis = scope.ServiceProvider.GetRequiredService<IDatabase>();
        var externalApiService = scope.ServiceProvider.GetRequiredService<ExternalApiService>();

        _logger.LogInformation("Running migrations");
        await dbContext.Database.MigrateAsync();

        _logger.LogInformation("Load historical data");
        string jsonString = File.ReadAllText("./data.json");
        var data = JsonSerializer.Deserialize<HistoricalDataJson>(jsonString);

        data.Rates.ToList().ForEach(kv =>
        {
            var date = DateTime.Parse(kv.Key).ToUniversalTime();
            dbContext.Add(new Rate(date, "USD", 1.0M));
            kv.Value.ToList().ForEach(rkv =>
                    dbContext.Add(new Rate(date, rkv.Key, rkv.Value)));
        });

        await dbContext.SaveChangesAsync();

        _logger.LogInformation("Running fetch service");
        var resp = await externalApiService.Call("frankfurter");
        if (resp is not null)
        {
            resp.Rates.ToList().ForEach(kv => dbContext.rates.Add(new Rate(DateTime.UtcNow, kv.Key, kv.Value)));
            await dbContext.SaveChangesAsync();
        }

        _logger.LogInformation("Warming cache");
        var latestRates = await dbContext.rates
        .GroupBy(r => r.Currency)
        .Select(g => g.OrderByDescending(r => r.Timestamp).First())
        .ToListAsync();

        await Task.WhenAll(
            latestRates
            .Select(r => redis.HashSetAsync("rates", r.Currency, r.Value.ToString()))
    );

        _logger.LogInformation("Initialization finished");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
