using FluentValidation;
using Moq;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.UnitTests.Common.Validators;

public class UkprnFormatValidatorTests
{ 
    public class TestUkprnEntity : IUkprnEntity
    {
        public long? Ukprn { get; set; }
    }

    [Test]
    public async Task ValidateUkrpnFormat_ReturnsValid()
    {
        var providerReadRepositoryMock = new Mock<IProviderReadRepository>();
        providerReadRepositoryMock.Setup(a => a.ProviderExists(It.IsAny<long>(), CancellationToken.None)).ReturnsAsync(true);

        var entity = new TestUkprnEntity { Ukprn = 10000003 };

        var validator = new InlineValidator<TestUkprnEntity>();
        validator.RuleFor(x => x.Ukprn)
                 .IsValidUkprn(providerReadRepositoryMock.Object);

        var validationResult = await validator.ValidateAsync(entity);

        Assert.That(validationResult.IsValid, Is.True);
    }

    [Test]
    public async Task ValidateUkrpnFormat_ReturnsInvalid()
    {
        var providerReadRepositoryMock = new Mock<IProviderReadRepository>();

        var entity = new TestUkprnEntity { Ukprn = 1 };

        var validator = new InlineValidator<TestUkprnEntity>();
        validator.RuleFor(x => x.Ukprn)
                 .IsValidUkprn(providerReadRepositoryMock.Object);

        var validationResult = await validator.ValidateAsync(entity);

        Assert.Multiple(() =>
        {
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Errors[0].ErrorMessage, Is.EqualTo(UkprnFormatValidator.UkprnFormatValidationMessage));
        });        
    }

    [Test]
    public async Task ValidateUkrpnExists_ReturnsInvalid()
    {
        var providerReadRepositoryMock = new Mock<IProviderReadRepository>();
        providerReadRepositoryMock.Setup(a => a.ProviderExists(It.IsAny<long>(), CancellationToken.None)).ReturnsAsync(false);

        var entity = new TestUkprnEntity { Ukprn = 10000001 };

        var validator = new InlineValidator<TestUkprnEntity>();
        validator.RuleFor(x => x.Ukprn)
                 .IsValidUkprn(providerReadRepositoryMock.Object);

        var validationResult = await validator.ValidateAsync(entity);

        Assert.Multiple(() =>
        {
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Errors[0].ErrorMessage, Is.EqualTo(UkprnFormatValidator.ProviderEntityExistValidationMessage));
        });
    }
}