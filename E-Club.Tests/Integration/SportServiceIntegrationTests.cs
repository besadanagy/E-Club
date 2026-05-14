using E_Club.DTOs.Sports.Requests;
using E_Club.Models;
using E_Club.Services;
using E_Club.Errors;
using E_Club.Tests.Configuration;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace E_Club.Tests.Integration;

public class SportServiceIntegrationTests : IntegrationTestBase
{
    private readonly E_Club.Interfaces.ISportService _sportService;

    public SportServiceIntegrationTests(CustomWebApplicationFactory<Program> factory) : base(factory)
    {
        _sportService = GetService<E_Club.Interfaces.ISportService>();
    }

    [Fact]
    public async Task CreateClassAsync_WithCoach_ShouldAssignCoach()
    {
        // Arrange
        var sport = new Sport { Name = "Basketball", Icon = "ball" };
        var coach = new Coach { FullName = "Alex Coach", Specialization = "Basketball" };
        DbContext.Sports.Add(sport);
        DbContext.Coaches.Add(coach);
        await DbContext.SaveChangesAsync();

        var request = new CreateSportClassRequest(
            Title: "Basketball Morning Session",
            Description: "Pro drill",
            ImageUrl: null,
            SportId: sport.Id,
            StartTime: DateTime.UtcNow.AddDays(1),
            EndTime: DateTime.UtcNow.AddDays(1).AddHours(1),
            Location: "Main Court",
            MaxParticipants: 10,
            Price: 45.0m,
            Type: "Regular",
            CoachId: coach.Id
        );

        // Act
        var result = await _sportService.CreateClassAsync(request, TestUserId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.CoachId.Should().Be(coach.Id);
        result.Value.CoachName.Should().Be("Alex Coach");

        var classInDb = await DbContext.SportClasses.FindAsync(result.Value.Id);
        classInDb!.CoachId.Should().Be(coach.Id);
    }

    [Fact]
    public async Task GetUpcomingClassesAsync_WithFilters_ShouldReturnFilteredResults()
    {
        // Arrange
        var sport = new Sport { Name = "Tennis", Icon = "tennis" };
        DbContext.Sports.Add(sport);
        await DbContext.SaveChangesAsync();

        var coach1 = new Coach { FullName = "Coach 1", Specialization = "Tennis" };
        var coach2 = new Coach { FullName = "Coach 2", Specialization = "Tennis" };
        DbContext.Coaches.AddRange(coach1, coach2);
        await DbContext.SaveChangesAsync();

        var date1 = DateTime.UtcNow.AddDays(2).Date.AddHours(10);
        var date2 = DateTime.UtcNow.AddDays(3).Date.AddHours(10);

        var class1 = new SportClass { Title = "C1 D1", SportId = sport.Id, CoachId = coach1.Id, StartTime = date1, EndTime = date1.AddHours(1), MaxParticipants = 10, Location = "L1" };
        var class2 = new SportClass { Title = "C2 D1", SportId = sport.Id, CoachId = coach2.Id, StartTime = date1, EndTime = date1.AddHours(1), MaxParticipants = 10, Location = "L1" };
        var class3 = new SportClass { Title = "C1 D2", SportId = sport.Id, CoachId = coach1.Id, StartTime = date2, EndTime = date2.AddHours(1), MaxParticipants = 10, Location = "L1" };

        DbContext.SportClasses.AddRange(class1, class2, class3);
        await DbContext.SaveChangesAsync();

        // Act & Assert 1: Filter by Coach
        var resultCoach1 = await _sportService.GetUpcomingClassesAsync(sport.Id, TestUserId, coachId: coach1.Id);
        resultCoach1.Value.Should().HaveCount(2);
        resultCoach1.Value.Should().AllSatisfy(c => c.CoachId.Should().Be(coach1.Id));

        // Act & Assert 2: Filter by Date
        var resultDate1 = await _sportService.GetUpcomingClassesAsync(sport.Id, TestUserId, date: date1);
        resultDate1.Value.Should().HaveCount(2);
        resultDate1.Value.Should().AllSatisfy(c => c.StartTime.Should().Contain(date1.ToString("hh:mm tt")));

        // Act & Assert 3: Filter by Coach AND Date
        var resultBoth = await _sportService.GetUpcomingClassesAsync(sport.Id, TestUserId, coachId: coach1.Id, date: date2);
        resultBoth.Value.Should().HaveCount(1);
        resultBoth.Value.First().Title.Should().Be("C1 D2");
    }

    [Fact]
    public async Task BookClassAsync_WhenValid_ShouldCreateBooking()
    {
        // Arrange
        var sport = new Sport { Name = "Swimming", Icon = "swim" };
        DbContext.Sports.Add(sport);
        await DbContext.SaveChangesAsync();

        var sportClass = new SportClass 
        { 
            Title = "Morning Swim", 
            SportId = sport.Id, 
            StartTime = DateTime.UtcNow.AddDays(1), 
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(1), 
            MaxParticipants = 5, 
            Location = "Pool",
            Status = ClassStatus.Upcoming
        };
        DbContext.SportClasses.Add(sportClass);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _sportService.BookClassAsync(sportClass.Id, TestUserId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Status.Should().Be("Confirmed");

        var bookingInDb = await DbContext.ClassBookings.FirstOrDefaultAsync(b => b.SportClassId == sportClass.Id && b.UserId == TestUserId);
        bookingInDb.Should().NotBeNull();
        
        var updatedClass = await DbContext.SportClasses.FindAsync(sportClass.Id);
        updatedClass!.CurrentParticipants.Should().Be(1);
    }

    [Fact]
    public async Task BookClassAsync_WhenFull_ShouldReturnFailure()
    {
        // Arrange
        var sport = new Sport { Name = "Gym", Icon = "gym" };
        DbContext.Sports.Add(sport);
        await DbContext.SaveChangesAsync();

        var sportClass = new SportClass 
        { 
            Title = "Yoga", 
            SportId = sport.Id, 
            StartTime = DateTime.UtcNow.AddDays(1), 
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(1), 
            MaxParticipants = 1, 
            CurrentParticipants = 1,
            Location = "Studio",
            Status = ClassStatus.Upcoming
        };
        DbContext.SportClasses.Add(sportClass);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _sportService.BookClassAsync(sportClass.Id, TestUserId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(SportErrors.ClassFull);
    }
}
