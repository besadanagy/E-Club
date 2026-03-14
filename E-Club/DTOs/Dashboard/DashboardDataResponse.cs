
namespace E_Club.DTOs.Dashboard;

public record DashboardDataResponse(
    string MembershipType,
    string ClubName,
    int DigitalAccessKey,
    List<UpcomingMatch> UpcomingMatches,
    List<Booking> RecentBookings
);