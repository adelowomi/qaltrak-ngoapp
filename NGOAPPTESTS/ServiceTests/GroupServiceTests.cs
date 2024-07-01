using System.Security.Claims;
using Mapster;

namespace NGOAPPTESTS;

public class GroupServiceTests
{

    private readonly GroupService _groupService;
    private readonly IBaseRepository<Group> _groupRepository = Substitute.For<IBaseRepository<Group>>();
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IBaseRepository<GroupFollow> _groupFollowRepository = Substitute.For<IBaseRepository<GroupFollow>>();
    private readonly IBaseRepository<GroupUser> _groupUserRepository = Substitute.For<IBaseRepository<GroupUser>>();
    private readonly IBaseRepository<Event> _eventRepository = Substitute.For<IBaseRepository<Event>>();
    private readonly IBaseRepository<Ticket> _ticketRepository = Substitute.For<IBaseRepository<Ticket>>();
    private readonly IPostmarkHelper _postmarkHelper = Substitute.For<IPostmarkHelper>();
    private readonly IBaseRepository<User> _userRepository = Substitute.For<IBaseRepository<User>>();

    public GroupServiceTests()
    {
        _groupService = new GroupService(_groupRepository, _httpContextAccessor, _mapper, _groupFollowRepository, _groupUserRepository, _eventRepository, _ticketRepository, _postmarkHelper, _userRepository);
    }

    [Fact]
    public async Task CreateGroup_Success()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, FirstName = "Test", Email = "test@example.com" };
        var groupModel = new GroupModel { Name = "Test Group" };
        var group = groupModel.Adapt<Group>();

        _httpContextAccessor.HttpContext.Returns(new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) })) });
        _userRepository.GetById(userId).Returns(user);
        _groupRepository.Query().Returns(new List<Group>().AsQueryable());
        _groupRepository.CreateAndReturn(Arg.Any<Group>()).Returns(group);

        // Act
        var result = await _groupService.CreateGroup(groupModel);

        // Assert
        Assert.True(result.Status);
        Assert.Equal("Group created successfully", result.Message);
    }

    [Fact]
    public async Task CreateGroup_GroupAlreadyExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, FirstName = "Test", Email = "test@example.com" };
        var groupModel = new GroupModel { Name = "Test Group" };
        var existingGroup = new Group { Name = "Test Group" };

        _httpContextAccessor.HttpContext.Returns(new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) })) });
        _userRepository.GetById(userId).Returns(user);
        _groupRepository.Query().Returns(new List<Group> { existingGroup }.AsQueryable());

        // Act
        var result = await _groupService.CreateGroup(groupModel);

        // Assert
        Assert.False(result.Status);
        Assert.Equal("Group already exists", result.Message);
    }

    [Fact]
    public async Task CreateGroup_UserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var groupModel = new GroupModel { Name = "Test Group" };

        _httpContextAccessor.HttpContext.Returns(new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) })) });
        _userRepository.GetById(userId).Returns((User)null);

        // Act
        var result = await _groupService.CreateGroup(groupModel);

        // Assert
        Assert.False(result.Status);
        Assert.Equal("We can not find the user trying to create this group. Please contact admin.", result.Message);
    }

    [Fact]
    public async Task GetGroup_Success()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var group = new Group { Id = groupId, Name = "Test Group" };

        _httpContextAccessor.HttpContext.Returns(new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) })) });
        _groupRepository.GetById(groupId).Returns(group);
        _groupFollowRepository.Count(Arg.Any<Func<GroupFollow, bool>>()).Returns(0);
        _eventRepository.Count(Arg.Any<Func<Event, bool>>()).Returns(0);

        // Act
        var result = await _groupService.GetGroup(groupId);

        // Assert
        Assert.True(result.Status);
        Assert.Equal("Group retrieved successfully", result.Message);
    }

    [Fact]
    public async Task GetGroup_GroupNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var groupId = Guid.NewGuid();

        _httpContextAccessor.HttpContext.Returns(new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) })) });
        _groupRepository.GetById(groupId).Returns((Group)null);

        // Act
        var result = await _groupService.GetGroup(groupId);

        // Assert
        Assert.False(result.Status);
        Assert.Equal("Group not found", result.Message);
    }

    // Similar tests can be written for UpdateGroup, FollowGroup, UnfollowGroup, etc.

    
}

