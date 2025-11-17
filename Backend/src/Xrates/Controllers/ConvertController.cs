using Microsoft.AspNetCore.Mvc;


namespace Xrates.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ConvertController : ControllerBase
{

    private readonly ILogger<ConvertController> _logger;
    private readonly RepositoryService _repositoryService;
    private readonly ConvertService _convertService;

    public ConvertController(ILogger<ConvertController> logger, RepositoryService repositoryService, ConvertService convertService)
    {
        _logger = logger;
        _repositoryService = repositoryService;
        _convertService = convertService;
    }

    [HttpGet(Name = "Convert")]
    public async Task<ConvertResponse> Convert(string from, string to, decimal amount)
    {
        string fromRate = await _repositoryService.GetRate(from);
        string toRate = await _repositoryService.GetRate(to);
        decimal a;
        decimal b;
        decimal.TryParse(fromRate, out a);
        decimal.TryParse(toRate, out b);
        decimal rate = _convertService.CalculateConversionRate(a, b);
        return new ConvertResponse(
            Rate: rate,
            Amount: _convertService.Convert(rate, amount)
        );

    }
}

