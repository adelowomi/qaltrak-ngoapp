using NGOAPP.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NGOAPP.Extensions;
using NGOAPP.Models.UtilityModels;

namespace NGOAPP.Services;

public class IdentityEmailSender : IEmailSender<User>
{
    private readonly AppSettings _appSettings;
    private readonly IPostmarkHelper _postmarkHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICodeService _codeService;

    public IdentityEmailSender(IOptions<AppSettings> appSettings, IPostmarkHelper postmarkHelper, IHttpContextAccessor httpContextAccessor, ICodeService codeService)
    {
        _appSettings = appSettings.Value;
        _postmarkHelper = postmarkHelper;
        _httpContextAccessor = httpContextAccessor;
        _codeService = codeService;
    }

    public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        // check if this is a mobile request
        var isMobile = _httpContextAccessor.IsMobileRequest();
        var link = $"{_appSettings.FrontEndUrl}/confirm-email?{confirmationLink.Split("?")[1]}";

        var templateModel = new Dictionary<string, string>
        {
            { "link", link },
            { "username", user.FirstName}
        };

        // if this is a mobile request, send a code instead of a link
        if (isMobile)
        {
            // combine userId with original code + tyoe
            var description = $"{user.Id}|{CodeType.EmailConfirmation}|{confirmationLink.Split("code=")[1]}";
            var code = _codeService.GenerateCode(description, 6,numberOnly: true);
            return _postmarkHelper.SendTemplatedEmail(EmailTemplates.UserRegistrationTemplateMobile, email, templateModel);
        }

        return _postmarkHelper.SendTemplatedEmail(EmailTemplates.UserRegistrationTemplate, email, templateModel);
    }

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        throw new NotImplementedException();
    }

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        throw new NotImplementedException();
    }
}
