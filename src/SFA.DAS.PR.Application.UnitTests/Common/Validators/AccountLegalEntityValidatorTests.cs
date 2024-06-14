using FluentValidation;
using Moq;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.UnitTests.Common.Validators;

public class AccountLegalEntityValidatorTests
{
    public class TestAccountLegalEntityIdEntity : IAccountLegalEntityIdEntity
    {
        public long AccountLegalEntityId { get; set; }
    }

    [Test]
    public async Task ValidateAccountLegalEntityExists_ValidEntity_ReturnsValid()
    {
        var entity = new TestAccountLegalEntityIdEntity { AccountLegalEntityId = 1 };

        var mockRepository = new Mock<IAccountLegalEntityReadRepository>();
        mockRepository.Setup(r => r.AccountLegalEntityExists(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true);

        var validator = new InlineValidator<TestAccountLegalEntityIdEntity>();
        validator.RuleFor(x => x.AccountLegalEntityId)
                 .ValidateAccountLegalEntityExists(mockRepository.Object);

        var validationResult = await validator.ValidateAsync(entity);

        Assert.IsTrue(validationResult.IsValid);
    }

    [Test]
    public async Task ValidateAccountLegalEntity_Null_AccountLegalEntityId_ReturnsInvalid()
    {
        var entity = new TestAccountLegalEntityIdEntity { AccountLegalEntityId = 0 };

        var mockRepository = new Mock<IAccountLegalEntityReadRepository>();
        mockRepository.Setup(r => r.AccountLegalEntityExists(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true);

        var validator = new InlineValidator<TestAccountLegalEntityIdEntity>();
        validator.RuleFor(x => x.AccountLegalEntityId)
                 .ValidateAccountLegalEntityExists(mockRepository.Object);

        var validationResult = await validator.ValidateAsync(entity);

        Assert.IsFalse(validationResult.IsValid);
        Assert.That(validationResult.Errors[0].ErrorMessage, Is.EqualTo(AccountLegalEntityValidator.AccountLegalEntityIdValidationMessage));
    }

    [Test]
    public async Task ValidateAccountLegalEntity_AccountLegalEntityExists_ReturnsInvalid()
    {
        var entity = new TestAccountLegalEntityIdEntity { AccountLegalEntityId = 1 };

        var mockRepository = new Mock<IAccountLegalEntityReadRepository>();
        mockRepository.Setup(r => r.AccountLegalEntityExists(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(false);

        var validator = new InlineValidator<TestAccountLegalEntityIdEntity>();
        validator.RuleFor(x => x.AccountLegalEntityId)
                 .ValidateAccountLegalEntityExists(mockRepository.Object);

        var validationResult = await validator.ValidateAsync(entity);

        Assert.IsFalse(validationResult.IsValid);
        Assert.That(validationResult.Errors[0].ErrorMessage, Is.EqualTo(AccountLegalEntityValidator.AccountLegalEntityExistValidationMessage));
    }
}