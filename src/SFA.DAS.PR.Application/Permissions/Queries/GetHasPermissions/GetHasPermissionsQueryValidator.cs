
using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetHasPermissions;
public class GetHasPermissionsQueryValidator : AbstractValidator<GetHasPermissionsQuery>
{
    public const string UkprnNotSuppliedValidationMessage = "A Ukprn needs to be supplied";
    public const string AccountLegalEntityIdNotSuppliedValidationMessage = "An Account Legal entity Id needs to be supplied";

    public GetHasPermissionsQueryValidator(IProviderReadRepository providerReadRepository)
    {
        RuleFor(x => x.Ukprn)
            .NotEmpty()
            .WithMessage(UkprnNotSuppliedValidationMessage);

        RuleFor(x => x.Ukprn)
            .IsValidUkprn(providerReadRepository);

        RuleFor(model => model.AccountLegalEntityId)
            .NotEmpty()
            .WithMessage(AccountLegalEntityIdNotSuppliedValidationMessage);

        RuleFor(model => model.Operations)!.ContainsValidOperations();
    }
}
