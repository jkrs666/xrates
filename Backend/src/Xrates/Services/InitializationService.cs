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
        var repositoryService = scope.ServiceProvider.GetRequiredService<RepositoryService>();

        _logger.LogInformation("Running migrations");
        await dbContext.Database.MigrateAsync();

        _logger.LogInformation("Load historical data");
        string jsonString = File.ReadAllText("./data.json");
        var data = JsonSerializer.Deserialize<HistoricalDataJson>(jsonString);

        data.Rates.ToList().ForEach(kv =>
        {
            var rates = new List<Rate>();
            var date = DateTime.Parse(kv.Key).ToUniversalTime();
            kv.Value.ToList().ForEach(rkv => rates.Add(new Rate(date, "USD", rkv.Key, rkv.Value)));
            repositoryService.SaveRatesCombinations(rates.Last().Timestamp, rates);
        });

        _logger.LogInformation("Warming cache");
        await repositoryService.RefreshCache();

        _logger.LogInformation("Initialization finished");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
