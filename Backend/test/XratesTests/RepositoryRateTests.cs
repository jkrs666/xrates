using System.Text.Json;
namespace XratesTests;

[TestCaseOrderer(
    ordererTypeName: "XratesTests.AlphabeticalOrderer",
    ordererAssemblyName: "XratesTests")]
public class RepositoryRateTests
{
    DateTime date1 = new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    DateTime date2 = new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc);
    RepositoryService repo = RepositoryServiceFactory.Create();

    [Fact]
    public async Task IT0_SaveRatesCombinationsTest()
    {
        int insertedCount1 = repo.SaveRatesCombinations(date1, new List<Rate>{
                    new Rate(date1, "USD", "EUR", 0.33M),
                });
        int insertedCount2 = repo.SaveRatesCombinations(date2, new List<Rate>{
                    new Rate(date2, "USD", "EUR", 0.86M),
                });

        await repo.RefreshCache();

        Assert.Equal(4, insertedCount1);
        Assert.Equal(4, insertedCount2);

    }

    [Fact]
    public async Task IT1_GetRateRedisTest()
    {
        var rate = await repo.GetRate("EUR-USD");
        Assert.Equal(date2, rate.Ts);
        Assert.Equal(1.162791M, rate.Rate);
    }

    [Fact]
    public async Task IT2_GetAllRatesRedisTest()
    {
        var rates = await repo.GetAllRates();
        var ratesJson = JsonSerializer.Serialize(rates);
        Console.WriteLine(ratesJson);

        Assert.Equal(
        """
                {
                  "EUR-EUR": {
                    "Ts": "2026-01-02T00:00:00Z",
                    "Rate": 1.000000
                  },
                  "EUR-USD": {
                    "Ts": "2026-01-02T00:00:00Z",
                    "Rate": 1.162791
                  },
                  "USD-EUR": {
                    "Ts": "2026-01-02T00:00:00Z",
                    "Rate": 0.86
                  },
                  "USD-USD": {
                    "Ts": "2026-01-02T00:00:00Z",
                    "Rate": 1.0
                  }
                }
                """
            .Replace(" ", "")
            .Replace("\n", ""), ratesJson);
    }


    [Fact]
    public async Task IT3_GetHistoricalRatesTest()
    {
        var rates = await repo.GetHistoricalRates(date1, date2, "USD", "EUR");
        var ratesJson = JsonSerializer.Serialize(rates);
        Console.WriteLine(ratesJson);

        Assert.Equal(
        """
                [
                  {
                    "Timestamp": "2026-01-01T00:00:00Z",
                    "Base": "USD",
                    "Quote": "EUR",
                    "Value": 0.33,
                    "Id": 2
                  },
                  {
                    "Timestamp": "2026-01-02T00:00:00Z",
                    "Base": "USD",
                    "Quote": "EUR",
                    "Value": 0.86,
                    "Id": 6
                  }
                ]
                """.Replace(" ", "")
           .Replace("\n", ""), ratesJson);
    }
}
