using NGOAPP.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NGOAPP.Extensions;
using NGOAPP.Models.UtilityModels;
using Hangfire;

namespace NGOAPP.Services;

public class IdentityEmailSender : IEmailSender<User>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityEmailSender(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        // _userService = userService;
    }

    public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        bool isMobile = _httpContextAccessor.IsMobileRequest();
        //add a background job to trigger the method in userService
        BackgroundJob.Enqueue<IUserService>(x => x.SendConfirmationLinkAsync(user, email, confirmationLink, isMobile));
        return Task.CompletedTask;
    }

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        bool isMobile = _httpContextAccessor.IsMobileRequest();
        BackgroundJob.Enqueue<IUserService>(x => x.SendPasswordResetCodeAsync(user, email, resetCode, isMobile));
        return Task.CompletedTask;
    }

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        BackgroundJob.Enqueue<IUserService>(x => x.SendPasswordResetLinkAsync(user, email, resetLink));
        return Task.CompletedTask;
    }
}
