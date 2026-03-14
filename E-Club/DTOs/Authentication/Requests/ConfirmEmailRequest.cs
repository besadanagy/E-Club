namespace E_Club.DTOs.Auth.Requests;

public record ConfirmEmailRequest(
    string UserId,
    string Code
);