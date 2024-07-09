using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;

namespace SFA.DAS.PR.Application.Notifications.Commands;

public class PostNotificationsCommand : IRequest<ValidatedResponse<Unit>>
{
    public NotificationModel[] Notifications { get; set; } = [];
}
