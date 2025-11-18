using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;


namespace Xrates.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class RatesController : ControllerBase
{

    private readonly ILogger<RatesController> _logger;
    private readonly IRepositoryService _repo;

    public RatesController(ILogger<RatesController> logger, IRepositoryService repo)
    {
        _logger = logger;
        _repo = repo;
    }

    [HttpGet(Name = "GetAllRates")]
    public async Task<Dictionary<string, RateCompact>> GetAllRates()
    {
        return await _repo.GetAllRates();
    }

    [HttpGet("{pair}", Name = "GetRate")]
    public async Task<IActionResult> GetRate([DefaultValue("USD-EUR")] string pair)
    {
        RateCompact rate;
        try
        {
            rate = await _repo.GetRate(pair);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return NotFound(new { message = $"Rate {pair} not found" });
        }
        return Ok(rate);
    }
}
