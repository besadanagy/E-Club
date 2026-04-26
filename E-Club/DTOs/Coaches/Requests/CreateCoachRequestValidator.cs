namespace E_Club.DTOs.Coaches.Requests;

public class CreateCoachRequestValidator : AbstractValidator<CreateCoachRequest>
{
    public CreateCoachRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(150).WithMessage("Full name must not exceed 150 characters");

        RuleFor(x => x.Specialization)
            .NotEmpty().WithMessage("Specialization is required")
            .MaximumLength(100).WithMessage("Specialization must not exceed 100 characters");

        RuleFor(x => x.ExperienceYears)
            .GreaterThanOrEqualTo(0).WithMessage("Experience years must be 0 or more")
            .LessThanOrEqualTo(50).WithMessage("Experience years must be 50 or less");

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Invalid email format");
    }
}
