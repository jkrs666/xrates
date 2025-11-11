
public class ExternalApiService
{
    private readonly HttpClient _httpClient;
    private readonly ApplicationDbContext _dbcontext;
    private readonly ILogger<ExternalApiService> _logger;


    public ExternalApiService(HttpClient httpClient, ApplicationDbContext dbContext, ILogger<ExternalApiService> logger)
    {
        _httpClient = httpClient;
        _dbcontext = dbContext;
        _logger = logger;
    }

    public async Task<string> Call(string integrationName)
    {
        try
        {
            var integration = await _dbcontext.Integrations.FindAsync(integrationName);
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
