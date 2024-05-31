using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;
public class GetPermissionsQueryValidator : AbstractValidator<GetPermissionsQuery>
{
    public const string UkprnNotSuppliedValidationMessage = "A Ukprn needs to be supplied";
    public const string LegalEntityIdNotSuppliedValidationMessage = "Account Legal entity Id needs to be supplied";

    public GetPermissionsQueryValidator()
    {
        RuleFor(x => x.Ukprn)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(UkprnNotSuppliedValidationMessage);

        RuleFor(x => x.Ukprn)
            .CheckUkprnFormat();

        RuleFor(model => model.accountLegalEntityId)
            .NotEmpty()
            .WithMessage(LegalEntityIdNotSuppliedValidationMessage);
    }
}
