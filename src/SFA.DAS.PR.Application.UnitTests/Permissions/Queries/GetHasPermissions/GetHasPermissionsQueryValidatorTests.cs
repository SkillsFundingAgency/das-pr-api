using FluentValidation.TestHelper;
using Moq;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Application.Permissions.Queries.GetHasPermissions;
using SFA.DAS.PR.Domain.Interfaces;
using Operation = SFA.DAS.PR.Domain.Entities.Operation;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Queries.GetHasPermissions;
public class GetHasPermissionsQueryValidatorTests
{
    private Mock<IProviderReadRepository> _providerReadRepositoryTrueMock;

    private Mock<IProviderReadRepository> _providerReadRepositoryFalseMock;

    [SetUp]
    public void Setup()
    {
        _providerReadRepositoryTrueMock = new Mock<IProviderReadRepository>();
        _providerReadRepositoryTrueMock.Setup(a => a.ProviderExists(It.IsAny<long>(), CancellationToken.None)).ReturnsAsync(true);

        _providerReadRepositoryFalseMock = new Mock<IProviderReadRepository>();
        _providerReadRepositoryFalseMock.Setup(a => a.ProviderExists(It.IsAny<long>(), CancellationToken.None)).ReturnsAsync(false);
    }

    [Test]
    public async Task Validate_Ukprn_Valid_Query()
    {
        var sut = new GetHasPermissionsQueryValidator(_providerReadRepositoryTrueMock.Object);
        var result = await sut.TestValidateAsync(new GetHasPermissionsQuery { Ukprn = 10000003, AccountLegalEntityId = 1, Operations = new List<Operation> { Operation.CreateCohort } });
        result.ShouldNotHaveValidationErrorFor(query => query.Ukprn);
    }

    [Test]
    public async Task Validate_Ukprn_Returns_ErrorMessage()
    {
        var sut = new GetHasPermissionsQueryValidator(_providerReadRepositoryFalseMock.Object);
        var result = await sut.TestValidateAsync(new GetHasPermissionsQuery { Ukprn = null, AccountLegalEntityId = 1, Operations = new List<Operation> { Operation.CreateCohort } });
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
                    .WithErrorMessage(GetHasPermissionsQueryValidator.UkprnNotSuppliedValidationMessage);
    }

    [Test]
    public async Task Validate_Ukprn_Returns_InvalidFormat_ErrorMessage()
    {
        var sut = new GetHasPermissionsQueryValidator(_providerReadRepositoryFalseMock.Object);
        var result = await sut.TestValidateAsync(new GetHasPermissionsQuery { Ukprn = 10002, AccountLegalEntityId = 1, Operations = new List<Operation> { Operation.CreateCohort } });
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
                    .WithErrorMessage(UkprnValidator.UkprnFormatValidationMessage);
    }

    [Test]
    public async Task Validate_Operation_Valid_Query()
    {
        var sut = new GetHasPermissionsQueryValidator(_providerReadRepositoryTrueMock.Object);
        var result = await sut.TestValidateAsync(new GetHasPermissionsQuery { Ukprn = 12345678, AccountLegalEntityId = 1, Operations = new List<Operation> { Operation.Recruitment } });

        result.ShouldNotHaveValidationErrorFor(query => query.Operations);
    }

    [Test]
    public async Task Validate_Operation_Returns_ErrorMessage()
    {
        var sut = new GetHasPermissionsQueryValidator(_providerReadRepositoryTrueMock.Object);
        var result = await sut.TestValidateAsync(new GetHasPermissionsQuery { Ukprn = 12345678, AccountLegalEntityId = 1, Operations = new List<Operation>() });
        result.ShouldHaveValidationErrorFor(q => q.Operations)
                    .WithErrorMessage(OperationsValidator.OperationsFilterValidationMessage);
    }

    [Test]
    public async Task Validate_AccountLegalEntityId_Valid_Query()
    {
        var sut = new GetHasPermissionsQueryValidator(_providerReadRepositoryTrueMock.Object);
        var result = await sut.TestValidateAsync(new GetHasPermissionsQuery { Ukprn = 12345678, AccountLegalEntityId = 1, Operations = new List<Operation> { Operation.Recruitment } });

        result.ShouldNotHaveValidationErrorFor(query => query.AccountLegalEntityId);
    }

    [Test]
    public async Task Validate_AccountLegalEntityId_Returns_ErrorMessage()
    {
        var sut = new GetHasPermissionsQueryValidator(_providerReadRepositoryTrueMock.Object);
        var result = await sut.TestValidateAsync(new GetHasPermissionsQuery { Ukprn = 12345678, AccountLegalEntityId = null, Operations = new List<Operation> { Operation.Recruitment } });
        result.ShouldHaveValidationErrorFor(q => q.AccountLegalEntityId)
            .WithErrorMessage(GetHasPermissionsQueryValidator.AccountLegalEntityIdNotSuppliedValidationMessage);
    }
}
