using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Common;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public class NotificationsWriteRepositoryTests
{
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    [Test]
    public async Task CreateNotifications_Returns_Success()
    {
        Notification[] notifications = [NotificationsTestData.Create(Guid.NewGuid(), NotificationType.Employer)];

        int notificationCount = 0;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(CreateNotifications_Returns_Success)}")
        )
        {
            NotificationsWriteRepository sut = new(context);

            await sut.CreateNotifications(notifications, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            notificationCount = context.Notifications.Count();
        }

        Assert.That(notificationCount, Is.EqualTo(1), "There should be one notification in the datacontext");
    }
}
