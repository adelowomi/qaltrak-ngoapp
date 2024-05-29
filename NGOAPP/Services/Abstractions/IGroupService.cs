namespace NGOAPP;

public interface IGroupService
{
    Task<StandardResponse<GroupView>> CreateGroup(GroupModel model);
    Task<StandardResponse<GroupView>> UpdateGroup(GroupModel model, Guid groupId);
    Task<StandardResponse<GroupView>> GetGroup(Guid groupId);
    Task<StandardResponse<PagedCollection<GroupView>>> ListGroups(PagingOptions _options);
    Task<StandardResponse<GroupView>> FollowGroup(Guid groupId);
    Task<StandardResponse<GroupView>> UnfollowGroup(Guid groupId);
    Task<StandardResponse<PagedCollection<FollowerView>>> GetGroupFollowers(Guid groupId);
    Task<StandardResponse<GroupDashboardView>> GetGroupDashBoard(Guid groupId);
    Task<StandardResponse<PagedCollection<GroupView>>> GetGroupsFollowedByUser(PagingOptions pagingOptions);
}
