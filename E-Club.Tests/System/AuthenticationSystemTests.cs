using E_Club.DTOs.Auth.Requests;
using E_Club.Tests.Configuration;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace E_Club.Tests.System;

public class AuthenticationSystemTests : IntegrationTestBase
{
    public AuthenticationSystemTests(CustomWebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task Register_WithValidData_ShouldReturnOk()
    {
        // Arrange
        var request = new RegisterRequest(
            FullName: "System Test User",
            Email: "systemtest@example.com",
            Password: "Password123!",
            PhoneNumber: "0123456789",
            NationalId: "12345678901234",
            IsAdmin: false
        );

        // Act
        var response = await Client.PostAsJsonAsync("/auth/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ShouldReturnProblem()
    {
        // Arrange
        var request = new LoginRequest(
            Email: "nonexistent@example.com",
            MembershipId: null,
            SequenceNumber: null,
            ClubCode: null,
            Password: "WrongPassword123!",
            IsAdmin: false
        );

        // Act
        var response = await Client.PostAsJsonAsync("/auth/login", request);

        // Assert
        response.StatusCode.Should().NotBe(HttpStatusCode.OK);
    }
}
