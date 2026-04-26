namespace E_Club.DTOs.Sports.Responses;

public record SportsScreenResponse(
    List<SportResponse> Sports,
    List<SportClassResponse> UpcomingClasses,
    SportClassResponse? SpecialEvent
);
