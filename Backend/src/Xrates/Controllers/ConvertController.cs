using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;


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
    public async Task<IActionResult> Convert(
            [DefaultValue("USD")] string from,
            [DefaultValue("EUR")] string to,
            [DefaultValue("10.0")] decimal amount
    )
    {
        var rateKey = $"{from}-{to}";
        RateCompact rate;
        try
        {
            rate = await _repo.GetRate(rateKey);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return NotFound(new { message = $"Rate {rateKey} not found" });
        }
        return Ok(new ConvertResponse(
            Rate: rate.Rate,
            Amount: rate.Rate * amount
        ));
    }
}

