using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Notifications.Commands.PostNotifications;

public class NotificationModelConversionTests
{
    [Test]
    [MoqAutoData]
    public void NotificationModel_Converts_To_Notification(NotificationModel source)
    {
        Notification sut = source;

        Assert.Multiple(() =>
        {
            Assert.That(sut.TemplateName, Is.EqualTo(source.TemplateName));
            Assert.That(sut.NotificationType, Is.EqualTo(source.NotificationType));
            Assert.That(sut.Ukprn, Is.EqualTo(source.Ukprn));
            Assert.That(sut.EmailAddress, Is.EqualTo(source.EmailAddress));
            Assert.That(sut.Contact, Is.EqualTo(source.Contact));
            Assert.That(sut.EmployerName, Is.EqualTo(source.EmployerName));
            Assert.That(sut.RequestId, Is.EqualTo(source.RequestId));
            Assert.That(sut.AccountLegalEntityId, Is.EqualTo(source.AccountLegalEntityId));
            Assert.That(sut.PermitApprovals, Is.EqualTo(source.PermitApprovals));
            Assert.That(sut.PermitRecruit, Is.EqualTo(source.PermitRecruit));
            Assert.That(sut.CreatedBy, Is.EqualTo(source.CreatedBy));
        });
    }
}
