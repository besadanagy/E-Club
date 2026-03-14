using FluentValidation;

namespace E_Club.DTOs.Auth.Validators;

public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x)
            .Must(x => !string.IsNullOrEmpty(x.Email) || !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("Either Email or Phone Number is required.");

        When(x => !string.IsNullOrEmpty(x.Email), () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format.");
        });

        When(x => !string.IsNullOrEmpty(x.PhoneNumber), () =>
        {
            RuleFor(x => x.PhoneNumber)
                .Matches(@"^01[0-9]{9}$").WithMessage("Invalid Egyptian phone number.");
        });
    }
}