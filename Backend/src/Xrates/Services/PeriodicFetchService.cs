using Microsoft.EntityFrameworkCore;

public class PeriodicFetchService : BackgroundService
{
    private readonly ILogger<PeriodicFetchService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private int _executionCount = -1;

    public PeriodicFetchService(ILogger<PeriodicFetchService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        using PeriodicTimer timer = new(TimeSpan.FromSeconds(1));

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await DoWork();
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
        }
    }

    private async Task DoWork()
    {
        int count = Interlocked.Increment(ref _executionCount);
        _logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);

        var repositoryService = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IRepositoryService>();
        var integrations = await repositoryService.GetEnabledIntegrationsSorted();
        var primaryIntegration = integrations.First();

        // tick, continue
        if (count != 0 && count % primaryIntegration.FreqSeconds != 0) return;

        bool success;
        success = await FetchAndInsert(primaryIntegration, repositoryService);
        if (success) return;

        var backupIntegrations = integrations.Skip(1);
        foreach (Integration i in backupIntegrations)
        {
            success = await FetchAndInsert(i, repositoryService);
            if (success) return;
        }

    }

    private async Task<bool> FetchAndInsert(Integration integration, IRepositoryService repositoryService)
    {
        var externalApiService = _serviceProvider.GetRequiredService<ExternalApiService>();

        try
        {
            _logger.LogInformation($"Fetching {integration.Name}");
            var res = await externalApiService.Call(integration);
            var inserts = repositoryService.SaveRatesCombinations(res.Timestamp, res.Rates);
            await repositoryService.RefreshCache();
            _logger.LogInformation($"Inserted {inserts} rates");
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("{}({}) integration call failed.", integration.Name, integration.Url);
            _logger.LogError(e.Message);
            repositoryService.DisableIntegration(integration.Name);
            return false;
        }

    }
}
