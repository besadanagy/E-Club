namespace E_Club.DTOs.Events.Responses;

public record CountdownResponse(
    int Days,
    int Hours,
    int Minutes,
    int Seconds
);