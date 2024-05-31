using System.Diagnostics.CodeAnalysis;
using System.Net;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NGOAPP.Models.IdentityModels;
using NGOAPP.Services;

namespace NGOAPP;

public class AdminService : IAdminService
{
    private readonly UserManager<User> _userManager;
    private readonly IBaseRepository<GroupUser> _groupUserRepository;
    private readonly ICodeService _codeService;
    private readonly RoleManager<Role> _roleManager;
    private readonly IBaseRepository<AdminSchedule> _adminScheduleRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AdminService(UserManager<User> userManager, IBaseRepository<GroupUser> groupUserRepository, ICodeService codeService, RoleManager<Role> roleManager, IBaseRepository<AdminSchedule> adminScheduleRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _groupUserRepository = groupUserRepository;
        _codeService = codeService;
        _roleManager = roleManager;
        _adminScheduleRepository = adminScheduleRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<StandardResponse<UserView>> CreateUser(AdminModel model)
    {
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            var existingGroupUser = _groupUserRepository.Query().FirstOrDefault(x => x.UserId == existingUser.Id && x.GroupId == model.GroupId);
            if (existingGroupUser != null)
                return StandardResponse<UserView>.Error("User already exists in the group");
        }

        var newUser = model.Adapt<User>();

        var randomPasswordUpper = _codeService.GenerateCode("DefaultPassword", 8);
        var randomPasswordLower = _codeService.GenerateCode("DefaultPassword", 4).ToLower();
        var randomFourDigit = _codeService.GenerateCode("DefaultPassword", 4, numberOnly: true);

        var result = await _userManager.CreateAsync(newUser, $"{randomPasswordUpper}{randomPasswordLower}@{randomFourDigit}");

        if (!result.Succeeded)
            return StandardResponse<UserView>.Error(result.Errors.FirstOrDefault()?.Description);

        if (model.GroupId != Guid.Empty)
        {
            var groupUser = new GroupUser
            {
                GroupId = model.GroupId,
                UserId = newUser.Id
            };

            var thisGroupUser = _groupUserRepository.CreateAndReturn(groupUser);
        }
        // add user to role
        if (!string.IsNullOrEmpty(model.Role))
        {
            var roleExists = AssertRoleExists(model.Role);
            if (roleExists)
            {
                var roleAdded = _userManager.AddToRoleAsync(newUser, model.Role).Result;
            }
        }

        var userView = newUser.Adapt<UserView>();
        return StandardResponse<UserView>.Ok(userView);
    }

    public async Task<StandardResponse<List<RolesView>>> ListRoles()
    {
        var roleView = new List<RolesView>();
        var roles = new List<string> { Roles.PlatformAdmin, Roles.Admin, Roles.User };
        roles.ForEach(role =>
        {
            roleView.Add(new RolesView
            {
                Value = role.Replace(" ", ""),
                // name is role with space in between each word if word is more than 1
                Name = role.Split(' ').Length > 1 ? role : role.Replace(" ", "")
            });
        });
        return StandardResponse<List<RolesView>>.Ok(roleView);
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

    public async Task<StandardResponse<UserView>> AddUserToGroup(Guid userId, Guid groupId)
    {
        var existingUser = await _userManager.FindByIdAsync(userId.ToString());
        if (existingUser == null)
            return StandardResponse<UserView>.Error("User not found");

        var existingGroupUser = _groupUserRepository.Query().FirstOrDefault(x => x.UserId == userId && x.GroupId == groupId);
        if (existingGroupUser != null)
            return StandardResponse<UserView>.Error("User already exists in the group");

        var groupUser = new GroupUser
        {
            GroupId = groupId,
            UserId = userId
        };

        var thisGroupUser = _groupUserRepository.CreateAndReturn(groupUser);
        var userView = existingUser.Adapt<UserView>();
        return StandardResponse<UserView>.Ok(userView);
    }

    public async Task<StandardResponse<PagedCollection<UserView>>> ListGroupUsers(PagingOptions pagingOptions, string searchQuery)
    {
        var groupUsers = _groupUserRepository.Query().Include(x => x.User).AsQueryable();
        groupUsers = FilterUsers(groupUsers, searchQuery);
        var pagedGroupUsers = groupUsers.ToPagedCollection<GroupUser, UserView>(pagingOptions, Link.ToCollection(nameof(AdminController.ListGroupUsers)));
        return StandardResponse<PagedCollection<UserView>>.Ok(pagedGroupUsers);
    }

    public async Task<StandardResponse<AdminScheduleModel>> CreateAdminSchedule(AdminScheduleModel model)
    {
        var loggedInUser = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<Guid>();
        var adminSchedule = model.Adapt<AdminSchedule>();
        adminSchedule.AdminId = loggedInUser;
        adminSchedule = _adminScheduleRepository.CreateAndReturn(adminSchedule);
        var adminScheduleModel = adminSchedule.Adapt<AdminScheduleModel>();
        return StandardResponse<AdminScheduleModel>.Ok(adminScheduleModel);
    }

    public async Task<StandardResponse<List<AdminScheduleModel>>> ListAdminSchedulesForCurrentMonth(int currentMonth = 0)
    {
        var loggedInUser = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<Guid>();
        // if current month is not passed, get the current month from the date
        if (currentMonth == 0)
            currentMonth = DateTime.Now.Month;
        var adminSchedules = _adminScheduleRepository.Query().Where(x => x.AdminId == loggedInUser && x.StartDate.Month == currentMonth).ToList();
        var adminScheduleModels = adminSchedules.Adapt<List<AdminScheduleModel>>();
        return StandardResponse<List<AdminScheduleModel>>.Ok(adminScheduleModels);
    }

    public async Task<StandardResponse<AdminScheduleModel>> GetAdminSchedule(Guid scheduleId)
    {
        var adminSchedule = _adminScheduleRepository.GetById(scheduleId);
        if (adminSchedule == null)
            return StandardResponse<AdminScheduleModel>.Error("Schedule not found", HttpStatusCode.NotFound);

        var adminScheduleModel = adminSchedule.Adapt<AdminScheduleModel>();
        return StandardResponse<AdminScheduleModel>.Ok(adminScheduleModel);
    }

    private  IQueryable<GroupUser> FilterUsers(IQueryable<GroupUser> users, string searchQuery)
    {
        if (!string.IsNullOrEmpty(searchQuery))
            users = users.Where(x => x.User.Email.Contains(searchQuery) || x.User.PhoneNumber.Contains(searchQuery) || x.User.FirstName.Contains(searchQuery) || x.User.LastName.Contains(searchQuery));

        return users;
    }
}
