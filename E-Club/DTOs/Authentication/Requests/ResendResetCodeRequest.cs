namespace E_Club.DTOs.Auth.Requests;

public record ResendResetCodeRequest(
    string PhoneNumber
);