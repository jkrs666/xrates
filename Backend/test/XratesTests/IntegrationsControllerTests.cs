using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xrates.Controllers;

namespace Xrates.Tests.Controllers;

public class IntegrationsControllerTests
{
    private readonly Mock<ILogger<IntegrationsController>> _mockLogger;
    private readonly Mock<IRepositoryService> _mockRepo;
    private readonly IntegrationsController _controller;

    public IntegrationsControllerTests()
    {
        _mockLogger = new Mock<ILogger<IntegrationsController>>();
        _mockRepo = new Mock<IRepositoryService>();
        _controller = new IntegrationsController(_mockLogger.Object, _mockRepo.Object);
    }


    [Fact]
    public async Task GetAll_ReturnsListOfIntegrations()
    {
        var expectedIntegrations = new List<Integration>
        {
            new Integration("CoinGecko", "https://api.coingecko.com", 300, 1, true, "USD", "timestamp", "rates"),
            new Integration("ExchangeRateAPI", "https://api.exchangerate.com", 600, 2, true, "EUR", "time", "data")
        };
        _mockRepo.Setup(r => r.GetIntegrations()).ReturnsAsync(expectedIntegrations);

        var result = await _controller.GetAll();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, i => i.Name == "CoinGecko");
        _mockRepo.Verify(r => r.GetIntegrations(), Times.Once);
    }

    [Fact]
    public async Task GetAll_ReturnsEmptyList_WhenNoIntegrations()
    {
        _mockRepo.Setup(r => r.GetIntegrations()).ReturnsAsync(new List<Integration>());

        var result = await _controller.GetAll();

        Assert.NotNull(result);
        Assert.Empty(result);
    }



    [Fact]
    public async Task Create_ReturnsOk_WhenIntegrationCreatedSuccessfully()
    {
        var integration = new Integration("TestAPI", "https://api.test.com", 300, 1, true, "USD", "timestamp", "rates");
        _mockRepo.Setup(r => r.CreateIntegration(integration)).ReturnsAsync(1);

        var result = await _controller.Create(integration);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value;
        Assert.NotNull(response);

        var responseType = response.GetType();
        var messageProperty = responseType.GetProperty("message");
        var integrationProperty = responseType.GetProperty("integration");

        Assert.Equal("Integration created", messageProperty?.GetValue(response));
        Assert.Equal(integration, integrationProperty?.GetValue(response));

        _mockRepo.Verify(r => r.CreateIntegration(integration), Times.Once);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        var integration = new Integration("Test", "url", 300, 1, true, "USD", "ts", "rates");
        _controller.ModelState.AddModelError("Name", "Required");

        var result = await _controller.Create(integration);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<SerializableError>(badRequestResult.Value);
        _mockRepo.Verify(r => r.CreateIntegration(It.IsAny<Integration>()), Times.Never);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenNothingInserted()
    {
        var integration = new Integration("Test", "url", 300, 1, true, "USD", "ts", "rates");
        _mockRepo.Setup(r => r.CreateIntegration(integration)).ReturnsAsync(0);

        var result = await _controller.Create(integration);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var value = badRequestResult.Value;

        var errorProperty = value?.GetType().GetProperty("error");
        Assert.Equal("Nothing inserted", errorProperty?.GetValue(value));
    }

    [Fact]
    public async Task Create_Returns503_WhenExceptionThrown()
    {
        var integration = new Integration("Test", "url", 300, 1, true, "USD", "ts", "rates");
        var exceptionMessage = "Database connection failed";
        _mockRepo.Setup(r => r.CreateIntegration(integration))
            .ThrowsAsync(new Exception(exceptionMessage));

        var result = await _controller.Create(integration);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(503, objectResult.StatusCode);

        var value = objectResult.Value;
        var errorProperty = value?.GetType().GetProperty("error");
        Assert.Equal(exceptionMessage, errorProperty?.GetValue(value));
    }

    [Fact]
    public async Task Get_ReturnsIntegration_WhenIntegrationExists()
    {
        var integrationId = "test-api-1";
        var expectedIntegration = new Integration(
            "TestAPI",
            "https://api.test.com",
            300,
            1,
            true,
            "USD",
            "timestamp",
            "rates"
        );
        _mockRepo.Setup(r => r.GetIntegrationById(integrationId)).ReturnsAsync(expectedIntegration);

        var result = await _controller.Get(integrationId);

        Assert.NotNull(result);
        Assert.Equal("TestAPI", result.Name);
        Assert.Equal("https://api.test.com", result.Url);
        Assert.Equal(300, result.FreqSeconds);
        _mockRepo.Verify(r => r.GetIntegrationById(integrationId), Times.Once);
    }

    [Fact]
    public async Task Get_ReturnsNull_WhenIntegrationDoesNotExist()
    {
        var integrationId = "non-existent-id";
        _mockRepo.Setup(r => r.GetIntegrationById(integrationId)).ReturnsAsync((Integration)null);

        var result = await _controller.Get(integrationId);

        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ReturnsOk_WhenIntegrationUpdatedSuccessfully()
    {
        var integrationId = "test-api-1";
        var updateParams = new UpdateIntegrationParams { Enabled = false, Priority = 5 };
        _mockRepo.Setup(r => r.UpdateIntegration(integrationId, updateParams)).ReturnsAsync(1);

        var result = await _controller.Update(integrationId, updateParams);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var value = okResult.Value;

        var messageProperty = value?.GetType().GetProperty("message");
        Assert.Equal($"{integrationId} integration updated", messageProperty?.GetValue(value));

        _mockRepo.Verify(r => r.UpdateIntegration(integrationId, updateParams), Times.Once);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        var integrationId = "test-api-1";
        var updateParams = new UpdateIntegrationParams();
        _controller.ModelState.AddModelError("Enabled", "Required");

        var result = await _controller.Update(integrationId, updateParams);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<SerializableError>(badRequestResult.Value);
        _mockRepo.Verify(r => r.UpdateIntegration(It.IsAny<string>(), It.IsAny<UpdateIntegrationParams>()), Times.Never);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenNothingUpdated()
    {
        var integrationId = "test-api-1";
        var updateParams = new UpdateIntegrationParams { Enabled = false };
        _mockRepo.Setup(r => r.UpdateIntegration(integrationId, updateParams)).ReturnsAsync(0);

        var result = await _controller.Update(integrationId, updateParams);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var value = badRequestResult.Value;

        var errorProperty = value?.GetType().GetProperty("error");
        Assert.Equal("Nothing updated", errorProperty?.GetValue(value));
    }

    [Fact]
    public async Task Update_Returns503_WhenExceptionThrown()
    {
        var integrationId = "test-api-1";
        var updateParams = new UpdateIntegrationParams { Enabled = false };
        var exceptionMessage = "Database update failed";
        _mockRepo.Setup(r => r.UpdateIntegration(integrationId, updateParams))
            .ThrowsAsync(new Exception(exceptionMessage));

        var result = await _controller.Update(integrationId, updateParams);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(503, objectResult.StatusCode);

        var value = objectResult.Value;
        var errorProperty = value?.GetType().GetProperty("error");
        Assert.Equal(exceptionMessage, errorProperty?.GetValue(value));
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenIntegrationDeletedSuccessfully()
    {
        var integrationId = "test-api-1";
        _mockRepo.Setup(r => r.DeleteIntegration(integrationId)).ReturnsAsync(1);

        var result = await _controller.Delete(integrationId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var value = okResult.Value;

        var messageProperty = value?.GetType().GetProperty("message");
        Assert.Equal($"{integrationId} integration deleted", messageProperty?.GetValue(value));

        _mockRepo.Verify(r => r.DeleteIntegration(integrationId), Times.Once);
    }

    [Fact]
    public async Task Delete_ReturnsBadRequest_WhenNothingDeleted()
    {
        var integrationId = "non-existent-id";
        _mockRepo.Setup(r => r.DeleteIntegration(integrationId)).ReturnsAsync(0);

        var result = await _controller.Delete(integrationId);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var value = badRequestResult.Value;

        var errorProperty = value?.GetType().GetProperty("error");
        Assert.Equal("Nothing deleted", errorProperty?.GetValue(value));
    }

    [Fact]
    public async Task Delete_Returns503_WhenExceptionThrown()
    {
        var integrationId = "test-api-1";
        var exceptionMessage = "Database deletion failed";
        _mockRepo.Setup(r => r.DeleteIntegration(integrationId))
            .ThrowsAsync(new Exception(exceptionMessage));

        var result = await _controller.Delete(integrationId);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(503, objectResult.StatusCode);

        var value = objectResult.Value;
        var errorProperty = value?.GetType().GetProperty("error");
        Assert.Equal(exceptionMessage, errorProperty?.GetValue(value));
    }

}
