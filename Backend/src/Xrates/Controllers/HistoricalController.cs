using Microsoft.AspNetCore.Mvc;


namespace Xrates.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class HistoricalController : ControllerBase
{

    private readonly ILogger<HistoricalController> _logger;
    private readonly IRepositoryService _repo;

    public HistoricalController(ILogger<HistoricalController> logger, IRepositoryService repo)
    {
        _logger = logger;
        _repo = repo;
    }

    [HttpGet(Name = "Historical")]
    public async Task<HistoricalResponse> Historical(DateTime start, DateTime end, string to, string from = "USD")
    {
        var rates = await _repo.GetHistoricalRates(start.ToUniversalTime(), end.ToUniversalTime(), from, to);
        var historicalRates = rates.Select(r => new RateCompact(r.Timestamp, r.Value)).ToList();
        return new HistoricalResponse(From: from, To: to, Start: start, End: end, Rates: historicalRates);
    }
}

