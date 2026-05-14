using E_Club.DTOs.Events.Requests;
using E_Club.Models;
using E_Club.Services;
using E_Club.Tests.Configuration;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace E_Club.Tests.Integration;

public class EventServiceIntegrationTests : IntegrationTestBase
{
    private readonly E_Club.Interfaces.IEventService _eventService;

    public EventServiceIntegrationTests(CustomWebApplicationFactory<Program> factory) : base(factory)
    {
        _eventService = GetService<E_Club.Interfaces.IEventService>();
    }

    [Fact]
    public async Task CreateEventAsync_ShouldCreateEventAndReturnSuccess()
    {
        // Arrange
        var request = new CreateEventRequest(
            Title: "Test Event",
            Description: "Test Event Description",
            Location: "Cairo",
            StartDate: DateTime.UtcNow.AddDays(10),
            EndDate: DateTime.UtcNow.AddDays(11),
            ImageUrl: "http://example.com/image.jpg",
            MaxParticipants: 100
        );
        var userId = TestUserId;

        // Act
        var result = await _eventService.CreateEventAsync(request, userId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Title.Should().Be("Test Event");

        var eventInDb = await DbContext.Events.FirstOrDefaultAsync(e => e.Title == "Test Event");
        eventInDb.Should().NotBeNull();
        eventInDb.CreatedById.Should().Be(userId);
    }

    [Fact]
    public async Task GetEventByIdAsync_WhenEventExists_ShouldReturnEvent()
    {
        // Arrange
        var @event = new Event
        {
            Title = "Existing Event",
            Description = "Desc",
            Location = "Alex",
            StartDate = DateTime.UtcNow.AddDays(5),
            EndDate = DateTime.UtcNow.AddDays(6),
            MaxParticipants = 50,
            Status = EventStatus.Upcoming,
            CreatedById = TestUserId,
            CreatedOn = DateTime.UtcNow
        };
        DbContext.Events.Add(@event);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _eventService.GetEventByIdAsync(@event.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(@event.Id);
        result.Value.Title.Should().Be("Existing Event");
    }

    [Fact]
    public async Task UpdateEventAsync_WhenEventExists_ShouldUpdateEvent()
    {
        // Arrange
        var @event = new Event
        {
            Title = "Old Event Title",
            Description = "Old Desc",
            Location = "Giza",
            StartDate = DateTime.UtcNow.AddDays(5),
            EndDate = DateTime.UtcNow.AddDays(6),
            MaxParticipants = 50,
            Status = EventStatus.Upcoming,
            CreatedById = TestUserId,
            CreatedOn = DateTime.UtcNow
        };
        DbContext.Events.Add(@event);
        await DbContext.SaveChangesAsync();

        var updateRequest = new CreateEventRequest(
            Title: "Updated Event Title",
            Description: "Updated Desc",
            Location: "Giza",
            StartDate: DateTime.UtcNow.AddDays(5),
            EndDate: DateTime.UtcNow.AddDays(6),
            ImageUrl: null,
            MaxParticipants: 100
        );

        // Act
        var result = await _eventService.UpdateEventAsync(@event.Id, updateRequest);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var updatedEvent = await DbContext.Events.FindAsync(@event.Id);
        updatedEvent.Title.Should().Be("Updated Event Title");
        updatedEvent.MaxParticipants.Should().Be(100);
    }

    [Fact]
    public async Task DeleteEventAsync_WhenEventExists_ShouldRemoveEvent()
    {
        // Arrange
        var @event = new Event
        {
            Title = "Event To Delete",
            Description = "Desc",
            Location = "Aswan",
            StartDate = DateTime.UtcNow.AddDays(2),
            EndDate = DateTime.UtcNow.AddDays(3),
            MaxParticipants = 50,
            Status = EventStatus.Upcoming,
            CreatedById = TestUserId,
            CreatedOn = DateTime.UtcNow
        };
        DbContext.Events.Add(@event);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _eventService.DeleteEventAsync(@event.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var deletedEvent = await DbContext.Events.FindAsync(@event.Id);
        deletedEvent.Should().BeNull();
    }
}
