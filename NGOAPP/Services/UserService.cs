using AutoMapper;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NGOAPP.Extensions;
using NGOAPP.Models.IdentityModels;
using NGOAPP.Models.UtilityModels;
using NGOAPP.Services;

namespace NGOAPP;

public class UserService : IUserService
{
    private readonly AppSettings _appSettings;
    private readonly IPostmarkHelper _postmarkHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICodeService _codeService;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public UserService(IOptions<AppSettings> appSettings, IPostmarkHelper postmarkHelper, IHttpContextAccessor httpContextAccessor, ICodeService codeService, UserManager<User> userManager, IMapper mapper)
    {
        _appSettings = appSettings.Value;
        _postmarkHelper = postmarkHelper;
        _httpContextAccessor = httpContextAccessor;
        _codeService = codeService;
        _userManager = userManager;
        _mapper = mapper;
    }


    public async Task<StandardResponse<UserView>> VerifyUserFromMobileCodeAsync(string code)
    {
        var codeData = _codeService.GetCode(code);
        if (codeData == null)
            return StandardResponse<UserView>.Error("Invalid code");

        var userId = codeData.Description.Split("||")[0];
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return StandardResponse<UserView>.Error("User not found");

        var originalCode = codeData.Description.Split("||")[1];
        if (originalCode == null)
            return StandardResponse<UserView>.Error("Invalid code");

        var verificationResponse = await _userManager.ConfirmEmailAsync(user, originalCode);
        if (!verificationResponse.Succeeded)
            return StandardResponse<UserView>.Error(verificationResponse.Errors.FirstOrDefault().Description);

        var verifiedUser = _mapper.Map<UserView>(user);
        return StandardResponse<UserView>.Ok(verifiedUser);
    }

    public async Task<StandardResponse<bool>> ResetPasswordAsyncMobile(ResetPasswordModel model)
    {
        var codeData = _codeService.GetCode(model.ResetCode);
        if (codeData == null)
            return StandardResponse<bool>.Error("Invalid code");

        var userEmail = codeData.Description.Split("||")[0];
        var user = await _userManager.FindByIdAsync(userEmail);
        if (user == null)
            return StandardResponse<bool>.Error("User not found");

        var originalCode = codeData.Description.Split("||")[1];
        if (originalCode == null)
            return StandardResponse<bool>.Error("Invalid code");

        var resetPasswordResponse = await _userManager.ResetPasswordAsync(user, originalCode, model.NewPassword);
        if (!resetPasswordResponse.Succeeded)
            return StandardResponse<bool>.Error(message: resetPasswordResponse.Errors.FirstOrDefault().Description);

        return StandardResponse<bool>.Ok(true);

    }

    public async Task<StandardResponse<UserView>> RegisterUserAsync(UserModel model)
    {
        var isMobile = _httpContextAccessor.IsMobileRequest(); 
        var userToCreate = model.Adapt<User>();
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
            return StandardResponse<UserView>.Error("User already exists");

        var result = await _userManager.CreateAsync(userToCreate, model.Password);
        if (!result.Succeeded)
            return StandardResponse<UserView>.Error(result.Errors.FirstOrDefault().Description);

        var user = await _userManager.FindByEmailAsync(model.Email);
        var email = user.Email;
        var confirmationLink = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        await SendConfirmationLinkAsync(user, email, confirmationLink, isMobile);

        var userView = _mapper.Map<UserView>(user);
        return StandardResponse<UserView>.Ok(userView);
    }



    #region SendEmails for account confirmation and password reset
    public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink, bool isMobile)
    {
        var link = $"{_appSettings.FrontEndUrl}/confirm-email?{confirmationLink.Split("?")[1]}";

        var templateModel = new Dictionary<string, string>
        {
            { "link", link },
            { "username", user.FirstName}
        };

        // if this is a mobile request, send a code instead of a link
        if (isMobile)
        {
            // generate confirm email token for user
            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            // combine userId with original code + tyoe
            var description = $"{user.Id}||{confirmationToken}";
            var code = _codeService.GenerateCode(description, 6,numberOnly: true);
            templateModel.Add("code", code);
            await _postmarkHelper.SendTemplatedEmail(EmailTemplates.UserRegistrationTemplateMobile, email, templateModel);
        }

        await  _postmarkHelper.SendTemplatedEmail(EmailTemplates.UserRegistrationTemplate, email, templateModel);
    }


    public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode, bool isMobile)
    {
        var link = $"{_appSettings.FrontEndUrl}/password/reset/{resetCode}";
        // check if this is a mobile request
        var templateModel = new Dictionary<string, string>
        {
            { "code", resetCode },
            { "username", user.FirstName},
            { "link", link }

        };
        // if this is a mobile request, send a code instead of a link
        if (isMobile)
        {
            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            // combine userId with original code + tyoe
            var description = $"{email}||{passwordResetToken}";
            var code = _codeService.GenerateCode(description, 6,numberOnly: true);
            // add the code to the template model
            templateModel["code"] = code;
            await _postmarkHelper.SendTemplatedEmail(EmailTemplates.PasswordResetTemplateMobile, email, templateModel);
        }

        await _postmarkHelper.SendTemplatedEmail(EmailTemplates.PasswordResetTemplate, email, templateModel);
    }


    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        var link = $"{_appSettings.FrontEndUrl}/reset-password?{resetLink.Split("?")[1]}";

        var templateModel = new Dictionary<string, string>
        {
            { "link", link },
            { "username", user.FirstName}
        };

        return _postmarkHelper.SendTemplatedEmail(EmailTemplates.PasswordResetTemplate, email, templateModel);
    }

    #endregion
}
