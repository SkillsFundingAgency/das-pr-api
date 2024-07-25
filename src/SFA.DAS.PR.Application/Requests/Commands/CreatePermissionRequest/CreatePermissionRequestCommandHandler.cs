using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Commands.CreatePermissionRequest;

public class CreatePermissionRequestCommandHandler(
    IProviderRelationshipsDataContext _providerRelationshipsDataContext,
    IRequestWriteRepository _requestWriteRepository,
    IAccountLegalEntityReadRepository _accountLegalEntityReadRepository
) : IRequestHandler<CreatePermissionRequestCommand, ValidatedResponse<CreatePermissionRequestCommandResult>>
{
    public async Task<ValidatedResponse<CreatePermissionRequestCommandResult>> Handle(CreatePermissionRequestCommand command, CancellationToken cancellationToken)
    {
        AccountLegalEntity? accountLegalEntity = await _accountLegalEntityReadRepository.GetAccountLegalEntity(command.AccountLegalEntityId, cancellationToken);

        Request request = await _requestWriteRepository.CreateRequest(BuildRequest(command, accountLegalEntity!), cancellationToken);

        await _providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);

        return new(new CreatePermissionRequestCommandResult(request.Id));
    }

    private static Request BuildRequest(CreatePermissionRequestCommand command, AccountLegalEntity accountLegalEntity)
    {
        return new Request()
        {
            Ukprn = command.Ukprn!.Value,
            RequestedBy = command.RequestedBy,
            RequestedDate = DateTime.UtcNow,
            AccountLegalEntityId = command.AccountLegalEntityId,
            RequestType = RequestType.Permission.ToString(),
            Status = RequestStatus.New.ToString(),
            EmployerOrganisationName = accountLegalEntity.Name,
            PermissionRequests = command.Operations.Select(a => new PermissionRequest()
            {
                Operation = (int)a
            }).ToList()
        };
    }
}
