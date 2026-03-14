namespace E_Club.DTOs.Auth.Requests;

public record RefreshTokenRequest(
    string Token,
    string RefreshToken
);