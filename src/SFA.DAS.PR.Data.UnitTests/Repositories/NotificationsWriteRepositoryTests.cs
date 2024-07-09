using AutoFixture;
using SFA.DAS.Testing.AutoFixture;
using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public class NotificationsWriteRepositoryTests
{
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    private Fixture _fixture = null!;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Test]
    public async Task CreateNotifications_Returns_Success()
    {
        IEnumerable<Notification> notifications =
            FixtureBuilder.RecursiveMoqFixtureFactory().CreateMany<Notification>(3);

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

        Assert.That(notificationCount, Is.EqualTo(notifications.Count()), "There should be one notification in the datacontext");
    }
}
