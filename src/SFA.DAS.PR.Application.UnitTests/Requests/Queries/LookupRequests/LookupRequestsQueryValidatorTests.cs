using FluentValidation.TestHelper;
using Moq;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Application.Requests.Queries.LookupRequests;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.UnitTests.Requests.Queries.LookupRequests;

public class LookupRequestsQueryValidatorTests
{
    private readonly Mock<IProviderReadRepository> _providersReadRepositoryValidMock = new Mock<IProviderReadRepository>();
    private readonly Mock<IProviderReadRepository> _providersReadRepositoryInvalidMock = new Mock<IProviderReadRepository>();

    [SetUp]
    public void SetUp()
    {
        _providersReadRepositoryValidMock.Setup(a => a.ProviderExists(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _providersReadRepositoryInvalidMock.Setup(a => a.ProviderExists(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
    }

    [TestCase("paye", null, null)]
    [TestCase("paye", "", null)]
    [TestCase(null, "email", null)]
    [TestCase("", "email", null)]
    [TestCase("", "", 1)]
    [TestCase(null, "", 1)]
    [TestCase("", null, 1)]
    [TestCase(null, null, 1)]
    public async Task LookupRequestsQuery_Valid_Query(string? paye, string? email, long? accountLegalEntityId)
    {
        var sut = new LookupRequestsQueryValidator(_providersReadRepositoryValidMock.Object);
        var result = await sut.TestValidateAsync(new LookupRequestsQuery(10000003, paye, email, accountLegalEntityId));
        result.ShouldNotHaveValidationErrorFor(query => query.Ukprn);
        result.ShouldNotHaveValidationErrorFor(query => query.Paye);
    }

    [Test]
    public async Task LookupRequestsQuery_InvalidPayeAndEmail()
    {
        var sut = new LookupRequestsQueryValidator(_providersReadRepositoryValidMock.Object);
        var result = await sut.TestValidateAsync(new LookupRequestsQuery(10000003, string.Empty, string.Empty, null));
        result.ShouldHaveValidationErrorFor(q => q.Paye)
            .WithErrorMessage(LookupRequestsQueryValidator.PayeOrEmailOrAccountLegalEntityIdValidationMessage);
    }

    [Test]
    public async Task LookupRequestsQuery_Invalid_UkprnFormat()
    {
        var sut = new LookupRequestsQueryValidator(_providersReadRepositoryValidMock.Object);
        var result = await sut.TestValidateAsync(new LookupRequestsQuery(100, "Paye", null, null));
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
            .WithErrorMessage(UkprnValidator.UkprnFormatValidationMessage);
    }

    [Test]
    public async Task LookupRequestsQuery_Invalid_UkprnExists()
    {
        var sut = new LookupRequestsQueryValidator(_providersReadRepositoryInvalidMock.Object);
        var result = await sut.TestValidateAsync(new LookupRequestsQuery(10000002, "Paye", null, null));
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
            .WithErrorMessage(UkprnValidator.ProviderEntityExistValidationMessage);
    }
}
