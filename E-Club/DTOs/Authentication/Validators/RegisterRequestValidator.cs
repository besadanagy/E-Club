using FluentValidation;
using E_Club.Consts;
using E_Club.DTOs.Auth.Requests;

namespace E_Club.DTOs.Auth.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .Must(BeValidFullName).WithMessage("Full name must contain at least first and last name");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .Matches(RegexPatterns.PasswordPattern)
            .WithMessage("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\+?[0-9]{10,15}$").WithMessage("Invalid phone number format");

        When(x => x.IsAdmin, () =>
        {
            // ◀️ لو عايز تضيف validation خاصة بالـ Admin
        });
        RuleFor(x => x.NationalId)
    .NotEmpty().WithMessage("National ID is required")
    .Length(14).WithMessage("National ID must be 14 digits")
    .Matches(@"^[0-9]{14}$").WithMessage("National ID must contain only numbers");
    }

    private bool BeValidFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return false;

        var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 2;  // لازم على الأقل اسم أول واسم عائلة
    }
}