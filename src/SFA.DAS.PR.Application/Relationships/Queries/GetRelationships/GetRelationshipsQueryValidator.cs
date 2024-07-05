using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Relationships.Queries.GetRelationships;

public class GetRelationshipsQueryValidator : AbstractValidator<GetRelationshipsQuery>
{
    public const string UkprnValidationMessage = "A Ukprn must be supplied.";
    public const string AccountLegalEntityIdValidationMessage = "An AccountLegalEntityId must be supplied.";

    public GetRelationshipsQueryValidator(
        IProviderReadRepository providerReadRepository,
        IAccountLegalEntityReadRepository accountLegalEntityReadRepository
    )
    {
        RuleFor(x => x.Ukprn)
            .NotEmpty()
            .WithMessage(UkprnValidationMessage);

        RuleFor(x => x.Ukprn)
            .IsValidUkprn(providerReadRepository);

        RuleFor(a => a.AccountLegalEntityId)
            .ValidateAccountLegalEntityExists(accountLegalEntityReadRepository);
    }
}