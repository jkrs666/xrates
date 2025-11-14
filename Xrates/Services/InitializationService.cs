using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;

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
        var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
        using var dbContext = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();

        _logger.LogInformation("Running migrations");
        await dbContext.Database.MigrateAsync();

        _logger.LogInformation("Warming cache");
        var latestRates = await dbContext.rates
        .GroupBy(r => r.Currency)
        .Select(g => g.OrderByDescending(r => r.Timestamp).First())
        .ToListAsync();

        await Task.WhenAll(
            latestRates
            .Select(r => cache.SetStringAsync(r.Currency, r.Value.ToString())));

        _logger.LogInformation("Initialization finished");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

}
