using Moq;
using Xrates.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;


namespace XratesTests;


public class HistoricalControllerTests
{
    private readonly Mock<IRepositoryService> _repo;
    private readonly HistoricalController _controller;
    private DateTime start = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private DateTime end = new DateTime(2026, 1, 3, 0, 0, 0, DateTimeKind.Utc);

    public HistoricalControllerTests()
    {
        var logger = Mock.Of<ILogger<HistoricalController>>();
        _repo = new Mock<IRepositoryService>();
        _controller = new HistoricalController(logger, _repo.Object);
    }

    [Fact]
    public async Task HistoricalTest()
    {
        _repo.Setup(s => s.GetHistoricalRates(start, end, "USD", "EUR")).ReturnsAsync(new List<Rate>(){
                new Rate(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),"USD","EUR", 1.1M),
                new Rate(new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc),"USD","EUR", 1.2M),
                new Rate(new DateTime(2026, 1, 3, 0, 0, 0, DateTimeKind.Utc),"USD","EUR", 1.3M),
                });

        var result = await _controller.Historical(start, end, "EUR");

        Assert.Equivalent(result, new HistoricalResponse(
                    "USD",
                    "EUR",
                    start,
                    end,
                    new List<RateCompact>(){
                    new RateCompact(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), 1.1M),
                    new RateCompact(new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc), 1.2M),
                    new RateCompact(new DateTime(2026, 1, 3, 0, 0, 0, DateTimeKind.Utc), 1.3M),
                    }), strict: true);
    }

    [Fact]
    public async Task HistoricalEmptyTest()
    {
        _repo.Setup(s => s.GetHistoricalRates(start, end, "USD", "EUR")).ReturnsAsync(new List<Rate>());

        var result = await _controller.Historical(start, end, "EUR");

        Assert.Equivalent(result, new HistoricalResponse(
                    "USD",
                    "EUR",
                    start,
                    end,
                    new List<RateCompact>()), strict: true);
    }
}
