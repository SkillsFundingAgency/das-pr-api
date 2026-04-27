using AutoFixture.NUnit4;
using FluentAssertions;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Models;

namespace SFA.DAS.PR.Domain.UnitTests.Entities;

public class NotificationTests
{
    [Test, AutoData]
    public void Operator_ConvertsFromNotificationModel(NotificationModel source)
    {
        Notification sut = source;

        sut.Should().BeEquivalentTo(source, config => config.ExcludingMissingMembers());
    }
}
