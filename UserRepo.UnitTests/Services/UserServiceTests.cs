using Moq;
using UserRepo.Application.Errors;
using UserRepo.Application.Interfaces;
using UserRepo.Application.Services;
using UserRepo.Contracts.Requests;
using UserRepo.Domain.Entities;
using UserRepo.Domain.Interfaces;
using Xunit;

namespace UserRepo.UnitTests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _userService = new UserService(_userRepositoryMock.Object, _passwordServiceMock.Object);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnSuccess_WhenUsernameIsUnique()
    {
        // Arrange
        var request = new CreateUserRequest(
            "jdoe",
            "John Doe",
            "jdoe@example.com",
            "1234567890",
            "Password123!",
            "en",
            "US");

        _userRepositoryMock.Setup(x => x.GetByUserNameAsync(request.UserName))
            .ReturnsAsync((User?)null);

        _passwordServiceMock.Setup(x => x.HashPassword(request.Password))
            .Returns("hashed_password");

        // Act
        var result = await _userService.CreateUserAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(request.UserName, result.Value.UserName);
        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnDuplicateUsernameError_WhenUsernameExists()
    {
        // Arrange
        var request = new CreateUserRequest(
            "jdoe",
            "John Doe",
            "jdoe@example.com",
            "1234567890",
            "Password123!",
            "en",
            "US");

        _userRepositoryMock.Setup(x => x.GetByUserNameAsync(request.UserName))
            .ReturnsAsync(new User { UserName = request.UserName });

        // Act
        var result = await _userService.CreateUserAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(UserErrors.DuplicateUsername, result.Error);
    }

    [Fact]
    public async Task GetUserAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { UserName = "jdoe" };
        // We need to set the Id, but it's set in the constructor of Entity normally. 
        // Since we made the setter public for seeding, we can set it here too.
        user.Id = userId; 

        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserAsync(userId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(userId, result.Value.Id);
    }

    [Fact]
    public async Task GetUserAsync_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _userService.GetUserAsync(userId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(UserErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task ValidatePasswordAsync_ShouldReturnSuccess_WhenCredentialsAreValid()
    {
        // Arrange
        var userName = "jdoe";
        var password = "Password123!";
        var user = new User { UserName = userName, PasswordHash = "hashed_password" };

        _userRepositoryMock.Setup(x => x.GetByUserNameAsync(userName))
            .ReturnsAsync(user);

        _passwordServiceMock.Setup(x => x.VerifyPassword(password, user.PasswordHash))
            .Returns(true);

        // Act
        var result = await _userService.ValidatePasswordAsync(userName, password);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ValidatePasswordAsync_ShouldReturnInvalidCredentials_WhenPasswordIsWrong()
    {
        // Arrange
        var userName = "jdoe";
        var password = "WrongPassword";
        var user = new User { UserName = userName, PasswordHash = "hashed_password" };

        _userRepositoryMock.Setup(x => x.GetByUserNameAsync(userName))
            .ReturnsAsync(user);

        _passwordServiceMock.Setup(x => x.VerifyPassword(password, user.PasswordHash))
            .Returns(false);

        // Act
        var result = await _userService.ValidatePasswordAsync(userName, password);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(UserErrors.InvalidCredentials, result.Error);
    }
}
