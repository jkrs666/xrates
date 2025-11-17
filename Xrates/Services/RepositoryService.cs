using StackExchange.Redis;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

public class RepositoryService
{
    private readonly ILogger<ExternalApiService> _logger;
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly IConnectionMultiplexer _redisConnection;
    private readonly ConvertService _convertService;
    private AppDbContext _dbContext;
    private IDatabase _redisDb;

    public RepositoryService(ILogger<ExternalApiService> logger, IDbContextFactory<AppDbContext> dbContextFactory, IConnectionMultiplexer redisConnection, ConvertService convertService)
    {
        _logger = logger;
        _dbContextFactory = dbContextFactory;
        _redisConnection = redisConnection;
        _dbContext = dbContextFactory.CreateDbContext();
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

    public async Task<String?> GetRate(string quote)
    {
        return await _redisDb.HashGetAsync("rates", quote);
    }

    public async Task<List<Rate>> GetHistoricalRates(DateTime start, DateTime end, string @base, string quote)
    {
        return await _dbContext.Rates
            .Where(r => r.Base == @base && r.Quote == quote && r.Timestamp > start && r.Timestamp < end)
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

    public async Task<int> UpdateIntegration(string id, Integration newIntegration)
    {
        return await _dbContext.Integrations
        .Where(i => i.Name == id)
        .ExecuteUpdateAsync(setters => setters
            .SetProperty(i => i.Name, newIntegration.Name)
            .SetProperty(i => i.Url, newIntegration.Url)
            .SetProperty(i => i.FreqSeconds, newIntegration.FreqSeconds)
    );
    }

    public async Task<int> DeleteIntegration(string id)
    {
        return await _dbContext.Integrations
        .Where(i => i.Name == id)
        .ExecuteDeleteAsync();
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



}
