namespace E_Club.DTOs.Sports.Responses;

public record ClassBookingResponse(
    int BookingId,
    int ClassId,
    string ClassTitle,
    string SportName,
    string TimeRange,
    string Location,
    DateTime BookedOn,
    string Status
);
