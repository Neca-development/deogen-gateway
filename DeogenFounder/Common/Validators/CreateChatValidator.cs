using DeogenFounder.Common.DTO.Chats;
using FluentValidation;

namespace DeogenFounder.Common.Validators;

public class CreateChatValidator : AbstractValidator<CreateChatDto>
{
    public CreateChatValidator()
    {
        RuleForEach(x => x.AgentsIds)
            .GreaterThan(0);

        RuleFor(x => x.AgentsIds)
            .Must(x => x.Length >= 2);

        RuleFor(x => x.AgentsIds)
            .Must(x => x.Length == x.Distinct().Count());
    }
}
