using Moq;
using Xrates.Controllers;
using Microsoft.Extensions.Logging;


namespace XratesTests;


public class ConvertControllerTests
{
    private readonly Mock<IRepositoryService> _repo;
    private readonly ConvertController _controller;

    public ConvertControllerTests()
    {
        var logger = Mock.Of<ILogger<ConvertController>>();
        _repo = new Mock<IRepositoryService>();
        _controller = new ConvertController(logger, _repo.Object);
    }

    [Fact]
    public async Task ConvertTest()
    {
        var now = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var rate = new RateCompact(now, 2.0M);
        _repo.Setup(s => s.GetRate("USD-EUR")).ReturnsAsync(rate);

        var result = await _controller.Convert("USD", "EUR", 10);

        Assert.Equal(new ConvertResponse(Rate: rate.Rate, Amount: 20), result);
    }
}
