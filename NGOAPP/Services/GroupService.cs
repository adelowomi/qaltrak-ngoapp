using System.Net;
using AutoMapper;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace NGOAPP;

public class GroupService : IGroupService
{
    private readonly IBaseRepository<Group> _groupRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IBaseRepository<GroupFollow> _groupFollowRepository;
    private readonly IBaseRepository<GroupUser> _groupUserRepository;

    public GroupService(IBaseRepository<Group> groupRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IBaseRepository<GroupFollow> groupFollowRepository, IBaseRepository<GroupUser> groupUserRepository)
    {
        _groupRepository = groupRepository;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _groupFollowRepository = groupFollowRepository;
        _groupUserRepository = groupUserRepository;
    }

    public async Task<StandardResponse<GroupView>> CreateGroup(GroupModel model)
    {
        var loggedInUser = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<Guid>();
        var group = model.Adapt<Group>();
        var existingGroup = _groupRepository.Query().FirstOrDefault(x => x.Name.ToLower() == group.Name.ToLower());

        if (existingGroup != null)
            return StandardResponse<GroupView>.Error("Group already exists", HttpStatusCode.BadRequest);

        group.UserId = loggedInUser;
        group = _groupRepository.CreateAndReturn(group);
        var groupView = group.Adapt<GroupView>();
        return StandardResponse<GroupView>.Create(true, "Group created successfully", groupView); 
    }

    public async Task<StandardResponse<GroupView>> UpdateGroup(GroupModel model, Guid groupId)
    {
        var group = _groupRepository.GetById(groupId);
        if (group == null)
            return StandardResponse<GroupView>.Error("Group not found", HttpStatusCode.NotFound);

        group.Name = model.Name;
        group.Bio = model.Bio;
        group.Image = model.Image;
        group.WebsiteUrl = model.WebsiteUrl;
        group.Email = model.Email;
        group.Mission = model.Mission;
        group.Commitment = model.Commitment;
        group = _groupRepository.Update(group);
        var groupView = group.Adapt<GroupView>();
        return StandardResponse<GroupView>.Create(true, "Group updated successfully", groupView);
    }

    public async Task<StandardResponse<GroupView>> GetGroup(Guid groupId)
    {
        var group = _groupRepository.GetById(groupId);
        if (group == null)
            return StandardResponse<GroupView>.Error("Group not found", HttpStatusCode.NotFound);

        var groupView = group.Adapt<GroupView>();
        return StandardResponse<GroupView>.Create(true, "Group retrieved successfully", groupView);
    }

    public async Task<StandardResponse<PagedCollection<GroupView>>> ListGroups(PagingOptions pagingOptions)
    {
        var groups = _groupRepository.Query().OrderByDescending(x => x.DateCreated).AsQueryable();
        var pagedGroups = groups.ToPagedCollection<Group, GroupView>(pagingOptions, Link.ToCollection(nameof(GroupController.ListGroups)));
        // get the count of followers for each group and add to the group view
        return StandardResponse<PagedCollection<GroupView>>.Create(true, "Groups retrieved successfully", pagedGroups);
    }

    public async Task<StandardResponse<List<GroupView>>> ListUserGroups(Guid userId)
    {
        var userGroups  = _groupUserRepository.Query().Include(x => x.Group).Where(x => x.UserId == userId);
        var groupViews = userGroups.Select(x => x.Group).Adapt<List<GroupView>>();
        return StandardResponse<List<GroupView>>.Create(true, "Groups retrieved successfully", groupViews);
    }

    public async Task<StandardResponse<GroupView>> FollowGroup(Guid groupId)
    {
        var loggedInUser = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<Guid>();
        var existingGroupFollow = _groupFollowRepository.Query().FirstOrDefault(x => x.GroupId == groupId && x.UserId == loggedInUser);

        if (existingGroupFollow != null)
            return StandardResponse<GroupView>.Error("You are already following this group", HttpStatusCode.BadRequest);

        var groupFollow = new GroupFollow
        {
            GroupId = groupId,
            UserId = loggedInUser
        };

        groupFollow = _groupFollowRepository.CreateAndReturn(groupFollow);
        var group = _groupRepository.GetById(groupId);
        var groupView = group.Adapt<GroupView>();
        return StandardResponse<GroupView>.Create(true, "Group followed successfully", groupView);
    }

    public async Task<StandardResponse<GroupView>> UnfollowGroup(Guid groupId)
    {
        var loggedInUser = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<Guid>();
        var existingGroupFollow = _groupFollowRepository.Query().FirstOrDefault(x => x.GroupId == groupId && x.UserId == loggedInUser);

        if (existingGroupFollow == null)
            return StandardResponse<GroupView>.Error("You are not following this group", HttpStatusCode.BadRequest);

        _groupFollowRepository.Delete(existingGroupFollow);
        var group = _groupRepository.GetById(groupId);
        var groupView = group.Adapt<GroupView>();
        return StandardResponse<GroupView>.Create(true, "Group unfollowed successfully", groupView);
    }

    public async Task<StandardResponse<PagedCollection<FollowerView>>> GetGroupFollowers(Guid groupId)
    {
        var groupFollowers = _groupFollowRepository.Query().Include(x => x.User).Where(x => x.GroupId == groupId);
        var pagedFollowers = groupFollowers.ToPagedCollection<GroupFollow, FollowerView>(new PagingOptions(), Link.ToCollection(nameof(GroupController.GetGroupFollowers)));
        return StandardResponse<PagedCollection<FollowerView>>.Create(true, "Group followers retrieved successfully", pagedFollowers);
    }
}
