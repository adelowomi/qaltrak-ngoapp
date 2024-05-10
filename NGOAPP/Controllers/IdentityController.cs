using Microsoft.AspNetCore.Mvc;

namespace NGOAPP;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : StandardControllerBase
{
    private readonly IUserService _userService;

    public IdentityController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("mobile/confirm-email/{code}", Name = nameof(ConfirmEmail))]
    [ProducesResponseType(typeof(StandardResponse<UserView>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StandardResponse<UserView>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmEmail(string code)
    {
        var response = await _userService.VerifyUserFromMobileCodeAsync(code);
        return Ok(response);
    }
}
