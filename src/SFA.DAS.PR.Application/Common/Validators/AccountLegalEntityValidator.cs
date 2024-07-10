using FluentValidation;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Common.Validators;

public static class AccountLegalEntityValidator
{
    public const string AccountLegalEntityExistValidationMessage = "AccountLegalEntity must exist.";
    public const string AccountLegalEntityIdValidationMessage = "An AccountLegalEntityId must be supplied.";
    public static IRuleBuilderOptions<T, long> ValidateAccountLegalEntityExists<T>(this IRuleBuilderInitial<T, long> ruleBuilder, IAccountLegalEntityReadRepository accountLegalEntityReadRepository) where T : IAccountLegalEntityIdEntity
    {
        return ruleBuilder
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(AccountLegalEntityIdValidationMessage)
            .MustAsync(async (accountLegalEntityId, cancellationToken) =>
            {
                return await accountLegalEntityReadRepository.AccountLegalEntityExists(accountLegalEntityId, cancellationToken);
            })
            .WithMessage(AccountLegalEntityExistValidationMessage);
    }

    public static IRuleBuilderOptions<T, long?> ValidateAccountLegalEntityExists<T>(this IRuleBuilderInitial<T, long?> ruleBuilder, IAccountLegalEntityReadRepository accountLegalEntityReadRepository) where T : IAccountLegalEntityNullableIdEntity
    {
        return ruleBuilder
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage(AccountLegalEntityIdValidationMessage)
            .MustAsync(async (accountLegalEntityId, cancellationToken) =>
            {
                return await accountLegalEntityReadRepository.AccountLegalEntityExists(accountLegalEntityId!.Value, cancellationToken);
            })
            .WithMessage(AccountLegalEntityExistValidationMessage);
    }
}