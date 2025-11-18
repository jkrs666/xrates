public interface IRepositoryService
{
    Task<Dictionary<string, RateCompact>> GetAllRates();
    Task<RateCompact> GetRate(string pair);
    Task<List<Rate>> GetHistoricalRates(DateTime start, DateTime end, string @base, string quote);
    Task<IEnumerable<Integration>> GetIntegrations();
    Task<Integration> GetIntegrationById(string id);
    Task<int> CreateIntegration(Integration integration);
    Task<int> UpdateIntegration(string id, UpdateIntegrationParams uip);
    Task<int> DeleteIntegration(string id);
    Task<List<Integration>> GetEnabledIntegrationsSorted();
    Task<int> DisableIntegration(string id);
    Task<int> EnableIntegration(string id);
    int SaveRatesCombinations(DateTime timestamp, List<Rate> ratesUsd);
    Task RefreshCache();
}
