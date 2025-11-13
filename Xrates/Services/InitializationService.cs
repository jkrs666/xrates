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
        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
        using var context = dbContextFactory.CreateDbContext();

        _logger.LogInformation("Running migrations");
        await context.Database.MigrateAsync();

        _logger.LogInformation("Warming cache");
        var repository = scope.ServiceProvider.GetRequiredService<RepositoryService>();
        var latestRates = await repository.GetLatestRatesFromDb();
        await Task.WhenAll(latestRates.Select(r => cache.SetStringAsync(r.Currency, r.Value.ToString())));

        _logger.LogInformation("Initialization finished");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

}
