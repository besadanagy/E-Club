using E_Club.Services;
using FluentAssertions;
using System.Reflection;
using Xunit;
// Need to mock dependencies but since we're testing private logic methods that don't use dependencies, we can just pass nulls or use Moq if required.
using Moq;
using Microsoft.AspNetCore.Identity;
using E_Club.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using E_Club.Interfaces;
using E_Club.JWT;

using Microsoft.AspNetCore.Identity.UI.Services;

namespace E_Club.Tests.Unit.Services;

public class AuthenticationServiceTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IEmailSender> _emailServiceMock;
    // We assume the constructor has these based on standard identity setup. Let's create an instance.
    
    public AuthenticationServiceTests()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        
        var roleStore = new Mock<IRoleStore<IdentityRole>>();
        _roleManagerMock = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);
        
        _configurationMock = new Mock<IConfiguration>();
        _emailServiceMock = new Mock<IEmailSender>();
    }

    private AuthenticationService CreateService()
    {
        var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(_userManagerMock.Object, 
            new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>().Object, 
            new Mock<Microsoft.AspNetCore.Identity.IUserClaimsPrincipalFactory<ApplicationUser>>().Object, 
            null, null, null, null);

        // Adjust the parameters if your AuthenticationService has different DI constructor injected objects.
        return new AuthenticationService(
            _userManagerMock.Object,
            signInManagerMock.Object,
            new Mock<IJwtProvider>().Object,
            new Mock<Microsoft.Extensions.Options.IOptions<JwtOptions>>().Object,
            _emailServiceMock.Object,
            new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>().Object,
            new Mock<Microsoft.Extensions.Logging.ILogger<AuthenticationService>>().Object,
            null! // ApplicationDbContext can be null for testing private logic functions
        );
    }

    [Fact]
    public void GenerateMembershipId_ShouldReturnCorrectFormat()
    {
        // Arrange
        var service = CreateService();
        var methodInfo = typeof(AuthenticationService).GetMethod("GenerateMembershipId", BindingFlags.NonPublic | BindingFlags.Instance);

        // Act
        var result = (string)methodInfo.Invoke(service, null);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().StartWith($"MEM-{DateTime.Now.Year}-");
        result.Length.Should().Be(13); // MEM-YYYY-XXXX (4+4+1+4 = 13)
    }

    [Fact]
    public void GenerateSequenceNumber_ShouldReturnFourDigits()
    {
        // Arrange
        var service = CreateService();
        var methodInfo = typeof(AuthenticationService).GetMethod("GenerateSequenceNumber", BindingFlags.NonPublic | BindingFlags.Instance);

        // Act
        var result = (string)methodInfo.Invoke(service, null);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Length.Should().Be(4);
        int.TryParse(result, out _).Should().BeTrue();
    }
}
