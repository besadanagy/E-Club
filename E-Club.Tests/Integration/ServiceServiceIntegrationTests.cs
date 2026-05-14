using E_Club.DTOs.Services.Requests;
using E_Club.Models;
using E_Club.Services;
using E_Club.Tests.Configuration;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace E_Club.Tests.Integration;

public class ServiceServiceIntegrationTests : IntegrationTestBase
{
    private readonly E_Club.Interfaces.IServiceService _serviceService;

    public ServiceServiceIntegrationTests(CustomWebApplicationFactory<Program> factory) : base(factory)
    {
        _serviceService = GetService<E_Club.Interfaces.IServiceService>();
    }

    [Fact]
    public async Task CreateServiceAsync_ShouldCreateServiceAndReturnSuccess()
    {
        // Arrange
        var request = new CreateServiceRequest(
            Name: "Test Service",
            Description: "Test Service Desc",
            Icon: "icon.png",
            Endpoint: "test-endpoint",
            ImageUrl: "http://example.com/image.png",
            IsActive: true,
            DisplayOrder: 1,
            Type: "Other"
        );
        var userId = TestUserId;

        // Act
        var result = await _serviceService.CreateServiceAsync(request, userId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be("Test Service");

        var serviceInDb = await DbContext.Services.FirstOrDefaultAsync(s => s.Name == "Test Service");
        serviceInDb.Should().NotBeNull();
        serviceInDb.CreatedById.Should().Be(userId);
    }
}
