namespace E_Club.DTOs.Roles;

public class RoleRequestValidator : AbstractValidator<RoleRequest>
{
    public RoleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Role name is required.")
            .Length(3, 256).WithMessage("Role name must be between 3 and 256 characters.");

        When(x => x.Permissions != null && x.Permissions.Any(), () =>
        {
            RuleFor(x => x.Permissions)
                .Must(permissions => permissions!.Distinct().Count() == permissions!.Count())
                .WithMessage("Cannot add duplicate permissions for the same role.");
        });
    }
}