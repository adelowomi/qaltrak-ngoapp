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
        return Result(response);
    }
    
    [HttpPost("mobile/reset-password", Name = nameof(ResetPassword))]
    [ProducesResponseType(typeof(StandardResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StandardResponse<bool>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        var response = await _userService.ResetPasswordAsyncMobile(model);
        return Result(response);
    }

    [HttpPost("register", Name = nameof(RegisterUser))]
    [ProducesResponseType(typeof(StandardResponse<UserView>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StandardResponse<UserView>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUser([FromBody] UserModel model)
    {
        var response = await _userService.RegisterUserAsync(model);
        return Result(response);
    }

    [HttpGet("profile", Name = nameof(GetUserProfile))]
    [ProducesResponseType(typeof(StandardResponse<UserView>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StandardResponse<UserView>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(StandardResponse<UserView>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserProfile()
    {
        var response = await _userService.GetUserProfile();
        return Result(response);
    }

    [HttpPost("profile/update", Name = nameof(UpdateUserProfile))]
    [ProducesResponseType(typeof(StandardResponse<UserView>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StandardResponse<UserView>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(StandardResponse<UserView>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfile model)
    {
        var response = await _userService.UpdateUser(model);
        return Result(response);
    }
}
