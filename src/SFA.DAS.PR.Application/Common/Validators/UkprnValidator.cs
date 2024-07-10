using FluentValidation;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Common.Validators
{
    public static class UkprnValidator
    {
        public const string UkprnFormatValidationMessage = "Currently a Ukprn must start with the value 1 and should be 8 digits long.";

        public const string ProviderEntityExistValidationMessage = "Provider must exist.";

        private static IRuleBuilderOptions<T, long?> IsValidUkprnFormat<T>(this IRuleBuilder<T, long?> ruleBuilder) where T : IUkprnEntity
        {
            return ruleBuilder
                .Must(ukprn => ukprn.ToString()!.StartsWith('1') && ukprn.ToString()!.Length == 8)
                .WithMessage(UkprnFormatValidationMessage)
            .When(model => model.Ukprn.HasValue);
        }

        public static IRuleBuilderOptions<T, long?> IsValidUkprn<T>(this IRuleBuilder<T, long?> ruleBuilder, IProviderReadRepository providerReadRepository) where T : IUkprnEntity
        {
            return ruleBuilder
                .IsValidUkprnFormat()
                .MustAsync(async (ukprn, cancellationToken) =>
                {
                    return await providerReadRepository.ProviderExists(ukprn!.Value, cancellationToken);
                })
                .WithMessage(ProviderEntityExistValidationMessage)
            .When(model => model.Ukprn.HasValue);
        }
    }
}