using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NGOAPP.Services;

namespace NGOAPPTESTS;

public class AdminServiceTests
{
    private readonly UserManager<User> _userManager;
    private readonly IBaseRepository<GroupUser> _groupUserRepository;
    private readonly ICodeService _codeService;
    private readonly RoleManager<Role> _roleManager;
    private readonly IBaseRepository<AdminSchedule> _adminScheduleRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceProvider _serviceProvider;
    private readonly AppSettings _appSettings;
    private readonly IPostmarkHelper _postmarkHelper;

    private readonly AdminService _adminService;

    public AdminServiceTests()
    {
        _userManager = Substitute.For<UserManager<User>>(Substitute.For<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        _groupUserRepository = Substitute.For<IBaseRepository<GroupUser>>();
        _codeService = Substitute.For<ICodeService>();
        _roleManager = Substitute.For<RoleManager<Role>>(Substitute.For<IRoleStore<Role>>(), null, null, null, null);
        _adminScheduleRepository = Substitute.For<IBaseRepository<AdminSchedule>>();
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _serviceProvider = Substitute.For<IServiceProvider>();
        _appSettings = new AppSettings();
        _postmarkHelper = Substitute.For<IPostmarkHelper>();

        _adminService = new AdminService(
            _userManager,
            _groupUserRepository,
            _codeService,
            _roleManager,
            _adminScheduleRepository,
            _httpContextAccessor,
            _serviceProvider,
            Options.Create(_appSettings),
            _postmarkHelper
        );
    }

    // [Fact]
    public async Task CreateUser_WithValidModel_ShouldReturnUserView()
    {
        // Arrange
        var model = new AdminModel
        {
            Email = "test@example.com",
            Role = "Admin",
            GroupId = Guid.NewGuid()
        };

        var existingUser = new User { Id = Guid.NewGuid(), Email = model.Email };
        _userManager.FindByEmailAsync(model.Email).Returns((User)null);

        var newUser = new User { Id = Guid.NewGuid(), Email = model.Email, FirstName = "John" };
        _userManager.CreateAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(IdentityResult.Success);
        _userManager.FindByIdAsync(newUser.Id.ToString()).Returns(newUser);
        _userManager.GetRolesAsync(newUser).Returns(new List<string> { model.Role });

        _groupUserRepository.Query().Returns(new List<GroupUser>{
                new GroupUser { UserId = existingUser.Id, GroupId = model.GroupId }
            }.AsQueryable());

        newUser.Adapt<UserView>().Returns(new UserView { Id = newUser.Id, FirstName = newUser.FirstName });

        // _groupUserRepository.Query().FirstOrDefault(x => x.UserId == existingUser.Id && x.GroupId == model.GroupId);

        _codeService.GenerateCode("DefaultPassword", 8).Returns("ABC12345");
        _codeService.GenerateCode("DefaultPassword", 4).Returns("def6");
        _codeService.GenerateCode("DefaultPassword", 4, numberOnly: true).Returns("7890");

        _postmarkHelper.SendTemplatedEmail(EmailTemplates.AdminUserInviteTemplate, newUser.Email, Arg.Any<Dictionary<string, string>>()).Returns(Task.CompletedTask);

        // Act
        var result = await _adminService.CreateUser(model);

        // Assert
        Assert.True(result.Status);
        Assert.NotNull(result.Data);
        Assert.Equal(newUser.Id, result.Data.Id);
        Assert.Equal(newUser.FirstName, result.Data.FirstName);
    }

    // [Fact]
    public async Task CreateUser_WithExistingUserInGroup_ShouldReturnError()
    {
        // Arrange
        var model = new AdminModel
        {
            Email = "test@example.com",
            Role = "Admin",
            GroupId = Guid.NewGuid()
        };

        var existingUser = new User { Id = Guid.NewGuid(), Email = model.Email };
        _userManager.FindByEmailAsync(model.Email).Returns(existingUser);

        _groupUserRepository.Query().FirstOrDefault(x => x.UserId == existingUser.Id && x.GroupId == model.GroupId).Returns(new GroupUser());

        // Act
        var result = await _adminService.CreateUser(model);

        // Assert
        Assert.False(result.Status);
        Assert.Equal("User already exists in the group", result.Message);
    }

    // Add more test cases for other methods in the AdminService class
}
