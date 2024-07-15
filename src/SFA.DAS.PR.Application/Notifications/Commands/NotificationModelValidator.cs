using FluentValidation;
using FluentValidation.Validators;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.PR.Domain.Models;

namespace SFA.DAS.PR.Application.Notifications.Commands;

public class NotificationModelValidator : AbstractValidator<NotificationModel>
{
    public const string EmailAddressValidationMessage = "Email address must be in a valid format";
    public NotificationModelValidator(IProviderReadRepository _providerReadRepository)
    {
        RuleFor(x => x.NotificationType)
            .ValidNotificationType();

        RuleFor(x => x.Ukprn)
            .IsValidUkprn(_providerReadRepository);

        RuleFor(x => x.EmailAddress)
            .EmailAddress(EmailValidationMode.AspNetCoreCompatible)
            .When(x => !string.IsNullOrWhiteSpace(x.EmailAddress))
            .WithMessage(EmailAddressValidationMessage);
    }
}
