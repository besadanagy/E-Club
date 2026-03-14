namespace E_Club.DTOs.Auth.Requests;

public record ForgotPasswordRequest(
    string? Email = null,
    string? PhoneNumber = null
);