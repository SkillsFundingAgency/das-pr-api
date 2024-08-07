﻿using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PR.Application.Common.Commands;
using SFA.DAS.PR.Application.Mediatr.Responses;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Api.Common
{
    [ExcludeFromCodeCoverage]
    public abstract class ActionResponseControllerBase : ControllerBase
    {
        public const string GetMethodName = "Get";
        public const string PutMethodName = "Put";

        public abstract string ControllerName { get; }

        protected IActionResult GetResponse<T>(ValidatedResponse<T> response)
        {
            if (response.Result == null && response.IsValidResponse)
            {
                return NotFound();
            }

            if (response.IsValidResponse)
            {
                return new OkObjectResult(response.Result);
            }
                
            return new BadRequestObjectResult(FormatErrors(response.Errors));
        }

        protected IActionResult GetPostResponse<T>(ValidatedResponse<T> response)
        {
            if (response.IsValidResponse)
            {
                return Created();
            }

            return new BadRequestObjectResult(FormatErrors(response.Errors));
        }

        protected IActionResult GetPutResponse(ValidatedResponse<SuccessCommandResult> response)
        {
            return response.IsValidResponse ? 
                NoContent() : 
                new BadRequestObjectResult(FormatErrors(response.Errors));
        }

        protected IActionResult GetDeleteResponse(ValidatedResponse<SuccessCommandResult> response)
        {
            return response.IsValidResponse ? 
                NoContent() : 
                new BadRequestObjectResult(FormatErrors(response.Errors));
        }

        protected IActionResult GetPatchResponse(ValidatedResponse<SuccessCommandResult> response)
        {
            if (response.Result is { IsSuccess: false })
            {
                return NotFound();
            }
                
            if (response.IsValidResponse)
            {
                return NoContent();
            }
                
            return new BadRequestObjectResult(FormatErrors(response.Errors));
        }

        protected static List<ValidationError> FormatErrors(IEnumerable<ValidationFailure> errors)
        {
            return errors.Select(err => new ValidationError
            {
                PropertyName = err.PropertyName,
                ErrorMessage = err.ErrorMessage
            }).ToList();
        }
    }
}