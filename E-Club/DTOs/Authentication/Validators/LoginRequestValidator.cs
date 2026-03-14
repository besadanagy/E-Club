
namespace E_Club.DTOs.Auth.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            // Admin validation
            When(x => x.IsAdmin, () =>
            {
                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required for admin login.")
                    .EmailAddress().WithMessage("Invalid email format.");

                RuleFor(x => x.ClubCode)
                    .NotEmpty().WithMessage("Club code is required for admin login.");
            });

            // Member validation
            When(x => !x.IsAdmin, () =>
            {
                RuleFor(x => x.MembershipId)
                    .NotEmpty().WithMessage("Membership ID is required.");

                RuleFor(x => x.SequenceNumber)
                    .NotEmpty().WithMessage("Sequence number is required.");
            });

            // Common validation
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}