using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;

public class RepositoryService
{
    private readonly ILogger<ExternalApiService> _logger;
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly IDistributedCache _cache;

    public RepositoryService(ILogger<ExternalApiService> logger, IDbContextFactory<AppDbContext> dbContextFactory, IDistributedCache cache)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
        _cache = cache;
    }

    public async Task<IEnumerable<Rate>> GetAllRates()
    {
        using var db = _dbContextFactory.CreateDbContext();
        return await db.rates.ToListAsync();
    }

    public async Task<String> GetRate(string quote)
    {
        return await _cache.GetStringAsync(quote) ?? "null";
    }

    public async Task<IEnumerable<Integration>> GetIntegrations()
    {
        using var db = _dbContextFactory.CreateDbContext();
        return await db.integrations.ToListAsync();
    }

    public async Task<Integration> GetIntegrationByName(string id)
    {
        using var db = _dbContextFactory.CreateDbContext();
        return await db.integrations.FirstAsync(i => i.Name == id);
    }

    public async Task<IEnumerable<Rate>> GetLatestRatesFromDb()
    {
        using var db = _dbContextFactory.CreateDbContext();
        return await db.rates
        .GroupBy(r => r.Currency)
        .Select(g => g.OrderByDescending(r => r.Timestamp).FirstOrDefault())
        .ToListAsync();
    }

}
