using FluentValidation;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Common.Validators;

public static class ProviderValidator
{
    public const string ProviderExistValidationMessage = "Ukprn not found.";
    public const string UkprnValidationMessage = "A Ukprn must be provided.";

    public static IRuleBuilderOptions<T, long?> ValidateProviderExists<T>(this IRuleBuilderInitial<T, long?> ruleBuilder, IProvidersReadRepository providersReadRepository) where T : IProviderUkprn
    {
        return ruleBuilder
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(UkprnValidationMessage)
            .MustAsync(providersReadRepository.ProviderExists)
            .WithMessage(ProviderExistValidationMessage);
    }
}