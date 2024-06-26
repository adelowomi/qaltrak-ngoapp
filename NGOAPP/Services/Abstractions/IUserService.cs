﻿using NGOAPP.Models.IdentityModels;

namespace NGOAPP;

public interface IUserService
{
    Task<StandardResponse<UserView>> VerifyUserFromMobileCodeAsync(string code);
    Task<StandardResponse<bool>> ResetPasswordAsyncMobile(ResetPasswordModel model);
    Task SendConfirmationLinkAsync(User user, string email, string confirmationLink, bool isMobile);
    Task SendPasswordResetCodeAsync(User user, string email, string resetCode, bool isMobile);
    Task SendPasswordResetLinkAsync(User user, string email, string resetLink);
    Task<StandardResponse<UserView>> RegisterUserAsync(UserModel model);
    Task<StandardResponse<UserView>> GetUserProfile();
    Task<StandardResponse<UserView>> UpdateUser(UserProfile model);
    Task AddUserToPlatformAdminROle(string UserId);
}
