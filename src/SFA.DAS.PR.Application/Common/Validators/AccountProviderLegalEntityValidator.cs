using FluentValidation;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Common.Validators;

public static class AccountProviderLegalEntityValidator
{
    public const string AccountProviderLegalEntityExistValidationMessage = "AccountProviderLegalEntity must exist.";

    public static IRuleBuilderOptions<T, AccountProviderLegalEntityValidationObject> ValidateAccountProviderLegalEntityExists<T>(
        this IRuleBuilderInitial<T, AccountProviderLegalEntityValidationObject> ruleBuilder,
        IAccountProviderLegalEntitiesReadRepository accountProviderLegalEntitiesReadRepository
    ) where T : IAccountLegalEntityIdEntity, IUkprnEntity
    {
        return ruleBuilder
            .MustAsync(async (validationObject, cancellationToken) =>
            {
                return await accountProviderLegalEntitiesReadRepository.AccountProviderLegalEntityExists(
                    validationObject.Ukprn!.Value, 
                    validationObject.AccountLegalEntityId, 
                    cancellationToken
                );
            })
            .WithMessage(AccountProviderLegalEntityExistValidationMessage);
    }
}
