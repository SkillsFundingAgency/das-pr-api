using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Commands.RemovePermissions;
public class RemovePermissionsCommandValidator : AbstractValidator<RemovePermissionsCommand>
{
    public static readonly string UserRefValidationMessage = "A UserRef must be provided.";
    public const string UkprnValidationMessage = "A ukprn must be provided.";

    public RemovePermissionsCommandValidator(IAccountLegalEntityReadRepository accountLegalEntityReadRepository, IProviderReadRepository providersReadRepository)
    {
        RuleFor(a => a.UserRef)
            .NotEmpty()
            .WithMessage(UserRefValidationMessage);

        RuleFor(x => x.Ukprn)
            .NotEmpty()
            .WithMessage(UkprnValidationMessage);

        RuleFor(x => x.Ukprn)
            .IsValidUkprn(providersReadRepository);

        RuleFor(a => a.AccountLegalEntityId)
            .ValidateAccountLegalEntityExists(accountLegalEntityReadRepository);
    }
}