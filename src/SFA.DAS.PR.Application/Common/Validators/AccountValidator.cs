using FluentValidation;
using SFA.DAS.PR.Domain.Interfaces;

public static class AccountValidator
{
    public const string AccountNotExistValidationMessage = "Account must exist.";
    public const string AccountHashedIdValidationMessage = "An AccountHashedId must be supplied.";
    public static IRuleBuilderOptions<T, string> ValidateAccount<T>(this IRuleBuilder<T, string> ruleBuilder, IAccountReadRepository accountReadRepository) where T : IAccountHashedIdEntity
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage(AccountHashedIdValidationMessage)
            .MustAsync(async (accountHashedId, cancellationToken) =>
            {
                return await accountReadRepository.AccountExists(accountHashedId, cancellationToken);
            })
        .WithMessage(AccountNotExistValidationMessage);
    }
}