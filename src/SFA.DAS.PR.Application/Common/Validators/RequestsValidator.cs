using FluentValidation;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Common.Validators;

public static class RequestsValidator
{
    public const string RequestValidationMessage = "A 'new' or 'sent' request for this AccountLegalEntityId and Ukprn combination already exists.";
    public const string RequestEmployerPAYEValidationMessage = "A 'new' or 'sent' request for this EmployerPAYE and Ukprn combination already exists.";
    public static IRuleBuilderOptions<T, RequestValidationObject> ValidateRequest<T>(this IRuleBuilder<T, RequestValidationObject> ruleBuilder, IRequestReadRepository requestReadRepository) where T : IUkprnEntity, IAccountLegalEntityIdEntity
    {
        return ruleBuilder
            .MustAsync(async (requestValidationObject, cancellationToken) =>
            {
                return !await requestReadRepository.RequestExists(
                    requestValidationObject.Ukprn!.Value, 
                    requestValidationObject.AccountLegalEntityId,
                    requestValidationObject.RequestStatuses,
                    cancellationToken
                );
            })
            .WithMessage(RequestValidationMessage);
    }

    public static IRuleBuilderOptions<T, EmployerPayeRequestObject> ValidateRequest<T>(this IRuleBuilder<T, EmployerPayeRequestObject> ruleBuilder, IRequestReadRepository requestReadRepository) where T : IUkprnEntity
    {
        return ruleBuilder
            .MustAsync(async (requestValidationObject, cancellationToken) =>
            {
                return !await requestReadRepository.RequestExists(
                    requestValidationObject.Ukprn!.Value,
                    requestValidationObject.EmployerPAYE!,
                    requestValidationObject.RequestStatuses,
                    cancellationToken
                );
            })
            .WithMessage(RequestEmployerPAYEValidationMessage);
    }

    public static IRuleBuilderOptions<T, RequestIdValidationObject> ValidateRequest<T>(this IRuleBuilder<T, RequestIdValidationObject> ruleBuilder, IRequestReadRepository requestReadRepository) where T : IRequestEntity
    {
        return ruleBuilder
            .MustAsync(async (requestIdValidationObject, cancellationToken) =>
            {
                return !await requestReadRepository.RequestExists(
                    requestIdValidationObject.RequestId,
                    requestIdValidationObject.RequestStatuses,
                    requestIdValidationObject.RequestType,
                    cancellationToken
                );
            })
            .WithMessage(RequestValidationMessage);
    }
}
