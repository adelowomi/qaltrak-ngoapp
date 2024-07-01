using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NGOAPP.Models.AppModels;
using NGOAPP.Services;

namespace NGOAPPTESTS;

public class UserServiceTests
{
    private readonly AppSettings _appSettings;
    private readonly IPostmarkHelper _postmarkHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICodeService _codeService;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly RoleManager<Role> _roleManager;

    private readonly UserService _userService;

    public UserServiceTests()
    {
        _appSettings = new AppSettings();
        _postmarkHelper = Substitute.For<IPostmarkHelper>();
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _codeService = Substitute.For<ICodeService>();
        _userManager = Substitute.For<UserManager<User>>(Substitute.For<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        _mapper = Substitute.For<IMapper>();
        _roleManager = Substitute.For<RoleManager<Role>>(Substitute.For<IRoleStore<Role>>(), null, null, null, null);

        _userService = new UserService(
            Options.Create(_appSettings),
            _postmarkHelper,
            _httpContextAccessor,
            _codeService,
            _userManager,
            _mapper,
            _roleManager
        );
    }

    [Fact]
    public async Task VerifyUserFromMobileCodeAsync_ValidCode_ReturnsUserView()
    {
        // Arrange
        var code = "123456";
        var codeData = new Code { Description = "1||123456" };
        var user = new User();
        var verificationResponse = IdentityResult.Success;
        var expectedUserView = new UserView();

        _codeService.GetCode(code).Returns(codeData);
        _userManager.FindByIdAsync("1").Returns(user);
        _userManager.ConfirmEmailAsync(user, "123456").Returns(verificationResponse);
        _mapper.Map<UserView>(user).Returns(expectedUserView);

        // Act
        var result = await _userService.VerifyUserFromMobileCodeAsync(code);

        // Assert
        Assert.Equal(true, result.Status);
        Assert.Equal(expectedUserView, result.Data);
    }

    [Fact]
    public async Task VerifyUserFromMobileCodeAsync_InvalidCode_ReturnsError()
    {
        // Arrange
        var code = "123456";
        var codeData = (Code)null;

        _codeService.GetCode(code).Returns(codeData);

        // Act
        var result = await _userService.VerifyUserFromMobileCodeAsync(code);

        // Assert
        Assert.Equal(false, result.Status);
        Assert.Equal("Invalid code", result.Message);
    }

    [Fact]
    public async Task GetUserProfile_ExistingUser_ReturnsUserView()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com"
        };


        _userManager.FindByIdAsync(userId.ToString()).Returns(user);


        _mapper.Map<UserView>(user).Returns(new UserView
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com"
        });

        // Act
        var result = await _userService.GetUserProfile();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Status);
        Assert.NotNull(result.Data);
        Assert.Equal(userId, result.Data.Id);
        Assert.Equal("John", result.Data.FirstName);
        Assert.Equal("Doe", result.Data.LastName);
        Assert.Equal("johndoe@example.com", result.Data.Email);
    }

    [Fact]
    public async Task GetUserProfile_NonExistingUser_ReturnsError()
    {
        // Arrange
        var userId = Guid.NewGuid();


        _userManager.FindByIdAsync(userId.ToString()).Returns((User)null);


        // Act
        var result = await _userService.GetUserProfile();

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Status);
        Assert.Null(result.Data);
        Assert.Equal("User not found", result.Message);
    }

    [Fact]
        public async Task UpdateUser_ValidUserProfile_ReturnsOkResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var model = new UserProfile
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                Bio = "Lorem ipsum",
                ImageUrl = "https://example.com/image.jpg",
                AddressLine1 = "123 Street",
                AddressLine2 = "Apt 4B",
                City = "New York",
                StateOrProvince = "NY",
                Country = "USA",
                PostalCode = "12345"
            };



            var user = new User
            {
                Id = userId,
                FirstName = "Jane",
                LastName = "Doe",
                PhoneNumber = "0987654321",
                Bio = "Lorem ipsum dolor sit amet",
                ImageUrl = "https://example.com/old-image.jpg",
                AddressLine1 = "456 Street",
                AddressLine2 = "Apt 2C",
                City = "Los Angeles",
                StateOrProvince = "CA",
                Country = "USA",
                PostalCode = "54321"
            };

            _userManager.FindByIdAsync(userId.ToString()).Returns(user);
            _userManager.UpdateAsync(Arg.Any<User>()).Returns(IdentityResult.Success);

            // Act
            var result = await _userService.UpdateUser(model);

            // Assert
            Assert.True(result.Status);
            Assert.Equal(model.FirstName, result.Data.FirstName);
            Assert.Equal(model.LastName, result.Data.LastName);
            Assert.Equal(model.PhoneNumber, result.Data.PhoneNumber);
            Assert.Equal(model.Bio, result.Data.Bio);
            Assert.Equal(model.ImageUrl, result.Data.ImageUrl);
            Assert.Equal(model.AddressLine1, result.Data.AddressLine1);
            Assert.Equal(model.AddressLine2, result.Data.AddressLine2);
            Assert.Equal(model.City, result.Data.City);
            Assert.Equal(model.StateOrProvince, result.Data.StateOrProvince);
            Assert.Equal(model.Country, result.Data.Country);
            Assert.Equal(model.PostalCode, result.Data.PostalCode);
        }

        [Fact]
        public async Task UpdateUser_InvalidUserProfile_ReturnsErrorResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var model = new UserProfile
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                Bio = "Lorem ipsum",
                ImageUrl = "https://example.com/image.jpg",
                AddressLine1 = "123 Street",
                AddressLine2 = "Apt 4B",
                City = "New York",
                StateOrProvince = "NY",
                Country = "USA",
                PostalCode = "12345"
            };



            var user = new User
            {
                Id = userId,
                FirstName = "Jane",
                LastName = "Doe",
                PhoneNumber = "0987654321",
                Bio = "Lorem ipsum dolor sit amet",
                ImageUrl = "https://example.com/old-image.jpg",
                AddressLine1 = "456 Street",
                AddressLine2 = "Apt 2C",
                City = "Los Angeles",
                StateOrProvince = "CA",
                Country = "USA",
                PostalCode = "54321"
            };

            _userManager.FindByIdAsync(userId.ToString()).Returns(user);
            _userManager.UpdateAsync(Arg.Any<User>()).Returns(IdentityResult.Failed(new IdentityError { Description = "Failed to update user" }));

            // Act
            var result = await _userService.UpdateUser(model);

            // Assert
            Assert.False(result.Status);
            Assert.Equal("Failed to update user", result.Message);
        }

        [Fact]
        public async Task UpdateUser_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var model = new UserProfile
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                Bio = "Lorem ipsum",
                ImageUrl = "https://example.com/image.jpg",
                AddressLine1 = "123 Street",
                AddressLine2 = "Apt 4B",
                City = "New York",
                StateOrProvince = "NY",
                Country = "USA",
                PostalCode = "12345"
            };


            _userManager.FindByIdAsync(userId.ToString()).Returns((User)null);

            // Act
            var result = await _userService.UpdateUser(model);

            // Assert
            Assert.False(result.Status);
            Assert.Equal("User not found", result.Message);
        }
}
