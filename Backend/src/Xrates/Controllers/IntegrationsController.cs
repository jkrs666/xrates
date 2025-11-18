using Microsoft.AspNetCore.Mvc;

namespace Xrates.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class IntegrationsController : ControllerBase
{

    private readonly ILogger<IntegrationsController> _logger;
    private readonly IRepositoryService _repo;

    public IntegrationsController(ILogger<IntegrationsController> logger, IRepositoryService repo)
    {
        _logger = logger;
        _repo = repo;
    }

    [HttpGet(Name = "ListIntegrations")]
    public async Task<IEnumerable<Integration>> GetAll()
    {
        return await _repo.GetIntegrations();
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
            count = await _repo.CreateIntegration(integration);
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
        return await _repo.GetIntegrationById(id);
    }

    [HttpPut("{id}", Name = "UpdateIntegration")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateIntegrationParams uip)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int count;
        try
        {
            count = await _repo.UpdateIntegration(id, uip);
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

        return Ok(new { message = $"{id} integration updated" });
    }

    [HttpDelete("{id}", Name = "DeleteIntegration")]
    public async Task<IActionResult> Delete(string id)
    {
        int count;

        try
        {
            count = await _repo.DeleteIntegration(id);
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
