namespace E_Club.DTOs.Auth.Requests;

public record VerifyResetCodeRequest(
    string PhoneNumber,
    string Code
);