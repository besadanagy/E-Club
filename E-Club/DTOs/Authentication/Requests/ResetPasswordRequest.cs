namespace E_Club.DTOs.Auth.Requests;

public record ResetPasswordRequest(
    string Token,                  
    string NewPassword,            
    string? Email = null,          
    string? PhoneNumber = null     
);