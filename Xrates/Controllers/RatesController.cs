using Microsoft.AspNetCore.Mvc;

namespace Xrates.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class RatesController : ControllerBase
{

    private readonly ILogger<WeatherForecastController> _logger;

    public RatesController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetRates")]
    public IEnumerable<Rate> Get()
    {
        return [new Rate { Name = "test" }];
    }
}
