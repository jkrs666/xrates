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

    public async Task<IEnumerable<Integration>> GetIntegrations()
    {
        return await _dbContext.integrations.ToListAsync();
    }

    public async Task<Integration> GetIntegrationByName(string id)
    {
        return await _dbContext.integrations.FirstAsync(i => i.Name == id);
    }
}
