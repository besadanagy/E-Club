namespace E_Club.DTOs.Users.Validators;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Current password is required.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required.")
            .Matches(RegexPatterns.PasswordPattern)
            .WithMessage("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")
            .NotEqual(x => x.CurrentPassword)
            .WithMessage("New password must be different from the current password.");

        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty().WithMessage("Please confirm your new password.")
            .Equal(x => x.NewPassword)
            .WithMessage("Password and confirmation do not match.");
    }
}