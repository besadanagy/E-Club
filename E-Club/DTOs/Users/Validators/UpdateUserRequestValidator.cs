namespace E_Club.DTOs.Users.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .Length(3, 100).WithMessage("First name must be between 3 and 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Length(3, 100).WithMessage("Last name must be between 3 and 100 characters.");

        RuleFor(x => x.Roles)
            .NotEmpty().WithMessage("At least one role is required.")
            .Must(roles => roles.Distinct().Count() == roles.Count)
            .WithMessage("Cannot add duplicate roles for the same user.");
    }
}