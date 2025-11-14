using StackExchange.Redis;

public class ExternalApiService
{
    private readonly HttpClient _httpClient;
    private readonly AppDbContext _dbcontext;
    private readonly ILogger<ExternalApiService> _logger;
    private readonly IDatabase _redis;


    public ExternalApiService(HttpClient httpClient, AppDbContext dbContext, ILogger<ExternalApiService> logger, IDatabase redis)
    {
        _httpClient = httpClient;
        _dbcontext = dbContext;
        _logger = logger;
        _redis = redis;
    }

    public async Task<string> Call(string integrationName)
    {
        try
        {
            var integration = await _dbcontext.integrations.FindAsync(integrationName);
            if (integration == null)
            {
                throw new Exception("not found");
            }

            _logger.LogInformation($"calling {integration.Name}: {integration.Url}");
            var response = await _httpClient.GetAsync(integration.Url);
            response.EnsureSuccessStatusCode();

            if (response.Content == null)
            {
                throw new Exception("null response");
            }

            return await response.Content.ReadAsStringAsync();

        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return e.Message;
        }

    }

}
