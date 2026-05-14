using E_Club.DTOs.Users.Requests;  // CreateUserRequest, UpdateUserRequest
using E_Club.Interfaces;
using E_Club.Tests.Configuration;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace E_Club.Tests.Integration;

public class UserServiceIntegrationTests : IntegrationTestBase
{
    private readonly IUserService _userService;

    public UserServiceIntegrationTests(CustomWebApplicationFactory<Program> factory) : base(factory)
    {
        _userService = GetService<IUserService>();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        // Arrange - نتأكد إن في مستخدمين موجودين
        var users = await DbContext.Users.ToListAsync();
        users.Count.Should().BeGreaterThan(0); // على الأقل Admin موجود

        // Act
        var result = await _userService.GetAllAsync(CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(users.Count);
    }

    [Fact]
    public async Task GetAsync_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        var existingUser = await DbContext.Users.FirstOrDefaultAsync();
        existingUser.Should().NotBeNull();

        // Act
        var result = await _userService.GetAsync(existingUser.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(existingUser.Id);
        result.Value.Email.Should().Be(existingUser.Email);
    }

    [Fact]
    public async Task GetAsync_WhenUserDoesNotExist_ShouldReturnFailure()
    {
        // Act
        var result = await _userService.GetAsync("non-existent-id-12345");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("User.NotFound");
    }

    [Fact]
    public async Task CreateUserAsync_ShouldCreateUserAndReturnSuccess()
    {
        // Arrange
        var request = new CreateUserRequest(
            FirstName: "Test",
            LastName: "User",
            Email: "testuser@eclub.com",
            Password: "Test@123456",
            Roles: new List<string> { "Member" }
        );

        // Act
        var result = await _userService.AddAsync(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Email.Should().Be("testuser@eclub.com");
        result.Value.FirstName.Should().Be("Test");

        var userInDb = await DbContext.Users.FirstOrDefaultAsync(u => u.Email == "testuser@eclub.com");
        userInDb.Should().NotBeNull();
        userInDb.FirstName.Should().Be("Test");
    }

    [Fact]
    public async Task UpdateAsync_WhenUserExists_ShouldUpdateUser()
    {
        // Arrange - نضيف مستخدم جديد أولاً
        var createRequest = new CreateUserRequest(
            FirstName: "Old",
            LastName: "Name",
            Email: "update@eclub.com",
            Password: "Test@123456",
            Roles: new List<string> { "Member" }
        );
        var createResult = await _userService.AddAsync(createRequest, CancellationToken.None);
        createResult.IsSuccess.Should().BeTrue();

        var updateRequest = new UpdateUserRequest(
            FirstName: "New",
            LastName: "Name",
            Email: "update@eclub.com",
            Roles: new List<string> { "Member" }
        );

        // Act
        var result = await _userService.UpdateAsync(createResult.Value.Id, updateRequest, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var updatedUser = await DbContext.Users.FindAsync(createResult.Value.Id);
        updatedUser.FirstName.Should().Be("New");
        updatedUser.LastName.Should().Be("Name");
    }

    [Fact]
    public async Task ToggleStatusAsync_WhenUserExists_ShouldToggleIsDisabled()
    {
        // Arrange - نضيف مستخدم جديد أولاً
        var createRequest = new CreateUserRequest(
            FirstName: "Toggle",
            LastName: "Test",
            Email: "toggle@eclub.com",
            Password: "Test@123456",
            Roles: new List<string> { "Member" }
        );
        var createResult = await _userService.AddAsync(createRequest, CancellationToken.None);
        createResult.IsSuccess.Should().BeTrue();

        var initialStatus = createResult.Value.IsDisabled;

        // Act
        var result = await _userService.ToggleStatusAsync(createResult.Value.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var toggledUser = await DbContext.Users.FindAsync(createResult.Value.Id);
        toggledUser.IsDisabled.Should().Be(!initialStatus);
    }
}