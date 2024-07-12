using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Commands.PostPermissions;

public class PostPermissionsCommandValidator : AbstractValidator<PostPermissionsCommand>
{
    public static readonly string UserRefValidationMessage = "A UserRef must be provided.";
    public static readonly string UkprnValidationMessage = "A Ukprn must be provided.";
    public static readonly string AccountLegalEntityIdValidationMessage = "An AccountLegalEntityId must be provided.";

    public PostPermissionsCommandValidator(IAccountLegalEntityReadRepository accountLegalEntityReadRepository, IProviderReadRepository providerReadRepository)
    {
        RuleFor(a => a.UserRef)
            .NotEmpty()
            .WithMessage(UserRefValidationMessage);

        RuleFor(x => x.Ukprn)
            .NotEmpty()
            .WithMessage(UkprnValidationMessage);

        RuleFor(x => x.Ukprn)
            .IsValidUkprn(providerReadRepository);

        RuleFor(a => a.AccountLegalEntityId)
            .ValidateAccountLegalEntityExists(accountLegalEntityReadRepository);

        RuleFor(a => a.Operations).ValidateOperationCombinations();
    }
}
