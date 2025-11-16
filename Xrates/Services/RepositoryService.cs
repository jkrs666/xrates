using StackExchange.Redis;
using Microsoft.EntityFrameworkCore;

public class RepositoryService
{
    private readonly ILogger<ExternalApiService> _logger;
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly IConnectionMultiplexer _redisConnection;
    private AppDbContext _dbContext;
    private IDatabase _redisDb;

    public RepositoryService(ILogger<ExternalApiService> logger, IDbContextFactory<AppDbContext> dbContextFactory, IConnectionMultiplexer redisConnection)
    {
        _logger = logger;
        _dbContextFactory = dbContextFactory;
        _redisConnection = redisConnection;
        _dbContext = dbContextFactory.CreateDbContext();
        _redisDb = redisConnection.GetDatabase();
    }

    public async Task<Dictionary<string, string>> GetAllRates()
    {
        var entries = await _redisDb.HashGetAllAsync("rates");
        return entries.ToStringDictionary();
    }

    public async Task<String?> GetRate(string quote)
    {
        return await _redisDb.HashGetAsync("rates", quote);
    }

    public async Task<List<Rate>> GetHistoricalRatesBaseUsd(DateTime start, DateTime end, string to)
    {
        return await _dbContext.rates
            .Where(r => r.Currency == to && r.Timestamp > start && r.Timestamp < end)
            .GroupBy(r => DateOnly.FromDateTime(r.Timestamp))
            .Select(g => g.OrderByDescending(r => r.Timestamp).First())
            .ToListAsync();
    }

    public async Task<IEnumerable<Integration>> GetIntegrations()
    {
        return await _dbContext.integrations.ToListAsync();
    }

    public async Task<Integration> GetIntegrationById(string id)
    {
        return await _dbContext.integrations.FirstAsync(i => i.Name == id);
    }

    public async Task<int> CreateIntegration(Integration integration)
    {
        await _dbContext.integrations.AddAsync(integration);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> UpdateIntegration(string id, Integration newIntegration)
    {
        return await _dbContext.integrations
        .Where(i => i.Name == id)
        .ExecuteUpdateAsync(setters => setters
            .SetProperty(i => i.Name, newIntegration.Name)
            .SetProperty(i => i.Url, newIntegration.Url)
    );
    }

    public async Task<int> DeleteIntegration(string id)
    {
        return await _dbContext.integrations
        .Where(i => i.Name == id)
        .ExecuteDeleteAsync();
    }
}
