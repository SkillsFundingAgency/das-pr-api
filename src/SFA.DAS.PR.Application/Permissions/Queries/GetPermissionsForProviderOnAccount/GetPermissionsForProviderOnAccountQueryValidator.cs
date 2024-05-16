using FluentValidation;
using SFA.DAS.PR.Application.Validators;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetPermissionsForProviderOnAccount;
public class GetPermissionsForProviderOnAccountQueryValidator : AbstractValidator<GetPermissionsForProviderOnAccountQuery>
{
    public const string UkprnNotSuppliedValidationMessage = "A Ukprn needs to be supplied";
    public const string LegalEntityPublicHashedIdNotSuppliedValidationMessage = "A Legal entity public hashed Id needs to be supplied";

    public GetPermissionsForProviderOnAccountQueryValidator()
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
