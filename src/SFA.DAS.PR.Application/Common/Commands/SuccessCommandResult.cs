using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Application.Common.Commands
{
    [ExcludeFromCodeCoverage]
    public record SuccessCommandResult(bool IsSuccess = true);
}