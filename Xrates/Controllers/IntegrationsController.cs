using Microsoft.AspNetCore.Mvc;

namespace Xrates.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class IntegrationsController : ControllerBase
{

    private readonly ILogger<IntegrationsController> _logger;
    private readonly ExternalApiService _externalApiService;
    private readonly RepositoryService _repositoryService;

    public IntegrationsController(ILogger<IntegrationsController> logger, ExternalApiService externalApiService, RepositoryService repositoryService)
    {
        _logger = logger;
        _externalApiService = externalApiService;
        _repositoryService = repositoryService;
    }

    [HttpGet(Name = "GetIntegrations")]
    public async Task<IEnumerable<Integration>> Get()
    {
        return await _repositoryService.GetIntegrations();
    }

    [HttpGet("{id}", Name = "GetIntegrationByName")]
    public async Task<Integration> GetBy(string id)
    {
        return await _repositoryService.GetIntegrationByName(id);
    }

    [HttpGet("call/{id}", Name = "CallIntegrationByName")]
    public async Task<string> CallBy(string id)
    {
        return await _externalApiService.Call(id);
    }
}
