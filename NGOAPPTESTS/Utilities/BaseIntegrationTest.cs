using Microsoft.Extensions.DependencyInjection;

namespace NGOAPPTESTS;

public class BaseIntegrationTest : IClassFixture<TestsWebAppFactory>
{
    private readonly IServiceScope _scope;
    public readonly IUtilityService _utilityService;
    public BaseIntegrationTest(TestsWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _utilityService = _scope.ServiceProvider.GetRequiredService<IUtilityService>();
    }
}
