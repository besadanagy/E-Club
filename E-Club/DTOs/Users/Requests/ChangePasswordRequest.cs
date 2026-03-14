namespace E_Club.DTOs.Users.Requests;

public record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword,
    string ConfirmNewPassword  // إضافة ConfirmPassword
);