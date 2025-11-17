using Microsoft.AspNetCore.Mvc;


namespace Xrates.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class HistoricalController : ControllerBase
{

    private readonly ILogger<HistoricalController> _logger;
    private readonly RepositoryService _repositoryService;
    private readonly ConvertService _convertService;

    public HistoricalController(ILogger<HistoricalController> logger, RepositoryService repositoryService, ConvertService convertService)
    {
        _logger = logger;
        _repositoryService = repositoryService;
        _convertService = convertService;
    }

    [HttpGet(Name = "Historical")]
    public async Task<HistoricalResponse> Historical(DateTime start, DateTime end, string to, string from = "USD")
    {
        var rates = await _repositoryService.GetHistoricalRates(start.ToUniversalTime(), end.ToUniversalTime(), from, to);
        var historicalRates = rates.Select(r => new HistoricalRate(r.Timestamp, r.Value));
        return new HistoricalResponse(From: from, To: to, Start: start, End: end, Rates: historicalRates);
    }
}

