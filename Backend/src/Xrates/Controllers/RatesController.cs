using Microsoft.AspNetCore.Mvc;


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

    [HttpGet("{id}", Name = "GetRate")]
    public async Task<RateCompact> GetRate(string id)
    {
        return await _repo.GetRate(id);
    }
}
