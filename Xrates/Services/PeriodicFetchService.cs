using Microsoft.Extensions.Caching.Distributed;

public class FetchService : BackgroundService
{
    private readonly ILogger<FetchService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private int _executionCount;

    public FetchService(ILogger<FetchService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        // When the timer should have no due-time, then do the work once now.
        await DoWork();

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
    }
}
