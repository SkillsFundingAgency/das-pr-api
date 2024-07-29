using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Queries.LookupRequests;

public class LookupRequestsQueryValidator : AbstractValidator<LookupRequestsQuery>
{
    public static readonly string PayeValidationMessage = "Paye is required.";
    public LookupRequestsQueryValidator(IProviderReadRepository providerReadRepository, IRequestReadRepository requestReadRepository)
    {
        RuleFor(x => x.Ukprn)
            .NotEmpty()
            .WithMessage(UkprnValidator.UkprnValidationMessage);

        RuleFor(x => x.Paye)
            .NotEmpty()
            .WithMessage(PayeValidationMessage);

        RuleFor(x => x.Ukprn)
            .IsValidUkprn(providerReadRepository);
    }
}
