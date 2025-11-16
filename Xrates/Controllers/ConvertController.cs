using Microsoft.AspNetCore.Mvc;


namespace Xrates.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ConvertController : ControllerBase
{

    private readonly ILogger<ConvertController> _logger;
    private readonly RepositoryService _repositoryService;

    public ConvertController(ILogger<ConvertController> logger, RepositoryService repositoryService)
    {
        _logger = logger;
        _repositoryService = repositoryService;
    }

    [HttpGet(Name = "Convert")]
    public async Task<ConvertResponse> Convert(string from, string to, decimal amount)
    {
        string fromRate = await _repositoryService.GetRate(from) ?? "0";
        string toRate = await _repositoryService.GetRate(to) ?? "0";
        decimal a;
        decimal b;
        decimal.TryParse(fromRate, out a);
        decimal.TryParse(toRate, out b);
        decimal rate = (1 / a) * b;
        return new ConvertResponse(
            Rate: rate,
            Amount: rate * amount
        );

    }
}

