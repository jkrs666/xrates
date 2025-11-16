using Microsoft.AspNetCore.Mvc;


namespace Xrates.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class HistoricalController : ControllerBase
{

    private readonly ILogger<HistoricalController> _logger;
    private readonly RepositoryService _repositoryService;

    public HistoricalController(ILogger<HistoricalController> logger, RepositoryService repositoryService)
    {
        _logger = logger;
        _repositoryService = repositoryService;
    }

    [HttpGet(Name = "Historical")]
    public async Task<HistoricalResponse> Historical(DateTime start, DateTime end, string to, string from = "USD")
    {
        var rates = await _repositoryService.GetHistoricalRatesBaseUsd(start.ToUniversalTime(), end.ToUniversalTime(), to);
        var historicalRates = rates.Select(r => new HistoricalRate(Date: r.Timestamp.ToString("yyyy-MM-dd"), Value: r.Value)).ToList();
        return new HistoricalResponse(From: from, To: to, Start: start, End: end, Rates: historicalRates);

    }
}

