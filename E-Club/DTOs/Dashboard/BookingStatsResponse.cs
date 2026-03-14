namespace E_Club.DTOs.Dashboard;

public record BookingStatsResponse(
    int TotalBookings,
    int TodayBookings,
    int UpcomingBookings,
    List<SportBookingStats> SportStats
);

public record SportBookingStats(
    string SportName,
    int BookingCount,
    int AvailableSlots
);