using Microsoft.AspNetCore.Mvc;

namespace NGOAPP;

[Route("api/[controller]")]
[ApiController]
public class GroupController : StandardControllerBase
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
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
        return Result(await _groupService.ListGroups(_options));
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
    public async Task<ActionResult<StandardResponse<GroupDashboardView>>> GetGroupDashBoard(Guid groupId)
    {
        return Result(await _groupService.GetGroupDashBoard(groupId));
    }
}
