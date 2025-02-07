using DeogenFounder.Common.DTO.Chats;
using FluentValidation;

namespace DeogenFounder.Common.Validators;

public class SendMessageValidator : AbstractValidator<SendMessageDto>
{
    public SendMessageValidator()
    {
        RuleFor(x => x.ChatId)
            .GreaterThan(0);

        RuleFor(x => x.Message)
            .NotEmpty()
            .MaximumLength(1000);
    }
}
