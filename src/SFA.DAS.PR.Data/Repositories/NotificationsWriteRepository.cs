using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class NotificationsWriteRepository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : INotificationsWriteRepository
{
    public async Task CreateNotifications(IEnumerable<Notification> notifications, CancellationToken cancellationToken)
    {
        await providerRelationshipsDataContext.Notifications.AddRangeAsync(notifications, cancellationToken);
    }
}
