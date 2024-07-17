using SFA.DAS.PR.Domain.Common;
using AutoFixture;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Data.UnitTests.Setup;

public static class NotificationsTestData
{
    public static Notification Create(Guid id, NotificationType notificationType) => FixtureBuilder.RecursiveMoqFixtureFactory().Build<Notification>()
        .With(a => a.Id, id)
        .With(a => a.NotificationType, notificationType.ToString())
    .Create();
}
