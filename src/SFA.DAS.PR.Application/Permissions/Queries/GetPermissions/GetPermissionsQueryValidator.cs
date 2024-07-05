using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;
public class GetPermissionsQueryValidator : AbstractValidator<GetPermissionsQuery>
{
    public const string UkprnNotSuppliedValidationMessage = "A Ukprn needs to be supplied";
    public const string LegalEntityIdNotSuppliedValidationMessage = "Account Legal entity Id needs to be supplied";

    public GetPermissionsQueryValidator(IProviderReadRepository providerReadRepository)
    {
        RuleFor(x => x.Ukprn)
            .NotEmpty()
            .WithMessage(UkprnNotSuppliedValidationMessage);

        RuleFor(x => x.Ukprn)
            .IsValidUkprn(providerReadRepository);

        RuleFor(model => model.accountLegalEntityId)
            .NotEmpty()
            .WithMessage(LegalEntityIdNotSuppliedValidationMessage);
    }
}