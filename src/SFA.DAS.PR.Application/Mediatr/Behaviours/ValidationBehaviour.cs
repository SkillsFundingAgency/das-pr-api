using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.PR.Application.Mediatr.Responses;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Application.Mediatr.Behaviours
{
    [ExcludeFromCodeCoverage]
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : ValidatedResponse
        where TRequest : IRequest<TResponse>
    {
        private readonly IValidator<TRequest> _compositeValidator;
        private readonly ILogger<ValidationBehaviour<TRequest, TResponse>> _logger;

        public ValidationBehaviour(IValidator<TRequest> compositeValidator, ILogger<ValidationBehaviour<TRequest, TResponse>> logger)
        {
            _compositeValidator = compositeValidator;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Validating AANHub API request");

            var result = await _compositeValidator.ValidateAsync(request, cancellationToken);

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(s => s.ErrorMessage).Aggregate(
                    (acc, current) => acc + string.Concat(' ', current)
                );

                _logger.LogTrace("Validation errors: {Errors}", errors);

                var responseType = typeof(TResponse);

                if (responseType.IsGenericType)
                {
                    var convertedType = typeof(ValidatedResponse<>).MakeGenericType(responseType.GetGenericArguments()[0]);

                    if (Activator.CreateInstance(convertedType, result.Errors) is TResponse invalidResponse)
                    {
                        return invalidResponse;
                    }
                }
            }

            _logger.LogTrace("Validation passed");

            var response = await next();

            return response;
        }
    }
}