using System.Text.Json;
namespace XratesTests;

[Collection("Sequential")]
public class RepositoryTests
{
    [Fact]
    public async Task SaveRatesCombinationsTest()
    {

        var repo = RepositoryServiceFactory.Create();
        var now = new DateTime(1970, 1, 1, 1, 1, 1, 1);

        repo.SaveRatesCombinations(now, new List<Rate>{
            new Rate(now, "USD", "EUR", 0.86M),
        });

        await repo.RefreshCache();
        var rates = await repo.GetAllRates();
        var ratesJson = JsonSerializer.Serialize(rates);
        RedisConnectionFactory.ClearCache();

        Assert.Equal(4, rates.Count());
        Assert.Equal(
        """
        {
          "USD-USD": {
            "Ts": "1970-01-01T01:01:01.001",
            "Rate": 1.0
          },
          "USD-EUR": {
            "Ts": "1970-01-01T01:01:01.001",
            "Rate": 0.86
          },
          "EUR-USD": {
            "Ts": "1970-01-01T01:01:01.001",
            "Rate": 1.162791
          },
          "EUR-EUR": {
            "Ts": "1970-01-01T01:01:01.001",
            "Rate": 1.000000
          }
        }
        """.Replace(" ", "")
           .Replace("\n", ""), ratesJson);
    }
}
