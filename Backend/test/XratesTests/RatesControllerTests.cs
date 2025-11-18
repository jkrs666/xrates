using Moq;
using Xrates.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;


namespace XratesTests;


public class RatesControllerTests
{
    private readonly Mock<IRepositoryService> _repo;
    private readonly RatesController _controller;
    private DateTime now = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public RatesControllerTests()
    {
        var logger = Mock.Of<ILogger<RatesController>>();
        _repo = new Mock<IRepositoryService>();
        _controller = new RatesController(logger, _repo.Object);
    }

    [Fact]
    public async Task GetRateTest()
    {
        var rate = new RateCompact(now, 2.0M);
        _repo.Setup(s => s.GetRate("USD-EUR")).ReturnsAsync(rate);

        var result = await _controller.GetRate("USD-EUR");

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<RateCompact>(okResult.Value);

        Assert.Equal(response, rate);
    }

    [Fact]
    public async Task GetRate_NotFound()
    {
        _repo.Setup(s => s.GetRate("USD-EUR")).ThrowsAsync(new ArgumentNullException());

        var result = await _controller.GetRate("USD-EUR");

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetAllRatesTest()
    {
        var rates = new Dictionary<string, RateCompact>(){
        {"USD-EUR", new RateCompact(now, 2.0M)},
        {"USD-CHF", new RateCompact(now, 3.0M)},
        {"USD-JPY", new RateCompact(now, 4.0M)},
        };
        _repo.Setup(s => s.GetAllRates()).ReturnsAsync(rates);

        var result = await _controller.GetAllRates();

        Assert.Equivalent(rates, result, strict: true);
    }

    [Fact]
    public async Task GetAllRates_Empty()
    {
        var rates = new Dictionary<string, RateCompact>() { };
        _repo.Setup(s => s.GetAllRates()).ReturnsAsync(rates);

        var result = await _controller.GetAllRates();

        Assert.Empty(result);
    }

}
