namespace E_Club.DTOs.Users.Requests;

public record UpdateProfileRequest(
    string FirstName,
    string LastName,
    string Email,           // إضافة Email
    string? PhoneNumber = null  // إضافة PhoneNumber اختياري
);