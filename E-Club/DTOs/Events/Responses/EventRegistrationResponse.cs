namespace E_Club.DTOs.Events.Responses;

public record EventRegistrationResponse(
    int RegistrationId,
    int EventId,
    string EventTitle,
    DateTime RegisteredOn,
    string Status
);
