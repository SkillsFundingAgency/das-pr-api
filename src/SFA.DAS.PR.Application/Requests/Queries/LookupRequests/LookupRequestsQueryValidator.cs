using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Queries.LookupRequests;

public class LookupRequestsQueryValidator : AbstractValidator<LookupRequestsQuery>
{
    public static readonly string PayeOrEmailValidationMessage = "Paye or Email is required.";
    public LookupRequestsQueryValidator(IProviderReadRepository providerReadRepository)
    {
        RuleFor(x => x.Ukprn)
            .NotEmpty()
            .WithMessage(UkprnValidator.UkprnValidationMessage);

        RuleFor(x => x.Paye)
            .Must(HasPayeOrEmail)
            .WithMessage(PayeOrEmailValidationMessage);

        RuleFor(x => x.Ukprn)
            .IsValidUkprn(providerReadRepository);
    }

    private static bool HasPayeOrEmail(LookupRequestsQuery model, string? paye)
    {
        return !(string.IsNullOrEmpty(paye) && string.IsNullOrEmpty(model.Email));
    }
}
