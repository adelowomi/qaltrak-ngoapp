using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace NGOAPP;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = Roles.PlatformAdmin)]
public class AdminController : StandardControllerBase
{
    private readonly IAdminService _adminService;
    private readonly PagingOptions _defaultPagingOptions;

    public AdminController(IAdminService adminService, IOptions<PagingOptions> defaultPagingOptions)
    {
        _adminService = adminService;
        _defaultPagingOptions = defaultPagingOptions.Value;
    }

    [HttpPost("user/create", Name = (nameof(CreateAdminUser)))]
    [ProducesResponseType(typeof(StandardResponse<UserView>), 200)]
    [ProducesResponseType(typeof(StandardResponse<UserView>), 400)]
    [ProducesResponseType(typeof(StandardResponse<UserView>), 500)]
    public async Task<ActionResult<StandardResponse<UserView>>> CreateAdminUser(AdminModel model)
    {
        return Result(await _adminService.CreateUser(model));
    }

    [HttpGet("roles/list", Name = (nameof(ListRoles)))]
    [ProducesResponseType(typeof(StandardResponse<List<RolesView>>), 200)]
    [ProducesResponseType(typeof(StandardResponse<List<RolesView>>), 400)]
    [ProducesResponseType(typeof(StandardResponse<List<RolesView>>), 500)]
    public async Task<ActionResult<StandardResponse<List<RolesView>>>> ListRoles()
    {
        return Result(await _adminService.ListRoles());
    }

    [HttpPost("add/user/{userId}/group/{groupId}", Name = (nameof(AddUserToGroup)))]
    [ProducesResponseType(typeof(StandardResponse<UserView>), 200)]
    [ProducesResponseType(typeof(StandardResponse<UserView>), 400)]
    [ProducesResponseType(typeof(StandardResponse<UserView>), 500)]
    public async Task<ActionResult<StandardResponse<UserView>>> AddUserToGroup(Guid userId, Guid groupId)
    {
        return Result(await _adminService.AddUserToGroup(userId, groupId));
    }

    [HttpGet("list/group/users", Name = (nameof(ListGroupUsers)))]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<UserView>>), 200)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<UserView>>), 400)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<UserView>>), 500)]
    public async Task<ActionResult<StandardResponse<PagedCollection<UserView>>>> ListGroupUsers([FromQuery] PagingOptions pagingOptions,[FromQuery] string? searchQuery)
    {
        pagingOptions.Replace(_defaultPagingOptions);
        return Result(await _adminService.ListGroupUsers(pagingOptions, searchQuery));
    }

    [HttpPost("schedule/create", Name = (nameof(CreateAdminSchedule)))]
    [ProducesResponseType(typeof(StandardResponse<AdminScheduleModel>), 200)]
    [ProducesResponseType(typeof(StandardResponse<AdminScheduleModel>), 400)]
    [ProducesResponseType(typeof(StandardResponse<AdminScheduleModel>), 500)]
    public async Task<ActionResult<StandardResponse<AdminScheduleModel>>> CreateAdminSchedule(AdminScheduleModel model)
    {
        return Result(await _adminService.CreateAdminSchedule(model));
    }

    [HttpGet("schedule/list/{currentMonth}", Name = (nameof(ListAdminSchedulesForCurrentMonth)))]
    [ProducesResponseType(typeof(StandardResponse<List<AdminScheduleModel>>), 200)]
    [ProducesResponseType(typeof(StandardResponse<List<AdminScheduleModel>>), 400)]
    [ProducesResponseType(typeof(StandardResponse<List<AdminScheduleModel>>), 500)]
    public async Task<ActionResult<StandardResponse<List<AdminScheduleModel>>>> ListAdminSchedulesForCurrentMonth(int currentMonth = 0)
    {
        return Result(await _adminService.ListAdminSchedulesForCurrentMonth(currentMonth));
    }

    [HttpGet("schedule/{scheduleId}", Name = (nameof(GetAdminSchedule)))]
    [ProducesResponseType(typeof(StandardResponse<AdminScheduleModel>), 200)]
    [ProducesResponseType(typeof(StandardResponse<AdminScheduleModel>), 400)]
    [ProducesResponseType(typeof(StandardResponse<AdminScheduleModel>), 500)]
    public async Task<ActionResult<StandardResponse<AdminScheduleModel>>> GetAdminSchedule(Guid scheduleId)
    {
        return Result(await _adminService.GetAdminSchedule(scheduleId));
    }
}
