using FluentValidation;
using Moq;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.UnitTests.Common.Validators;

public class RequestsValidatorTests
{
    [Test]
    public async Task Validate_ValidateRequest_Returns_True()
    {
        RequestStatus[] requestStatuses = [RequestStatus.New, RequestStatus.Sent];
        var entity = new RequestValidationObject { Ukprn = 10000003, AccountLegalEntityId = 1, RequestStatuses = requestStatuses };

        var requestReadRepository = new Mock<IRequestReadRepository>();
        requestReadRepository.Setup(a => a.RequestExists(entity.Ukprn.Value, entity.AccountLegalEntityId, entity.RequestStatuses, CancellationToken.None)).ReturnsAsync(false);

        var validator = new InlineValidator<RequestValidationObject>();
        validator.RuleFor(x => new RequestValidationObject() { Ukprn = x.Ukprn, AccountLegalEntityId = x.AccountLegalEntityId, RequestStatuses = x.RequestStatuses })
                 .ValidateRequest(requestReadRepository.Object);

        var validationResult = await validator.ValidateAsync(entity);

        Assert.That(validationResult.IsValid, Is.True);
    }

    [Test]
    public async Task Validate_ValidateRequest_Returns_False()
    {
        RequestStatus[] requestStatuses = [RequestStatus.New, RequestStatus.Sent];
        var entity = new RequestValidationObject { Ukprn = 10000003, AccountLegalEntityId = 1, RequestStatuses = requestStatuses };

        var requestReadRepository = new Mock<IRequestReadRepository>();
        requestReadRepository.Setup(a => a.RequestExists(entity.Ukprn.Value, entity.AccountLegalEntityId, entity.RequestStatuses, CancellationToken.None)).ReturnsAsync(true);

        var validator = new InlineValidator<RequestValidationObject>();
        validator.RuleFor(x => new RequestValidationObject() { Ukprn = x.Ukprn, AccountLegalEntityId = x.AccountLegalEntityId, RequestStatuses = x.RequestStatuses })
                 .ValidateRequest(requestReadRepository.Object);

        var validationResult = await validator.ValidateAsync(entity);

        Assert.Multiple(() =>
        {
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Errors[0].ErrorMessage, Is.EqualTo(RequestsValidator.RequestValidationMessage));
        });
    }
}
