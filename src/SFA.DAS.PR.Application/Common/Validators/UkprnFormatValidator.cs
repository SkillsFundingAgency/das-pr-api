using FluentValidation;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Common.Validators
{
    public static class UkprnFormatValidator
    {
        public const string UkprnFormatValidationMessage = "Currently a Ukprn must start with the value 1 and should be 8 digits long.";

        public static IRuleBuilderOptions<T, long?> CheckUkprnFormat<T>(this IRuleBuilder<T, long?> ruleBuilder) where T : IUkprnEntity
        {
            return ruleBuilder.Must(ukprn => ukprn.ToString()!.StartsWith('1') && ukprn.ToString()!.Length == 8)
                               .WithMessage(UkprnFormatValidationMessage)
                               .When(model => model.Ukprn.HasValue);
        }

        public static IRuleBuilderOptions<T, long?> CheckNullableUkprnFormat<T>(this IRuleBuilder<T, long?> ruleBuilder) where T : INullableUkprnEntity
        {
            return ruleBuilder.Must(ukprn => ukprn.ToString()!.StartsWith('1') && ukprn.ToString()!.Length == 8)
                               .WithMessage(UkprnFormatValidationMessage)
                               .When(model => model.Ukprn.HasValue);
        }
    }
}
