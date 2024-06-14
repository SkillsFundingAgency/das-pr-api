using FluentValidation;
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
    public async Task ValidateUkrpn_Ukprn_ReturnsValid()
    {
        var entity = new TestUkprnEntity { Ukprn = 10000003 };

        var validator = new InlineValidator<TestUkprnEntity>();
        validator.RuleFor(x => x.Ukprn)
                 .CheckUkprnFormat();

        var validationResult = await validator.ValidateAsync(entity);

        Assert.That(validationResult.IsValid, Is.True);
    }

    [Test]
    public async Task ValidateUkrpn_Ukprn_ReturnsInvalid()
    {
        var entity = new TestUkprnEntity { Ukprn = 1 };

        var validator = new InlineValidator<TestUkprnEntity>();
        validator.RuleFor(x => x.Ukprn)
                 .CheckUkprnFormat();

        var validationResult = await validator.ValidateAsync(entity);

        Assert.Multiple(() =>
        {
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Errors[0].ErrorMessage, Is.EqualTo(UkprnFormatValidator.UkprnFormatValidationMessage));
        });        
    }
}