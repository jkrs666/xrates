using StackExchange.Redis;
using System.Text.Json.Serialization;

public class ExternalApiService
{
    private readonly HttpClient _httpClient;
    private readonly AppDbContext _dbcontext;
    private readonly ILogger<ExternalApiService> _logger;
    private readonly IDatabase _redis;

    public class XApiResponse
    {
        //[JsonPropertyName("amount")]
        //public decimal Amount { get; set; }

        //[JsonPropertyName("base")]
        //public string Base { get; set; }

        //[JsonPropertyName("date")]
        //public string Date { get; set; }

        [JsonPropertyName("rates")]
        required public Dictionary<string, decimal> Rates { get; set; }
    }

    public ExternalApiService(HttpClient httpClient, AppDbContext dbContext, ILogger<ExternalApiService> logger, IDatabase redis)
    {
        _httpClient = httpClient;
        _dbcontext = dbContext;
        _logger = logger;
        _redis = redis;
    }

    public async Task<XApiResponse?> Call(string integrationName)
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

            return await response.Content.ReadFromJsonAsync<XApiResponse>();

        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }

    }

}
