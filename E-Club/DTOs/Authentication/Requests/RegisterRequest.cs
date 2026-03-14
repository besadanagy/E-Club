namespace E_Club.DTOs.Auth.Requests;

public record RegisterRequest(
    string FullName,
    string Email,
    string Password,
    string PhoneNumber,
    string NationalId,                  // ◀️ إضافة National ID
    bool IsAdmin = false
);