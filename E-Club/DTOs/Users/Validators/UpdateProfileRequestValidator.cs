namespace E_Club.DTOs.Users.Validators;

public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .Length(3, 100).WithMessage("First name must be between 3 and 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Length(3, 100).WithMessage("Last name must be between 3 and 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(15).WithMessage("Phone number cannot exceed 15 characters.")
            .Matches(@"^\+?[0-9]*$").WithMessage("Invalid phone number format.")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }
}