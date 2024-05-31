using AutoMapper;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    private readonly RoleManager<Role> _roleManager;

    public UserService(IOptions<AppSettings> appSettings, IPostmarkHelper postmarkHelper, IHttpContextAccessor httpContextAccessor, ICodeService codeService, UserManager<User> userManager, IMapper mapper, RoleManager<Role> roleManager)
    {
        _appSettings = appSettings.Value;
        _postmarkHelper = postmarkHelper;
        _httpContextAccessor = httpContextAccessor;
        _codeService = codeService;
        _userManager = userManager;
        _mapper = mapper;
        _roleManager = roleManager;
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

    public async Task<StandardResponse<UserView>> GetUserProfile()
    {
        var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<Guid>();
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return StandardResponse<UserView>.Error("User not found");

        var userView = _mapper.Map<UserView>(user);
        return StandardResponse<UserView>.Ok(userView);
    }

    public async Task AddUserToPlatformAdminROle(string UserId)
    {
        var user = await _userManager.FindByEmailAsync(UserId.ToString());
        if (user == null)
            return;

        var roleExists = AssertRoleExists(Roles.PlatformAdmin);
        await _userManager.AddToRoleAsync(user, Roles.PlatformAdmin);
    }

    private bool AssertRoleExists(string roleName)
    {
        var roleExists = _roleManager.RoleExistsAsync(roleName).Result;
        if (!roleExists)
        {
            var role = new Role()
            {
                Name = roleName
            };
            var roleCreated = _roleManager.CreateAsync(role).Result;
            return roleExists;
        }
        return true;
    }
    public async Task<StandardResponse<UserView>> UpdateUser(UserProfile model)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<Guid>();
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return StandardResponse<UserView>.Error("User not found");

        // update user properties that are different from the model properties
        if (!string.IsNullOrEmpty(model.FirstName) && model.FirstName.ToLower() != user.FirstName.ToLower())
            user.FirstName = model.FirstName;

        if (!string.IsNullOrEmpty(model.LastName) && model.LastName.ToLower() != user.LastName.ToLower())
            user.LastName = model.LastName;

        if (!string.IsNullOrEmpty(model.PhoneNumber) && model.PhoneNumber.ToLower() != user.PhoneNumber.ToLower())
            user.PhoneNumber = model.PhoneNumber;

        if (!string.IsNullOrEmpty(model.OtherNames) && model.OtherNames.ToLower() != user.OtherNames.ToLower())
            user.OtherNames = model.OtherNames;

        if (!string.IsNullOrEmpty(model.Bio) && model.Bio.ToLower() != user.Bio.ToLower())
            user.Bio = model.Bio;

        if (!string.IsNullOrEmpty(model.ImageUrl) && model.ImageUrl.ToLower() != user.ImageUrl.ToLower())
            user.ImageUrl = model.ImageUrl;

        if (!string.IsNullOrEmpty(model.AddressLine1) && model.AddressLine1.ToLower() != user.AddressLine1.ToLower())
            user.AddressLine1 = model.AddressLine1;

        if (!string.IsNullOrEmpty(model.AddressLine2) && model.AddressLine2.ToLower() != user.AddressLine2.ToLower())
            user.AddressLine2 = model.AddressLine2;

        if (!string.IsNullOrEmpty(model.City) && model.City.ToLower() != user.City.ToLower())
            user.City = model.City;

        if (!string.IsNullOrEmpty(model.StateOrProvince) && model.StateOrProvince.ToLower() != user.StateOrProvince.ToLower())
            user.StateOrProvince = model.StateOrProvince;

        if (!string.IsNullOrEmpty(model.Country) && model.Country.ToLower() != user.Country.ToLower())
            user.Country = model.Country;

        if (!string.IsNullOrEmpty(model.PostalCode) && model.PostalCode.ToLower() != user.PostalCode.ToLower())
            user.PostalCode = model.PostalCode;

        var updateResponse = await _userManager.UpdateAsync(user);
        if (!updateResponse.Succeeded)
            return StandardResponse<UserView>.Error(updateResponse.Errors.FirstOrDefault().Description);

        user = await _userManager.FindByIdAsync(userId.ToString());
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
            var code = _codeService.GenerateCode(description, 6, numberOnly: true);
            templateModel.Add("code", code);
            await _postmarkHelper.SendTemplatedEmail(EmailTemplates.UserRegistrationTemplateMobile, email, templateModel);
        }

        await _postmarkHelper.SendTemplatedEmail(EmailTemplates.UserRegistrationTemplate, email, templateModel);
    }


    public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode, bool isMobile)
    {
        var link = $"{_appSettings.FrontEndUrl}/password/reset/{resetCode}?email={email}";
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
            var code = _codeService.GenerateCode(description, 6, numberOnly: true);
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
