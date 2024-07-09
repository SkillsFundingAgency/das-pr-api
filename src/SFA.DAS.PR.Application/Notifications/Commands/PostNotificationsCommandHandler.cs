using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Notifications.Commands;

public class PostNotificationsCommandHandler(INotificationsWriteRepository _notificationsWriteRepository, IProviderRelationshipsDataContext _providerRelationshipsDataContext) : IRequestHandler<PostNotificationsCommand, ValidatedResponse<Unit>>
{
    public async Task<ValidatedResponse<Unit>> Handle(PostNotificationsCommand command, CancellationToken cancellationToken)
    {
        await _notificationsWriteRepository.CreateNotifications(command.Notifications.Select(a => (Notification)a), cancellationToken);
        await _providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);

        return ValidatedResponse<Unit>.EmptySuccessResponse();
    }
}
