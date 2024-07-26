using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Commands.CreateAddAccountRequest;

public class CreateAddAccountRequestCommandHandler(
    IProviderRelationshipsDataContext _providerRelationshipsDataContext,
    IRequestWriteRepository _requestWriteRepository,
    IAccountLegalEntityReadRepository _accountLegalEntityReadRepository
) : IRequestHandler<CreateAddAccountRequestCommand, ValidatedResponse<CreateAddAccountRequestCommandResult>>
{
    public async Task<ValidatedResponse<CreateAddAccountRequestCommandResult>> Handle(CreateAddAccountRequestCommand command, CancellationToken cancellationToken)
    {
        AccountLegalEntity? accountLegalEntity = await _accountLegalEntityReadRepository.GetAccountLegalEntity(command.AccountLegalEntityId, cancellationToken);

        Request request = await _requestWriteRepository.CreateRequest(BuildRequest(command, accountLegalEntity!), cancellationToken);

        await _providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);

        return new (new CreateAddAccountRequestCommandResult(request.Id));
    }

    private static Request BuildRequest(CreateAddAccountRequestCommand command, AccountLegalEntity accountLegalEntity)
    {
        return new Request()
        {
            Ukprn = command.Ukprn!.Value,
            RequestedBy = command.RequestedBy,
            RequestedDate = DateTime.UtcNow,
            AccountLegalEntityId = command.AccountLegalEntityId,
            RequestType = RequestType.AddAccount,
            Status = RequestStatus.New,
            EmployerOrganisationName = accountLegalEntity.Name,
            EmployerContactEmail = command.EmployerContactEmail,
            PermissionRequests = command.Operations.Select(a => new PermissionRequest()
            {
                Operation = (short)a
            }).ToList()
        };
    }
}
