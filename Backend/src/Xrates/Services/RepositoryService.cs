using StackExchange.Redis;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

public class RepositoryService : IRepositoryService
{
    private readonly ILogger<RepositoryService> _logger;
    private readonly AppDbContext _dbContext;
    private readonly IConnectionMultiplexer _redisConnection;
    private readonly ConvertService _convertService;
    private IDatabase _redisDb;

    public RepositoryService(ILogger<RepositoryService> logger, AppDbContext dbContext, IConnectionMultiplexer redisConnection, ConvertService convertService)
    {
        _logger = logger;
        _redisConnection = redisConnection;
        _dbContext = dbContext;
        _redisDb = redisConnection.GetDatabase();
        _convertService = convertService;
    }

    public async Task<Dictionary<string, RateCompact>> GetAllRates()
    {
        var entries = await _redisDb.HashGetAllAsync("rates");
        return entries.ToDictionary(
            kvp => kvp.Name.ToString(),
            kvp => JsonSerializer.Deserialize<RateCompact>(kvp.Value));
    }

    // example pair = "USD-EUR"
    public async Task<RateCompact> GetRate(string pair)
    {
        var redisValue = await _redisDb.HashGetAsync("rates", pair);
        return JsonSerializer.Deserialize<RateCompact>(redisValue);
    }

    public async Task<List<Rate>> GetHistoricalRates(DateTime start, DateTime end, string @base, string quote)
    {
        return await _dbContext.Rates
            .Where(r => r.Base == @base && r.Quote == quote && r.Timestamp >= start && r.Timestamp <= end)
            .GroupBy(r => DateOnly.FromDateTime(r.Timestamp))
            .Select(g => g.OrderByDescending(r => r.Timestamp).First())
            .ToListAsync();
    }

    public async Task<IEnumerable<Integration>> GetIntegrations()
    {
        return await _dbContext.Integrations.ToListAsync();
    }

    public async Task<Integration> GetIntegrationById(string id)
    {
        return await _dbContext.Integrations.FirstAsync(i => i.Name == id);
    }

    public async Task<int> CreateIntegration(Integration integration)
    {
        await _dbContext.Integrations.AddAsync(integration);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> UpdateIntegration(string id, UpdateIntegrationParams uip)
    {
        return await _dbContext.Integrations
        .Where(i => i.Name == id)
        .ExecuteUpdateAsync(setters =>
            setters
            .SetProperty(i => i.Name, i => uip.Name ?? i.Name)
            .SetProperty(i => i.Url, i => uip.Url ?? i.Url)
            .SetProperty(i => i.FreqSeconds, i => uip.FreqSeconds ?? i.FreqSeconds)
            .SetProperty(i => i.Priority, i => uip.Priority ?? i.Priority)
            .SetProperty(i => i.Enabled, i => uip.Enabled ?? i.Enabled)
    );
    }

    public async Task<int> DeleteIntegration(string id)
    {
        return await _dbContext.Integrations
        .Where(i => i.Name == id)
        .ExecuteDeleteAsync();
    }

    public async Task<List<Integration>> GetEnabledIntegrationsSorted()
    {
        //TODO: redis field with expiration
        return await _dbContext.Integrations.Where(i => i.Enabled == true).OrderBy(i => i.Priority).ToListAsync();
    }

    public async Task<int> DisableIntegration(string id)
    {
        return await UpdateIntegration(id, new UpdateIntegrationParams { Enabled = false });
    }

    public async Task<int> EnableIntegration(string id)
    {
        return await UpdateIntegration(id, new UpdateIntegrationParams { Enabled = true });
    }

    public int SaveRatesCombinations(DateTime timestamp, List<Rate> ratesUsd)
    {
        //USD-USD
        _dbContext.Rates.Add(new Rate(
                    Timestamp: timestamp,
                    Base: "USD",
                    Quote: "USD",
                    Value: 1.0M));
        //add USD base and inverse
        foreach (Rate r in ratesUsd)
        {
            _dbContext.Rates.Add(r);
            _dbContext.Rates.Add(new Rate(
                        Timestamp: timestamp,
                        Base: r.Quote,
                        Quote: r.Base,
                        Value: 1 / r.Value));
        }

        // cartesian
        foreach (Rate a in ratesUsd)
        {
            foreach (Rate b in ratesUsd)
            {
                _dbContext.Rates.Add(new Rate(
                            Timestamp: timestamp,
                            Base: a.Quote,
                            Quote: b.Quote,
                            Value: _convertService.CalculateConversionRate(a.Value, b.Value)));
            }
        }

        return _dbContext.SaveChanges();
    }

    public async Task RefreshCache()
    {
        var latestRates = await _dbContext.Rates
            .GroupBy(r => new { r.Base, r.Quote, r.Timestamp })
            .Select(g => g.OrderBy(r => r.Timestamp).First())
            .ToListAsync();

        var entries = latestRates
            .Select(r => new HashEntry(
                        $"{r.Base}-{r.Quote}",
                        JsonSerializer.Serialize(new RateCompact(Rate: Math.Round(r.Value, 6), Ts: r.Timestamp))
            ))
            .ToArray();

        await _redisDb.HashSetAsync("rates", entries);
    }



}
