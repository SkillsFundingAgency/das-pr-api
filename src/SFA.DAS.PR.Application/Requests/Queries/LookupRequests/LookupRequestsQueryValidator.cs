using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Queries.LookupRequests;

public class LookupRequestsQueryValidator : AbstractValidator<LookupRequestsQuery>
{
    public static readonly string PayeOrEmailOrAccountLegalEntityIdValidationMessage = "Paye or Email or AccountLegalEntityId is required.";
    public LookupRequestsQueryValidator(IProviderReadRepository providerReadRepository)
    {
        RuleFor(x => x.Ukprn)
            .NotEmpty()
            .WithMessage(UkprnValidator.UkprnValidationMessage);

        RuleFor(x => x.Paye)
            .Must(HasPayeOrEmailOrAccountLegalEntityId)
            .WithMessage(PayeOrEmailOrAccountLegalEntityIdValidationMessage);

        RuleFor(x => x.Ukprn)
            .IsValidUkprn(providerReadRepository);
    }

    private static bool HasPayeOrEmailOrAccountLegalEntityId(LookupRequestsQuery model, string? paye)
    {
        return !(string.IsNullOrEmpty(paye) && string.IsNullOrEmpty(model.Email) && model.AccountLegalEntityId == null);
    }
}
