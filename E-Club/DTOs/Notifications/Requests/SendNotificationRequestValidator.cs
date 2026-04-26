namespace E_Club.DTOs.Notifications.Requests;

public class SendNotificationRequestValidator : AbstractValidator<SendNotificationRequest>
{
    public SendNotificationRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("Body is required")
            .MaximumLength(1000).WithMessage("Body must not exceed 1000 characters");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid notification type");
    }
}
