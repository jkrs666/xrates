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
        var ratesTo = await _repositoryService.GetHistoricalRatesBaseUsd(start.ToUniversalTime(), end.ToUniversalTime(), to);
        var ratesFrom = await _repositoryService.GetHistoricalRatesBaseUsd(start.ToUniversalTime(), end.ToUniversalTime(), from);

        List<HistoricalRate> historicalRates = new List<HistoricalRate>();

        for (int i = 0; i < ratesTo.Count(); i++)
        {
            var rate = _convertService.CalculateConversionRate(ratesFrom[i].Value, ratesTo[i].Value);
            _logger.LogInformation(rate.ToString());
            var hr = new HistoricalRate(
                Date: ratesTo[i].Timestamp.ToString("yyyy-MM-dd"),
                Value: rate
                );
            historicalRates.Add(hr);
        }

        return new HistoricalResponse(From: from, To: to, Start: start, End: end, Rates: historicalRates);
    }
}

