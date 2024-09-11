using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Commands.CreateNewAccountRequest;

public class CreateNewAccountRequestCommandHandler(
    IProviderRelationshipsDataContext _providerRelationshipsDataContext,
    IRequestWriteRepository _requestWriteRepository
) : IRequestHandler<CreateNewAccountRequestCommand, ValidatedResponse<CreateNewAccountRequestCommandResult>>
{
    public async Task<ValidatedResponse<CreateNewAccountRequestCommandResult>> Handle(CreateNewAccountRequestCommand command, CancellationToken cancellationToken)
    {
        Request request = await _requestWriteRepository.CreateRequest(BuildRequest(command), cancellationToken);

        await _providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);

        return new(new CreateNewAccountRequestCommandResult(request.Id));
    }

    private static Request BuildRequest(CreateNewAccountRequestCommand command)
    {
        return new Request()
        {
            Ukprn = command.Ukprn!.Value,
            RequestedBy = command.RequestedBy,
            EmployerOrganisationName = command.EmployerOrganisationName,
            EmployerContactFirstName = command.EmployerContactFirstName,
            EmployerContactLastName = command.EmployerContactLastName,
            EmployerPAYE = command.EmployerPAYE,
            EmployerContactEmail = command.EmployerContactEmail,
            EmployerAORN = command.EmployerAORN,
            RequestedDate = DateTime.UtcNow,
            RequestType = RequestType.CreateAccount,
            Status = RequestStatus.New,
            PermissionRequests = command.Operations.Select(a => new PermissionRequest()
            {
                Operation = (short)a
            }).ToList()
        };
    }
}
