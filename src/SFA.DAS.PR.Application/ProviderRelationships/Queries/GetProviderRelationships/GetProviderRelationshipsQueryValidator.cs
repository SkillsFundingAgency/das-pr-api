using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.ProviderRelationships.Queries.GetProviderRelationships;
public class GetProviderRelationshipsQueryValidator : AbstractValidator<GetProviderRelationshipsQuery>
{
    public const string UkprnNotSuppliedValidationMessage = "A Ukprn needs to be supplied";
    public GetProviderRelationshipsQueryValidator(IProviderReadRepository providerReadRepository)
    {
        RuleFor(x => x.Ukprn)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(UkprnNotSuppliedValidationMessage);

        RuleFor(x => x.Ukprn)
            .IsValidUkprn(providerReadRepository);
    }
}
