using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface INotificationsWriteRepository
{
    Task CreateNotifications(IEnumerable<Notification> notifications, CancellationToken cancellationToken);
}
