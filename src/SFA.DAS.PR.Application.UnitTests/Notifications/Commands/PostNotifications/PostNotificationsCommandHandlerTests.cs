using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Notifications.Commands;
using SFA.DAS.PR.Application.Permissions.Commands.PostPermissions;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Notifications.Commands.PostNotifications;

public class PostNotificationsCommandHandlerTests
{
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_PostNotificationCommand_Creates_Notification_Returns_PostNotificationsCommandResult(PostNotificationsCommand command)
    {
        ValidatedResponse<PostNotificationsCommandResult> result = null!;

        int notificationCount = 0;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(Handle_PostNotificationCommand_Creates_Notification_Returns_PostNotificationsCommandResult)}")
        )
        {
            NotificationsWriteRepository repository = new NotificationsWriteRepository(context);

            PostNotificationsCommandHandler sut = new PostNotificationsCommandHandler(repository, context);

            result = await sut.Handle(command, cancellationToken);

            notificationCount = await context.Notifications.CountAsync(cancellationToken);
        }

        result.IsValidResponse.Should().BeTrue();
        Assert.That(command.Notifications, Has.Length.EqualTo(notificationCount), $"{command.Notifications.Length} Notifications must have been created.");
    }
}
