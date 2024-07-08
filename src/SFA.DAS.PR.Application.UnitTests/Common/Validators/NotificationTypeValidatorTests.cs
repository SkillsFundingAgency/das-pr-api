using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Common;

namespace SFA.DAS.PR.Application.UnitTests.Common.Validators;

public class TestEntity
{
    public required string NotificationType { get; set; }
}

public class NotificationTypeValidatorTests
{
    [Test]
    public async Task Validate_NotificationType_Returns_Valid()
    {
        var entity = new TestEntity { NotificationType = NotificationType.Provider.ToString() };

        var validator = new InlineValidator<TestEntity>();
        validator.RuleFor(x => x.NotificationType)
                 .ValidNotificationType();

        var validationResult = await validator.ValidateAsync(entity);

        Assert.That(validationResult.IsValid, Is.True);
    }

    [Test]
    public async Task Validate_NotificationType_Returns_Invalid()
    {
        var entity = new TestEntity { NotificationType = "invalid_notification_type" };

        var validator = new InlineValidator<TestEntity>();
        validator.RuleFor(x => x.NotificationType)
                 .ValidNotificationType();

        var validationResult = await validator.ValidateAsync(entity);

        Assert.That(validationResult.IsValid, Is.False);
    }
}
