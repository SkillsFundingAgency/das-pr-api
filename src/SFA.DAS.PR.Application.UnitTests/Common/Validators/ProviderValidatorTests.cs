using FluentValidation;
using Moq;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.UnitTests.Common.Validators;
public class ProviderValidatorTests
{
    public class TestProviderUkprn : IProviderUkprn
    {
        public long? Ukprn { get; set; }

    }

    [Test]
    public async Task ValidateProviderExists_ValidEntity_ReturnsValid()
    {
        var entity = new TestProviderUkprn { Ukprn = 12345678 };

        var mockRepository = new Mock<IProvidersReadRepository>();
        mockRepository.Setup(r => r.ProviderExists(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true);

        var validator = new InlineValidator<TestProviderUkprn>();
        validator.RuleFor(x => x.Ukprn)
                 .ValidateProviderExists(mockRepository.Object);

        var validationResult = await validator.ValidateAsync(entity);

        Assert.That(validationResult.IsValid, Is.True);
    }

    [Test]
    public async Task ValidateProvider_Null_Ukprn_ReturnsMustBeProvidedMessage()
    {
        var entity = new TestProviderUkprn { Ukprn = null };

        var mockRepository = new Mock<IProvidersReadRepository>();
        mockRepository.Setup(r => r.ProviderExists(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new InlineValidator<TestProviderUkprn>();
        validator.RuleFor(x => x.Ukprn)
                 .ValidateProviderExists(mockRepository.Object);

        var validationResult = await validator.ValidateAsync(entity);

        Assert.Multiple(() =>
        {
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Errors[0].ErrorMessage, Is.EqualTo(ProviderValidator.UkprnValidationMessage));
        });
    }

    [Test]
    public async Task ValidateProvider_UkprnDoesNotExist_ReturnsProviderDoesNotExistMessage()
    {
        var entity = new TestProviderUkprn { Ukprn = 12345678 };

        var mockRepository = new Mock<IProvidersReadRepository>();
        mockRepository.Setup(r => r.ProviderExists(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new InlineValidator<TestProviderUkprn>();
        validator.RuleFor(x => x.Ukprn)
            .ValidateProviderExists(mockRepository.Object);

        var validationResult = await validator.ValidateAsync(entity);

        Assert.Multiple(() =>
        {
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Errors[0].ErrorMessage, Is.EqualTo(ProviderValidator.ProviderExistValidationMessage));
        });
    }
}
