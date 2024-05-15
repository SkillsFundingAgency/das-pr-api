using FluentValidation;
using SFA.DAS.PR.Application.Validators;

namespace SFA.DAS.PR.Application.AccountProviders.Queries.GetPermissions;
public class GetPermissionsQueryValidator : AbstractValidator<GetPermissionsQuery>
{
    public const string UkprnNotSuppliedValidationMessage = "A Ukprn needs to be supplied";
    public const string LegalEntityPublicHashedIdNotSuppliedValidationMessage = "A Legal entity public hashed Id needs to be supplied";

    public GetPermissionsQueryValidator()
    {
        RuleFor(x => x.Ukprn)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(UkprnNotSuppliedValidationMessage);

        RuleFor(x => x.Ukprn)
            .CheckUkprnFormat();

        RuleFor(model => model.PublicHashedId)
            .NotEmpty()
            .WithMessage(LegalEntityPublicHashedIdNotSuppliedValidationMessage);
    }
}
