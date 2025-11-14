using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;

public class RepositoryService
{
    private readonly ILogger<ExternalApiService> _logger;
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly IDistributedCache _cache;
    private AppDbContext _dbContext;

    public RepositoryService(ILogger<ExternalApiService> logger, IDbContextFactory<AppDbContext> dbContextFactory, IDistributedCache cache)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
        _cache = cache;
        _dbContext = dbContextFactory.CreateDbContext();
    }

    public async Task<IEnumerable<Rate>> GetAllRates()
    {
        return await _dbContext.rates.ToListAsync();
    }

    public async Task<String> GetRate(string quote)
    {
        return await _cache.GetStringAsync(quote) ?? "null";
    }

    public async Task<IEnumerable<Integration>> GetIntegrations()
    {
        return await _dbContext.integrations.ToListAsync();
    }

    public async Task<Integration> GetIntegrationByName(string id)
    {
        return await _dbContext.integrations.FirstAsync(i => i.Name == id);
    }
}
