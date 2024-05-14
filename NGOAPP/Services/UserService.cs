using AutoMapper;
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

        var userId = codeData.Description.Split("|")[0];
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return StandardResponse<UserView>.Error("User not found");

        var originalCode = codeData.Description.Split("|")[2];
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

        var userId = codeData.Description.Split("|")[0];
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return StandardResponse<bool>.Error("User not found");

        var originalCode = codeData.Description.Split("|")[2];
        if (originalCode == null)
            return StandardResponse<bool>.Error("Invalid code");

        var resetPasswordResponse = await _userManager.ResetPasswordAsync(user, originalCode, model.NewPassword);
        if (!resetPasswordResponse.Succeeded)
            return StandardResponse<bool>.Error(message: resetPasswordResponse.Errors.FirstOrDefault().Description);

        return StandardResponse<bool>.Ok(true);

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
            var description = $"{user.Id}|{CodeType.EmailConfirmation}|{confirmationToken}";
            var code = _codeService.GenerateCode(description, 6,numberOnly: true);
            templateModel.Add("code", code);
            await _postmarkHelper.SendTemplatedEmail(EmailTemplates.UserRegistrationTemplateMobile, email, templateModel);
        }

        await  _postmarkHelper.SendTemplatedEmail(EmailTemplates.UserRegistrationTemplate, email, templateModel);
    }


    public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        // check if this is a mobile request
        var isMobile = _httpContextAccessor.IsMobileRequest();

        var templateModel = new Dictionary<string, string>
        {
            { "code", resetCode },
            { "username", user.FirstName}
        };

        // if this is a mobile request, send a code instead of a link
        if (isMobile)
        {
            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            // combine userId with original code + tyoe
            var description = $"{user.Id}|{CodeType.PasswordReset}|{passwordResetToken}";
            var code = _codeService.GenerateCode(description, 6,numberOnly: true);
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
