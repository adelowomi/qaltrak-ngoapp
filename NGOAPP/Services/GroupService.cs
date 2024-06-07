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
    private readonly IBaseRepository<Event> _eventRepository;
    private readonly IBaseRepository<Ticket> _ticketRepository;
    public GroupService(IBaseRepository<Group> groupRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IBaseRepository<GroupFollow> groupFollowRepository, IBaseRepository<GroupUser> groupUserRepository, IBaseRepository<Event> eventRepository, IBaseRepository<Ticket> ticketRepository)
    {
        _groupRepository = groupRepository;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _groupFollowRepository = groupFollowRepository;
        _groupUserRepository = groupUserRepository;
        _eventRepository = eventRepository;
        _ticketRepository = ticketRepository;
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
        var loggedInUser =  _httpContextAccessor.HttpContext.User.GetLoggedInUserId<Guid>();
        var group = _groupRepository.GetById(groupId);
        if (group == null)
            return StandardResponse<GroupView>.Error("Group not found", HttpStatusCode.NotFound);

        var totalEvents = _eventRepository.Count(x => x.GroupId == group.Id);
        var totalFollowers = _groupFollowRepository.Count(x => x.GroupId == group.Id);

        var groupView = group.Adapt<GroupView>();
        var existingGroupFollow = _groupFollowRepository.Query().FirstOrDefault(x => x.GroupId == groupId && x.UserId == loggedInUser);
        groupView.IsFollowing = existingGroupFollow != null;
        groupView.TotalEvents = totalEvents;
        groupView.TotalNumberOfFollowers = totalFollowers;
        return StandardResponse<GroupView>.Create(true, "Group retrieved successfully", groupView);
    }

    public async Task<StandardResponse<PagedCollection<GroupView>>> ListGroups(PagingOptions pagingOptions)
    {
        var groups = _groupRepository.Query().OrderByDescending(x => x.DateCreated).AsQueryable()
        .ApplySort(pagingOptions.SortDirection, pagingOptions.SortField);;
        var pagedGroups = groups.ToPagedCollection<Group, GroupView>(pagingOptions, Link.ToCollection(nameof(GroupController.ListGroups)));
        // get the count of followers for each group and add to the group view
        pagedGroups.Value.ToList().ForEach(x =>
        {
            x.TotalNumberOfFollowers = _groupFollowRepository.Count(y => y.GroupId == x.Id);
        });
        return StandardResponse<PagedCollection<GroupView>>.Create(true, "Groups retrieved successfully", pagedGroups);
    }

    public async Task<StandardResponse<List<GroupView>>> ListUserGroups(Guid userId)
    {
        var userGroups = _groupUserRepository.Query().Include(x => x.Group).Where(x => x.UserId == userId);
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

    public async Task<StandardResponse<GroupDashboardView>> GetGroupDashBoard(Guid? groupId)
    {
        var totalEvents = await GetTotalEvents(groupId);
        var totalPastEvents =await GetTotalPastEvents(groupId);
        var totalUpComingEvents = await GetTotalUpComingEvents(groupId);
        var totalAttendeeRegistered = await GetTotalAttendeeRegistered(groupId);
        var totalFollowers = await GetTotalFollowers(groupId);
        var upComingEvents = await GetUpcomingEvents(groupId);
        var topPerformingEvents =await GetTopPerformingEvents(groupId);
        var totalGroupsCreated = _groupRepository.Count();
        var TotalNumberOfFollowers = _groupFollowRepository.Count() ;

        var groupDashboardView = new GroupDashboardView
        {
            TotalEventsCreated = totalEvents,
            TotalPastEvents = totalPastEvents,
            TotalUpcomingEvents = totalUpComingEvents,
            TotalAttendeeRegistered = totalAttendeeRegistered,
            TotalFollowers = totalFollowers,
            UpcomingEvents = upComingEvents,
            TotalPerformingEventsPerResgistration = topPerformingEvents,
            TotalGroupsCreated = totalGroupsCreated,
            TotalNumberOfAllFOllowers = TotalNumberOfFollowers
        };
        return StandardResponse<GroupDashboardView>.Ok(groupDashboardView);
    }

    public async Task<StandardResponse<PagedCollection<GroupView>>> GetGroupsFollowedByUser(PagingOptions pagingOptions)
    {
        var loggedInUser = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<Guid>();
        var groupFollows = _groupFollowRepository.Query().Include(x => x.Group).Where(x => x.UserId == loggedInUser);
        var pagedGroups = groupFollows.Select(x => x.Group).ToPagedCollection<Group, GroupView>(pagingOptions, Link.ToCollection(nameof(GroupController.GetGroupsFollowedByUser)));
        return StandardResponse<PagedCollection<GroupView>>.Create(true, "Groups followed by user retrieved successfully", pagedGroups);
    }

    private async Task<int> GetTotalEvents(Guid? groupId)
    {
        if(groupId == Guid.Empty || groupId == null)
            return _eventRepository.Count();
        return await _eventRepository.Query().CountAsync(x => x.GroupId == groupId);
    }

    private async Task<int> GetTotalPastEvents(Guid? groupId)
    {
        if(groupId == Guid.Empty || groupId == null)
            return _eventRepository.Query().Count(x => x.EndDate < DateTime.Now);
        return await _eventRepository.Query().CountAsync(x => x.GroupId == groupId && x.EndDate < DateTime.Now);
    }

    private async Task<int> GetTotalUpComingEvents(Guid? groupId)
    {
        if(groupId == Guid.Empty || groupId == null)
            return _eventRepository.Query().Count(x => x.StartDate > DateTime.Now);
        return await _eventRepository.Query().CountAsync(x => x.GroupId == groupId && x.StartDate > DateTime.Now);
    }

    private async Task<int> GetTotalAttendeeRegistered(Guid? groupId)
    {
        if(groupId == Guid.Empty || groupId == null)
            return _ticketRepository.Query().Include(x => x.Event).Count();
        return await _ticketRepository.Query().Include(x => x.Event).CountAsync(x => x.Event.GroupId == groupId);
    }

    private async Task<int> GetTotalFollowers(Guid? groupId)
    {
        if(groupId == Guid.Empty || groupId == null)
            return _groupFollowRepository.Count();
        return await _groupFollowRepository.Query().CountAsync(x => x.GroupId == groupId);
    }

    private async Task<List<EventView>> GetUpcomingEvents(Guid? groupId)
    {
        if(groupId == Guid.Empty || groupId == null)
            return _eventRepository.Query().Where(x => x.StartDate > DateTime.Now).Take(10).ToList().Adapt<List<EventView>>();
        var events = _eventRepository.Query().Where(x => x.GroupId == groupId && x.StartDate > DateTime.Now).Take(10).ToList();
        return events.Adapt<List<EventView>>();
    }

    private async Task<List<EventView>> GetTopPerformingEvents(Guid? groupId)
    {
        //get event id of highest number of tickets sold
        var eventIds = _ticketRepository.Query().GroupBy(x => x.EventId).OrderByDescending(x => x.Count()).Select(x => x.Key).Take(10).ToList();
        // get events with the highest number of tickets sold
        var events = _eventRepository.Query().Where(x => eventIds.Contains(x.Id)).ToList();
        return events.Adapt<List<EventView>>();
    }
}
