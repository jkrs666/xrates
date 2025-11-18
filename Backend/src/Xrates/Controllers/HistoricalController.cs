using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;


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
    public async Task<HistoricalResponse> Historical(
        [DefaultValue("2025-1-1")] DateTime start,
        [DefaultValue("2026-1-1")] DateTime end,
        [DefaultValue("EUR")] string to,
            string from = "USD")
    {
        var rates = await _repo.GetHistoricalRates(start.ToUniversalTime(), end.ToUniversalTime(), from, to);
        var historicalRates = rates.Select(r => new RateCompact(r.Timestamp, r.Value)).ToList();
        return new HistoricalResponse(From: from, To: to, Start: start, End: end, Rates: historicalRates);
    }
}

