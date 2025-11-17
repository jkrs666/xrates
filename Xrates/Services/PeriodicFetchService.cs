using Microsoft.EntityFrameworkCore;

public class PeriodicFetchService : BackgroundService
{
    private readonly ILogger<PeriodicFetchService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private int _executionCount;

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
        var repositoryService = _serviceProvider.GetRequiredService<RepositoryService>();
        var externalApiService = _serviceProvider.GetRequiredService<ExternalApiService>();
        var integrations = await repositoryService.GetIntegrations();

        var calls = integrations
            .Where(i => (count % i.FreqSeconds) == 0)
            .Select(async i =>
            {
                var res = await externalApiService.Call(i);
                var inserts = repositoryService.SaveRatesCombinations(res.Timestamp, res.Rates);
                _logger.LogInformation(inserts.ToString());
            }).ToList();

        _logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);
    }
}
