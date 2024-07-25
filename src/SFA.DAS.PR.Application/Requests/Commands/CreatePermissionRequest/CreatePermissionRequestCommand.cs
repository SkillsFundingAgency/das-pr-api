using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.Requests.Commands.CreatePermissionRequest;

public class CreatePermissionRequestCommand : IRequest<ValidatedResponse<CreatePermissionRequestCommandResult>>, IUkprnEntity, IOperationsEntity, IAccountLegalEntityIdEntity
{
    public required long? Ukprn { get; set; }

    public required string RequestedBy { get; set; }

    public required long AccountLegalEntityId { get; set; }

    public required List<Operation> Operations { get; set; } = [];
}
