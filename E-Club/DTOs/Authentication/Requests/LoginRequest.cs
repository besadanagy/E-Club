namespace E_Club.DTOs.Auth.Requests;

public record LoginRequest(
    string? Email,
    string? MembershipId,
    string? SequenceNumber,
    string? ClubCode,
    string Password,
    bool IsAdmin = false   
);