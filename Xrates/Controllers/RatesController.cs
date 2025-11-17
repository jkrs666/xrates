using Microsoft.AspNetCore.Mvc;


namespace Xrates.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class RatesController : ControllerBase
{

    private readonly ILogger<RatesController> _logger;
    private readonly RepositoryService _repositoryService;

    public RatesController(ILogger<RatesController> logger, RepositoryService repositoryService)
    {
        _logger = logger;
        _repositoryService = repositoryService;
    }

    [HttpGet(Name = "GetAllRates")]
    public async Task<Dictionary<string, RateCompact>> GetAllRates()
    {
        return await _repositoryService.GetAllRates();
    }

    [HttpGet("{id}", Name = "GetRate")]
    public async Task<String> GetRate(string id)
    {
        return await _repositoryService.GetRate(id) ?? "";
    }
}
