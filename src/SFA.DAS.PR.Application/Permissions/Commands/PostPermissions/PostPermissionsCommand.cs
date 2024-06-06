using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Commands.PostPermissions;

public class PostPermissionsCommand : IRequest<ValidatedResponse<PostPermissionsCommandResult>>, IUkprnEntity, IOperationsEntity
{
    public required Guid UserRef { get; set; }

    public long? Ukprn { get; set; }

    public required long AccountLegalEntityId { get; set; }

    public required List<Operation> Operations { get; set; } = [];
}
