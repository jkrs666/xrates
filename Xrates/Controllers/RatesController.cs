using Microsoft.AspNetCore.Mvc;

namespace Xrates.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class RatesController : ControllerBase
{

    private readonly ILogger<RatesController> _logger;
    private readonly ApplicationDbContext _dbcontext;

    public RatesController(ILogger<RatesController> logger, ApplicationDbContext dbcontext)
    {
        _logger = logger;
        _dbcontext = dbcontext;
    }

    [HttpGet(Name = "GetRates")]
    public IEnumerable<Rate> Get()
    {
        return _dbcontext.Rates.ToList();
    }
}
