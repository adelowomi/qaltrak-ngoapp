using NGOAPP.Models.IdentityModels;

namespace NGOAPP;

public interface IUserService
{
    Task<StandardResponse<UserView>> VerifyUserFromMobileCodeAsync(string code);
    Task SendConfirmationLinkAsync(User user, string email, string confirmationLink, bool isMobile);
    Task SendPasswordResetCodeAsync(User user, string email, string resetCode);
    Task SendPasswordResetLinkAsync(User user, string email, string resetLink);
}
