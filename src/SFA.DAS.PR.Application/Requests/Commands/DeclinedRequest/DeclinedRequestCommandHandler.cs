using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Commands.DeclinedRequest;

public sealed class DeclinedRequestCommandHandler(IProviderRelationshipsDataContext _providerRelationshipsDataContext, IRequestWriteRepository _requestWriteRepository) : IRequestHandler<DeclinedRequestCommand, ValidatedResponse<Unit>>
{
    public async Task<ValidatedResponse<Unit>> Handle(DeclinedRequestCommand command, CancellationToken cancellationToken)
    {
        Request? request = await _requestWriteRepository.GetRequest(command.RequestId, cancellationToken);

        request!.Status = RequestStatus.Declined;
        request!.ActionedBy = command.ActionedBy;
        request!.UpdatedDate = DateTime.UtcNow;

        await _providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);

        return ValidatedResponse<Unit>.EmptySuccessResponse();
    }
}
