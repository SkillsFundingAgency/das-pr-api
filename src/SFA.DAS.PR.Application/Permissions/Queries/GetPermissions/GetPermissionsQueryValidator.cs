using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Data.Repositories;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;
public class GetPermissionsQueryValidator : AbstractValidator<GetPermissionsQuery>
{
    public const string UkprnValidationMessage = "A Ukprn must be provided";
    public const string AccountLegalEntityIdValidationMessage = "Account Legal entity Id needs to be supplied";

    public GetPermissionsQueryValidator(IProviderReadRepository _providerReadRepository)
    {
        RuleFor(x => x.Ukprn)
            .NotEmpty()
            .WithMessage(UkprnValidationMessage);

        RuleFor(x => x.Ukprn)
            .IsValidUkprn(_providerReadRepository);

        RuleFor(model => model.accountLegalEntityId)
            .NotEmpty()
            .WithMessage(AccountLegalEntityIdValidationMessage);
    }
}