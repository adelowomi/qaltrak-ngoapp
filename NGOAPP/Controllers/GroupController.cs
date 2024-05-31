using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace NGOAPP;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GroupController : StandardControllerBase
{
    private readonly IGroupService _groupService;
    private readonly PagingOptions _defaultPagingOptions;

    public GroupController(IGroupService groupService, IOptions<PagingOptions> defaultPagingOptions)
    {
        _groupService = groupService;
        _defaultPagingOptions = defaultPagingOptions.Value;
    }

    [HttpPost("create", Name = nameof(CreateGroup))]
    [ProducesResponseType(typeof(StandardResponse<GroupView>), 200)]
    [ProducesResponseType(typeof(StandardResponse<GroupView>), 400)]
    [ProducesResponseType(typeof(StandardResponse<GroupView>), 500)]
    public async Task<ActionResult<StandardResponse<GroupView>>> CreateGroup([FromBody] GroupModel model)
    {
        return Result(await _groupService.CreateGroup(model));
    }

    [HttpPost("update/{groupId}", Name = nameof(UpdateGroup))]
    [ProducesResponseType(typeof(StandardResponse<GroupView>), 200)]
    [ProducesResponseType(typeof(StandardResponse<GroupView>), 400)]
    [ProducesResponseType(typeof(StandardResponse<GroupView>), 500)]
    public async Task<ActionResult<StandardResponse<GroupView>>> UpdateGroup([FromBody] GroupModel model, Guid groupId)
    {
        return Result(await _groupService.UpdateGroup(model, groupId));
    }

    [HttpGet("details/{groupId}", Name = nameof(GetGroup))]
    [ProducesResponseType(typeof(StandardResponse<GroupView>), 200)]
    [ProducesResponseType(typeof(StandardResponse<GroupView>), 400)]
    [ProducesResponseType(typeof(StandardResponse<GroupView>), 500)]
    public async Task<ActionResult<StandardResponse<GroupView>>> GetGroup(Guid groupId)
    {
        return Result(await _groupService.GetGroup(groupId));
    }

    [HttpGet("list", Name = nameof(ListGroups))]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<GroupView>>), 200)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<GroupView>>), 400)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<GroupView>>), 500)]
    public async Task<ActionResult<StandardResponse<PagedCollection<GroupView>>>> ListGroups([FromQuery] PagingOptions _options)
    {
        _options.Replace(_defaultPagingOptions);
        return Result(await _groupService.ListGroups(_options));
    }

    [HttpGet("user/list/{userId}", Name = nameof(ListUserGroups))]
    [ProducesResponseType(typeof(StandardResponse<List<GroupView>>), 200)]
    [ProducesResponseType(typeof(StandardResponse<List<GroupView>>), 400)]
    [ProducesResponseType(typeof(StandardResponse<List<GroupView>>), 500)]
    public async Task<ActionResult<StandardResponse<List<GroupView>>>> ListUserGroups(Guid userId)
    {
        return Result(await _groupService.ListUserGroups(userId));
    }

    
    [HttpPost("follow/{groupId}", Name = nameof(FollowGroup))]
    [ProducesResponseType(typeof(StandardResponse<GroupView>), 200)]
    [ProducesResponseType(typeof(StandardResponse<GroupView>), 400)]
    [ProducesResponseType(typeof(StandardResponse<GroupView>), 500)]
    public async Task<ActionResult<StandardResponse<GroupView>>> FollowGroup(Guid groupId)
    {
        return Result(await _groupService.FollowGroup(groupId));
    }

    [HttpPost("unfollow/{groupId}", Name = nameof(UnfollowGroup))]
    [ProducesResponseType(typeof(StandardResponse<GroupView>), 200)]
    [ProducesResponseType(typeof(StandardResponse<GroupView>), 400)]
    [ProducesResponseType(typeof(StandardResponse<GroupView>), 500)]
    public async Task<ActionResult<StandardResponse<GroupView>>> UnfollowGroup(Guid groupId)
    {
        return Result(await _groupService.UnfollowGroup(groupId));
    }

    [HttpGet("followers/{groupId}", Name = nameof(GetGroupFollowers))]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<FollowerView>>), 200)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<FollowerView>>), 400)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<FollowerView>>), 500)]
    public async Task<ActionResult<StandardResponse<PagedCollection<FollowerView>>>> GetGroupFollowers(Guid groupId)
    {
        return Result(await _groupService.GetGroupFollowers(groupId));
    }

    [HttpGet("dashboard/{groupId}", Name = nameof(GetGroupDashBoard))]
    [ProducesResponseType(typeof(StandardResponse<GroupDashboardView>), 200)]
    [ProducesResponseType(typeof(StandardResponse<GroupDashboardView>), 400)]
    [ProducesResponseType(typeof(StandardResponse<GroupDashboardView>), 500)]
    public async Task<ActionResult<StandardResponse<GroupDashboardView>>> GetGroupDashBoard(Guid? groupId)
    {
        return Result(await _groupService.GetGroupDashBoard(groupId));
    }

    [HttpGet("user/followed", Name = nameof(GetGroupsFollowedByUser))]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<GroupView>>), 200)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<GroupView>>), 400)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<GroupView>>), 500)]
    public async Task<ActionResult<StandardResponse<PagedCollection<GroupView>>>> GetGroupsFollowedByUser([FromQuery] PagingOptions pagingOptions)
    {
        pagingOptions.Replace(_defaultPagingOptions);
        return Result(await _groupService.GetGroupsFollowedByUser(pagingOptions));
    }
}
