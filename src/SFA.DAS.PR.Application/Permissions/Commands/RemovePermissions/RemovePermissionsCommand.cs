using MediatR;
using SFA.DAS.PR.Application.Common.Commands;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Commands.RemovePermissions;

public class RemovePermissionsCommand : IRequest<ValidatedResponse<SuccessCommandResult>>, IUkprnEntity, IAccountLegalEntityIdEntity
{
    public required Guid UserRef { get; set; }

    public long? Ukprn { get; set; }

    public long AccountLegalEntityId { get; set; }
}