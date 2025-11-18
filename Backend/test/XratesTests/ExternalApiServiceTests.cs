using Moq;
using Xrates.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;


namespace XratesTests;


public class ExternalApiServiceTests
{
    private readonly Mock<IRepositoryService> _repo;
    private readonly ExternalApiService _service;

    public ExternalApiServiceTests()
    {
        var logger = Mock.Of<ILogger<ExternalApiService>>();
        var client = new HttpClient();
        _service = new ExternalApiService(client, logger);
    }

    [Fact]
    public async Task ExternalApiServiceTest()
    {

        var frankfurter = new Integration("frankfurter", "https://api.frankfurter.dev/v1/latest?base=USD", 10, 1, true, "USD", "date", "rates");
        var response = await _service.Call(frankfurter);
        Assert.NotNull(response);
        Assert.NotEmpty(response.Timestamp.ToString());
        Assert.NotEmpty(response.Rates);
    }

}
