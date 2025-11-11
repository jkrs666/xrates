using Microsoft.AspNetCore.Mvc;

namespace Xrates.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class IntegrationsController : ControllerBase
{

    private readonly ILogger<IntegrationsController> _logger;
    private readonly ApplicationDbContext _dbcontext;
    private readonly ExternalApiService _externalApiService;

    public IntegrationsController(ILogger<IntegrationsController> logger, ApplicationDbContext dbcontext, ExternalApiService externalApiService)
    {
        _logger = logger;
        _dbcontext = dbcontext;
        _externalApiService = externalApiService;
    }

    [HttpGet(Name = "GetIntegrations")]
    public IEnumerable<Integration> Get()
    {
        return _dbcontext.Integrations.ToList();
    }

    [HttpGet("{id}", Name = "GetIntegrationByName")]
    public async Task<Integration> GetBy(string id)
    {
        return await _dbcontext.Integrations.FindAsync(id);
    }

    [HttpGet("call/{id}", Name = "CallIntegrationByName")]
    public async Task<string> CallBy(string id)
    {
        return await _externalApiService.Call(id);
    }
}
