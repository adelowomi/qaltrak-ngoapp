using Microsoft.AspNetCore.Mvc;

namespace NGOAPPTESTS;

public class UtilitiesControllerTests : BaseIntegrationTest
{
    private readonly UtilityController _controller;
    public UtilitiesControllerTests(TestsWebAppFactory factory) : base(factory)
    {
        _controller = new UtilityController(_utilityService);
    }
        // [Fact]
        // public async Task GetLocationTypes_ReturnsOkResultWithLocationTypes()
        // {
        //     // Arrange

        //     // Act
        //     var result = await _controller.GetLocationTypes();

        //     // Assert
        //     var okResult = Assert.IsType<OkObjectResult>(result.Result);
        //     var response = Assert.IsType<StandardResponse<List<BaseViewModelI>>>(okResult.Value);
        //     Assert.NotNull(response.Data);
        // }

        // Write similar tests for other endpoints...

        // [Fact]
        // public async Task GetTicketTypes_ReturnsOkResultWithTicketTypes()
        // {
        //     // Arrange
        //     // ...

        //     // Act
        //     // ...

        //     // Assert
        //     // ...
        // }

        // [Fact]
        // public async Task GetEventTypes_ReturnsOkResultWithEventTypes()
        // {
        //     // Arrange
        //     // ...

        //     // Act
        //     // ...

        //     // Assert
        //     // ...
        // }

        // [Fact]
        // public async Task GetEventCategories_ReturnsOkResultWithEventCategories()
        // {
        //     // Arrange
        //     // ...

        //     // Act
        //     // ...

        //     // Assert
        //     // ...
        // }

        // [Fact]
        // public async Task GetEventSubCategories_ReturnsOkResultWithEventSubCategories()
        // {
        //     // Arrange
        //     // ...

        //     // Act
        //     // ...

        //     // Assert
        //     // ...
        // }
}
