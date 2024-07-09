using FluentValidation.TestHelper;
using Moq;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;
using SFA.DAS.PR.Data.Repositories;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Queries.GetPermissions;
public class GetPermissionsQueryValidatorTests
{
    Mock<IProviderReadRepository> _mockProviderExistsReadRepository = new Mock<IProviderReadRepository>();
    Mock<IProviderReadRepository> _mockProviderNotExistsReadRepository = new Mock<IProviderReadRepository>();
    [SetUp]
    public void Setup()
    {
        _mockProviderExistsReadRepository.Setup(a =>
            a.ProviderExists(
                It.IsAny<long>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(true);

        _mockProviderNotExistsReadRepository.Setup(a =>
            a.ProviderExists(
                It.IsAny<long>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(false);
    }

    [Test]
    public async Task Validate_AccountHashedIdEmpty_Returns_ErrorMessage()
    {
        var sut = new GetPermissionsQueryValidator(_mockProviderExistsReadRepository.Object);
        GetPermissionsQuery query = GetConstructedGetPermissionsQuery();
        query.accountLegalEntityId = null;

        var result = await sut.TestValidateAsync(query);
        result.ShouldHaveValidationErrorFor(q => q.accountLegalEntityId)
            .WithErrorMessage(GetPermissionsQueryValidator.AccountLegalEntityIdValidationMessage);
    }

    [Test]
    public async Task Validate_UkprnEmpty_Returns_ErrorMessage()
    {
        var sut = new GetPermissionsQueryValidator(_mockProviderExistsReadRepository.Object);
        GetPermissionsQuery query = GetConstructedGetPermissionsQuery();
        query.Ukprn = null;
        var result = await sut.TestValidateAsync(query);

        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
            .WithErrorMessage(GetPermissionsQueryValidator.UkprnValidationMessage);
    }

    [TestCase(1)]
    [TestCase(11)]
    [TestCase(111)]
    [TestCase(1111)]
    [TestCase(11111)]
    [TestCase(111111)]
    [TestCase(1111111)]
    [TestCase(1111111111)]
    [TestCase(211111111)]
    public async Task Validate_UkprnWrongFormat_Returns_ErrorMessage(long ukprn)
    {
        var sut = new GetPermissionsQueryValidator(_mockProviderExistsReadRepository.Object);
        GetPermissionsQuery query = GetConstructedGetPermissionsQuery();
        query.Ukprn = ukprn;
        var result = await sut.TestValidateAsync(query);

        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
            .WithErrorMessage(UkprnValidator.UkprnFormatValidationMessage);
    }

    [Test]
    public async Task Validate_Valid_Query()
    {
        var sut = new GetPermissionsQueryValidator(_mockProviderExistsReadRepository.Object);
        GetPermissionsQuery query = GetConstructedGetPermissionsQuery();
        var result = await sut.TestValidateAsync(query);

        result.ShouldNotHaveAnyValidationErrors();
    }

    private GetPermissionsQuery GetConstructedGetPermissionsQuery()
    {
        return new GetPermissionsQuery
        {
            Ukprn = 12345678,
            accountLegalEntityId = 2
        };
    }
}
