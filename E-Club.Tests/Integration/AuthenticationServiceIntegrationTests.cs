using E_Club.Services;
using E_Club.Tests.Configuration;
using FluentAssertions;
using Xunit;

namespace E_Club.Tests.Integration;

public class AuthenticationServiceIntegrationTests : IntegrationTestBase
{
    private readonly E_Club.Interfaces.IAuthenticationService _authenticationService;

    public AuthenticationServiceIntegrationTests(CustomWebApplicationFactory<Program> factory) : base(factory)
    {
        _authenticationService = GetService<E_Club.Interfaces.IAuthenticationService>();
    }

    // Add integration tests here for AuthenticationService (Login, Register)
}
