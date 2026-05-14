using E_Club.DTOs.Events.Requests;
using E_Club.Tests.Configuration;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace E_Club.Tests.System;

public class EventSystemTests : IntegrationTestBase
{
    public EventSystemTests(CustomWebApplicationFactory<Program> factory) : base(factory)
    {
        // بنخلي الـ Client يستخدم الـ Test Auth Scheme
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
    }

    [Fact]
    public async Task CreateEvent_AsAdmin_ShouldReturnCreated()
    {
        // Arrange
        var request = new CreateEventRequest(
            Title: "New API Event",
            Description: "Created via System Test",
            Location: "Club Hall",
            StartDate: DateTime.UtcNow.AddDays(1),
            EndDate: DateTime.UtcNow.AddDays(2),
            ImageUrl: "http://example.com/event.jpg",
            MaxParticipants: 50
        );

        // Act
        var response = await Client.PostAsJsonAsync("/api/events", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var eventDto = await response.Content.ReadFromJsonAsync<JsonElement>();
        eventDto.GetProperty("title").GetString().Should().Be(request.Title);
    }

    [Fact]
    public async Task GetUpcomingEvents_ShouldReturnOk()
    {
        // Act
        var response = await Client.GetAsync("/api/events/upcoming");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
