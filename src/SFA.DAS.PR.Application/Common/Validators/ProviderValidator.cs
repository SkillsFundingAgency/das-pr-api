using FluentValidation;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Common.Validators;

public static class ProviderValidator
{
    public const string ProviderEntityExistValidationMessage = "Provider must exist.";
    public const string UkprnValidationMessage = "A Ukprn must be supplied.";
    public static IRuleBuilderOptions<T, long?> ValidatProviderExists<T>(this IRuleBuilderInitial<T, long?> ruleBuilder, IProviderReadRepository providerReadRepository) where T : IUkprnEntity
    {
        return ruleBuilder
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage(UkprnValidationMessage)
            .MustAsync(async (ukprn, cancellationToken) =>
            {
                return await providerReadRepository.ProviderExists(ukprn!.Value, cancellationToken);
            })
            .WithMessage(ProviderEntityExistValidationMessage);
    }
}
