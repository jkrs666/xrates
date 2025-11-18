using Moq;
using Xrates.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;


namespace XratesTests;


public class ConvertControllerTests
{
    private readonly Mock<IRepositoryService> _repo;
    private readonly ConvertController _controller;
    private DateTime now = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public ConvertControllerTests()
    {
        var logger = Mock.Of<ILogger<ConvertController>>();
        _repo = new Mock<IRepositoryService>();
        _controller = new ConvertController(logger, _repo.Object);
    }

    [Fact]
    public async Task ConvertTest()
    {
        var rate = new RateCompact(now, 2.0M);
        _repo.Setup(s => s.GetRate("USD-EUR")).ReturnsAsync(rate);

        var result = await _controller.Convert("USD", "EUR", 10);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ConvertResponse>(okResult.Value);

        Assert.Equal(2.0M, response.Rate);
        Assert.Equal(20M, response.Amount);
    }

    [Fact]
    public async Task ConvertNegativeTest()
    {
        _repo.Setup(s => s.GetRate("USD-EUR")).ThrowsAsync(new ArgumentNullException());

        var result = await _controller.Convert("USD", "EUR", 10);

        Assert.IsType<NotFoundObjectResult>(result);
    }
}
