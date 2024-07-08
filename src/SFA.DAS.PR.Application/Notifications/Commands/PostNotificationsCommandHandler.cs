using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Notifications.Commands;

public class PostNotificationsCommandHandler(INotificationsWriteRepository _notificationsWriteRepository, IProviderRelationshipsDataContext _providerRelationshipsDataContext) : IRequestHandler<PostNotificationsCommand, ValidatedResponse<PostNotificationsCommandResult>>
{
    public async Task<ValidatedResponse<PostNotificationsCommandResult>> Handle(PostNotificationsCommand command, CancellationToken cancellationToken)
    {
        await _notificationsWriteRepository.CreateNotifications(CreateNotifications(command.Notifications), cancellationToken);
        await _providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);

        return ValidatedResponse<PostNotificationsCommandResult>.EmptySuccessResponse();
    }

    private IEnumerable<Notification> CreateNotifications(NotificationModel[] notifications)
    {
        return notifications.Select(a => new Notification()
        {
            TemplateName = a.TemplateName,
            NotificationType = a.NotificationType.ToString(),
            Ukprn = a.Ukprn!.Value,
            EmailAddress = a.EmailAddress,
            Contact = a.Contact,
            EmployerName = a.EmployerName,
            RequestsId = a.requestsId,
            AccountLegalEntityId = a.AccountLegalEntityId,
            PermitApprovals = a.PermitApprovals,
            PermitRecruit = a.PermitRecruit,
            CreatedBy = a.CreatedBy,
            CreatedDate = DateTime.UtcNow
        });
    }
}
