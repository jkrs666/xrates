using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    [HttpGet(Name = "ListIntegrations")]
    public async Task<IEnumerable<Integration>> GetAll()
    {
        return await _repositoryService.GetIntegrations();
    }

    [HttpPost(Name = "CreateIntegration")]
    public async Task<IActionResult> Create([FromBody] Integration integration)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int count;
        try
        {
            count = await _repositoryService.CreateIntegration(integration);
        }
        catch (Exception e)
        {
            return new ObjectResult(new { error = e.Message })
            {
                StatusCode = 503
            };
        }

        if (count == 0)
        {
            return BadRequest(new { error = "Nothing inserted" });
        }

        return Ok(new { message = "Integration created", integration = integration });
    }

    [HttpGet("{id}", Name = "GetIntegration")]
    public async Task<Integration> Get(string id)
    {
        return await _repositoryService.GetIntegrationById(id);
    }

    [HttpPut("{id}", Name = "UpdateIntegration")]
    public async Task<IActionResult> Update(string id, [FromBody] Integration integration)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int count;
        try
        {
            count = await _repositoryService.UpdateIntegration(id, integration);
        }
        catch (Exception e)
        {
            return new ObjectResult(new { error = e.Message })
            {
                StatusCode = 503
            };
        }

        if (count == 0)
        {
            return BadRequest(new { error = "Nothing updated" });
        }

        return Ok(new
        {
            message = "Integration updated",
            integration = integration
        });
    }

    [HttpDelete("{id}", Name = "DeleteIntegration")]
    public async Task<IActionResult> Delete(string id)
    {
        int count;

        try
        {
            count = await _repositoryService.DeleteIntegration(id);
        }
        catch (Exception e)
        {
            return new ObjectResult(new { error = e.Message })
            {
                StatusCode = 503
            };
        }
        if (count == 0)
        {
            return BadRequest(new { error = "Nothing deleted" });
        }
        return Ok(new { message = $"{id} integration deleted" });
    }


}
