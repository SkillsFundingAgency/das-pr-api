using FluentValidation.TestHelper;
using Moq;
using SFA.DAS.PR.Application.Notifications.Commands;
using SFA.DAS.PR.Domain.Common;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.UnitTests.Notifications.Commands.PostNotifications;

public class PostNotificationsCommandValidatorTests
{
    private readonly Mock<IProviderReadRepository> _providerReadRepositoryMock = new Mock<IProviderReadRepository>();

    [Test]
    public async Task Validate_EmailAddress_Returns_Valid()
    {
        var sut = new PostNotificationsCommandValidator(_providerReadRepositoryMock.Object);

        var notificationModel = CreateNotificationModel("test@email.com", 10000001, nameof(NotificationType.Provider));

        var command = new PostNotificationsCommand
        {
            Notifications = [notificationModel]
        };

        var result = await sut.TestValidateAsync(command);

        foreach (var notification in command.Notifications)
        {
            result.ShouldNotHaveValidationErrorFor(query => query.Notifications);
        }
    }

    [Test]
    public async Task Validate_EmailAddress_Returns_Invalid()
    {
        var sut = new PostNotificationsCommandValidator(_providerReadRepositoryMock.Object);

        var notificationModel = CreateNotificationModel("invalid_email", 10000001, nameof(NotificationType.Provider));

        var command = new PostNotificationsCommand
        {
            Notifications = [notificationModel]
        };

        var result = await sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor("Notifications[0].EmailAddress")
              .WithErrorMessage(NotificationModelValidator.EmailAddressValidationMessage);
    }

    [Test]
    public async Task Validate_NotificationType_Returns_Valid()
    {
        var sut = new PostNotificationsCommandValidator(_providerReadRepositoryMock.Object);

        var notificationModel = CreateNotificationModel("test@email.com", 10000001, nameof(NotificationType.Provider));

        var command = new PostNotificationsCommand
        {
            Notifications = [notificationModel]
        };

        var result = await sut.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor("Notifications[0].NotificationType");
    }

    [Test]
    public async Task Validate_NotificationType_Returns_Invalid()
    {
        var sut = new PostNotificationsCommandValidator(_providerReadRepositoryMock.Object);

        var notificationModel = CreateNotificationModel("test@email.com", 10000001, "invalid_notification_type");

        var command = new PostNotificationsCommand
        {
            Notifications = [notificationModel]
        };

        var result = await sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor("Notifications[0].NotificationType");
    }

    private NotificationModel CreateNotificationModel(string email, long ukprn, string notificationType)
    {
        return new ()
        {
            TemplateName = "TemplateName",
            NotificationType = notificationType,
            Ukprn = ukprn,
            AccountLegalEntityId = 1,
            EmailAddress = email,
            CreatedBy = "test user"
        };
    }
}
