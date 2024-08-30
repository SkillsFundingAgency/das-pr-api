using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Commands.AcceptCreateAccountRequest;

public class AcceptCreateAccountRequestCommandHandler(
    IProviderRelationshipsDataContext _providerRelationshipsDataContext,
    IRequestWriteRepository _requestWriteRepository,
    IAccountLegalEntityReadRepository _accountLegalEntityReadRepository
) : IRequestHandler<AcceptCreateAccountRequestCommand, ValidatedResponse<AcceptCreateAccountRequestCommandResult>>
{
    public async Task<ValidatedResponse<AcceptCreateAccountRequestCommandResult>> Handle(AcceptCreateAccountRequestCommand command, CancellationToken cancellationToken)
    {
        Request request = await _providerRelationshipsDataContext.Requests
                                    .Include(a => a.PermissionRequests)
                                    .FirstAsync(a => a.Id == command.RequestId, cancellationToken);

        // Add new records to the Accounts and AccountLegalEntities tables using values from
        // the body (if do not already exist).

        AccountLegalEntity? existingLegalEntity = await _accountLegalEntityReadRepository.GetAccountLegalEntity(command.AccountLegalEntity.Id, cancellationToken);

        // Add new records to the Accounts and AccountLegalEntities tables
        // using values from the body (if do not already exist).



        // Change Status of the Requests recdord to "accepted", and set ActionedBy and UpdatedDate

        request.ActionedBy = command.ActionedBy;
        request.Status = RequestStatus.Accepted;
        request.UpdatedDate = DateTime.UtcNow;

        // Then perform the permissions upsert (using the processing as in Set or Update Permissions)
        // with Ukprn from Requests and Operation
        // from the PermissionRequests.

        // The "accountLegalEntity"."id" as AccountLegalEntityId and "actionedBy" as EmployerUserRef plus Ukprn from Requests to be used for the PermissionsAudit for Action = "AccountCreated"

        return new ValidatedResponse<AcceptCreateAccountRequestCommandResult>();
    }
}
