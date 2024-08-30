using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Commands.CreatePermissionRequest;

public class CreatePermissionRequestCommandValidator : AbstractValidator<CreatePermissionRequestCommand>
{
    public static readonly string UkprnValidationMessage = "A Ukprn must be provided.";

    public CreatePermissionRequestCommandValidator(
        IRequestReadRepository requestReadRepository,
        IProviderReadRepository providerReadRepository,
        IAccountLegalEntityReadRepository accountLegalEntityReadRepository,
        IAccountProviderLegalEntitiesReadRepository accountProviderLegalEntitiesReadRepository
    )
    {
        RuleFor(x => x.Ukprn)
            .NotEmpty()
            .WithMessage(UkprnValidator.UkprnValidationMessage);

        RuleFor(x => x.Ukprn)
            .IsValidUkprn(providerReadRepository);

        RuleFor(a => a.AccountLegalEntityId)
            .ValidateAccountLegalEntityExists(accountLegalEntityReadRepository);

        RuleFor(a => new AccountProviderLegalEntityValidationObject { Ukprn = a.Ukprn, AccountLegalEntityId = a.AccountLegalEntityId })
            .ValidateAccountProviderLegalEntityExists(accountProviderLegalEntitiesReadRepository);

        RuleFor(a => new RequestValidationObject() { Ukprn = a.Ukprn, AccountLegalEntityId = a.AccountLegalEntityId, RequestStatuses = new[] { RequestStatus.New, RequestStatus.Sent } })
            .ValidateRequest(requestReadRepository);

        RuleFor(a => a.Operations).ValidateOperationCombinations();
    }
}
