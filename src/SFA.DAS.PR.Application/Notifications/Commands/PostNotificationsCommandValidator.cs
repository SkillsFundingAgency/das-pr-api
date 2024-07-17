using FluentValidation;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Notifications.Commands;

public class PostNotificationsCommandValidator : AbstractValidator<PostNotificationsCommand>
{
    public PostNotificationsCommandValidator(IProviderReadRepository _providerReadRepository)
    {
        RuleForEach(x => x.Notifications).SetValidator(new NotificationModelValidator(_providerReadRepository));
    }
}
