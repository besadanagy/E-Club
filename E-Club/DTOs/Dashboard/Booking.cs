namespace E_Club.DTOs.Dashboard;

public record Booking(
    string BookingId,
    string SportName,
    DateTime BookingDate,
    string Status,
    string? CoachName
);