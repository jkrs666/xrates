namespace XratesTests;

[TestCaseOrderer(
    ordererTypeName: "XratesTests.AlphabeticalOrderer",
    ordererAssemblyName: "XratesTests")]
public class RepositoryIntegrationsTests
{

    Integration i1 = new Integration("errorTest", "invalidUrl", 10, 0, true, "USD", "date", "rates");
    Integration i2 = new Integration("example", "https://example.com", 10, 0, true, "USD", "timestamp", "rates");
    Integration i3 = new Integration("frankfurter", "https://api.frankfurter.dev/v1/latest?base=USD", 10, 1, true, "USD", "date", "rates");
    Integration testIntegration = new Integration("testIntegration", "https://example.com", 10, 1, true, "USD", "timestamp", "rates");
    RepositoryService repo = RepositoryServiceFactory.Create();

    [Fact]
    public async Task IT0_CreateIntegration()
    {
        int inserted = await repo.CreateIntegration(testIntegration);

        Assert.Equal(1, inserted);
    }

    [Fact]
    public async Task IT1_GetIntegration()
    {
        Integration insertedIntegration = await repo.GetIntegrationById("testIntegration");

        Assert.Equal(testIntegration, insertedIntegration);
    }

    [Fact]
    public async Task IT2_GetAllIntegrations()
    {
        var allIntegrations = await repo.GetIntegrations();

        Assert.Equal(new List<Integration> { i1, i2, i3, testIntegration }, allIntegrations.ToList());
    }

    [Fact]
    public async Task IT3_DisableIntegration()
    {
        Integration expectedIntegration = new Integration("testIntegration", "https://example.com", 10, 1, false, "USD", "timestamp", "rates");

        int updatedCount = await repo.DisableIntegration("testIntegration");
        Integration updatedIntegration = await repo.GetIntegrationById("testIntegration");

        Assert.Equal(1, updatedCount);
        Assert.Equal(expectedIntegration, updatedIntegration);
    }

    [Fact]
    public async Task IT4_GetEnabledIntegrationsSorted()
    {
        var allIntegrations = await repo.GetEnabledIntegrationsSorted();

        Assert.Equal(new List<Integration> { i1, i2, i3 }, allIntegrations.ToList());
    }

    [Fact]
    public async Task IT5_EnableIntegration()
    {
        Integration expectedIntegration = new Integration("testIntegration", "https://example.com", 10, 1, true, "USD", "timestamp", "rates");

        int updatedCount = await repo.EnableIntegration("testIntegration");
        Integration updatedIntegration = await repo.GetIntegrationById("testIntegration");

        Assert.Equal(1, updatedCount);
        Assert.Equal(expectedIntegration, updatedIntegration);
    }

    [Fact]
    public async Task IT6_UpdateIntegration()
    {
        Integration expectedIntegration = new Integration("testIntegration", "test.com", 10, 1000, true, "USD", "timestamp", "rates");

        int updatedCount = await repo.UpdateIntegration("testIntegration", new UpdateIntegrationParams { Url = "test.com", Priority = 1000 });
        Integration updatedIntegration = await repo.GetIntegrationById("testIntegration");

        Assert.Equal(1, updatedCount);
        Assert.Equal(expectedIntegration, updatedIntegration);
    }

    [Fact]
    public async Task IT7_DeleteIntegration()
    {
        int deleteCount = await repo.DeleteIntegration("testIntegration");
        Assert.Equal(1, deleteCount);
    }


}
