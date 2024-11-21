using MediatR;
using SFA.DAS.PR.Application.Common.Commands;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.Permissions.Commands.PostPermissions;

public class PostPermissionsCommand : IRequest<ValidatedResponse<SuccessCommandResult>>, IUkprnEntity, IOperationsEntity, IAccountLegalEntityIdEntity
{
    public Guid UserRef { get; set; }

    public long? Ukprn { get; set; }

    public long AccountLegalEntityId { get; set; }

    public List<Operation> Operations { get; set; } = [];
}
