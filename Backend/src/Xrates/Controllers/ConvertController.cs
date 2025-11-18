using Microsoft.AspNetCore.Mvc;


namespace Xrates.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ConvertController : ControllerBase
{

    private readonly ILogger<ConvertController> _logger;
    private readonly IRepositoryService _repo;

    public ConvertController(ILogger<ConvertController> logger, IRepositoryService repo)
    {
        _logger = logger;
        _repo = repo;
    }

    [HttpGet(Name = "Convert")]
    public async Task<ConvertResponse> Convert(string from, string to, decimal amount)
    {
        RateCompact rate = await _repo.GetRate($"{from}-{to}");
        return new ConvertResponse(
            Rate: rate.Rate,
            Amount: rate.Rate * amount
        );

    }
}

