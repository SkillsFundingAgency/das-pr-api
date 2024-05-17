
using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetHasPermissions;
public class GetHasPermissionsQueryValidator : AbstractValidator<GetHasPermissionsQuery>
{
    public const string UkprnNotSuppliedValidationMessage = "A Ukprn needs to be supplied";
    public const string AccountLegalEntityIdNotSuppliedValidationMessage = "An Account Legal entity Id needs to be supplied";

    public GetHasPermissionsQueryValidator()
    {
        RuleFor(x => x.Ukprn)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(UkprnNotSuppliedValidationMessage);

        RuleFor(x => x.Ukprn)
            .CheckUkprnFormat();

        RuleFor(model => model.AccountLegalEntityId)
            .NotEmpty()
            .WithMessage(AccountLegalEntityIdNotSuppliedValidationMessage);

        RuleFor(model => model.Operations)!.ContainsValidOperations();
    }

}
