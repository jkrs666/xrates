using System.Text.Json;


public class ExternalApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExternalApiService> _logger;

    public ExternalApiService(HttpClient httpClient, ILogger<ExternalApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ExternalApiResponse> Call(Integration integration)
    {
        _logger.LogInformation($"calling {integration.Name}: {integration.Url}");
        var response = await _httpClient.GetAsync(integration.Url);
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        using JsonDocument doc = JsonDocument.Parse(jsonString);
        var ratesElement = doc.RootElement.GetProperty(integration.ratesJsonField);
        var ratesDict = JsonSerializer.Deserialize<Dictionary<string, decimal>>(ratesElement.GetRawText());
        var timestamp = ExtractTimestamp(doc.RootElement.GetProperty(integration.timestampJsonField));
        var ratesUsd = ratesDict.ToList().Select(r => new Rate(
                Timestamp: timestamp,
                Base: integration.BaseCurrency,
                Quote: r.Key,
                Value: r.Value
        )).ToList();
        return new ExternalApiResponse(timestamp, ratesUsd);
    }


    private DateTime ExtractTimestamp(JsonElement dateElement)
    {
        DateTime timestamp = DateTime.UtcNow;
        switch (dateElement.ValueKind)
        {
            case JsonValueKind.String:
                timestamp = new DateTime(DateOnly.Parse(dateElement.GetString()), new TimeOnly(0, 0), DateTimeKind.Utc);
                break;
            case JsonValueKind.Number:
                var t = dateElement.GetInt64();
                if (t < 10000000000)
                {
                    timestamp = DateTimeOffset.FromUnixTimeSeconds(t).UtcDateTime;
                }
                else
                {
                    timestamp = DateTimeOffset.FromUnixTimeMilliseconds(t).UtcDateTime;
                }
                break;
        }
        return timestamp;
    }

}
