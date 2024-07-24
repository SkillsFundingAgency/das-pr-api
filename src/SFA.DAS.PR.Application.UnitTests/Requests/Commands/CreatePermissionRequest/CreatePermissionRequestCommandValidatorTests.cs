using FluentValidation.TestHelper;
using Moq;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Application.Requests.Commands.CreatePermissionRequest;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.UnitTests.Requests.Commands.CreatePermissionRequest;

public class CreatePermissionRequestCommandValidatorTests
{
    private readonly Mock<IAccountLegalEntityReadRepository> _accountLegalEntityReadRepositoryValidMock = new Mock<IAccountLegalEntityReadRepository>();
    private readonly Mock<IProviderReadRepository> _providersReadRepositoryValidMock = new Mock<IProviderReadRepository>();
    private readonly Mock<IRequestReadRepository> _requestReadRepositoryValidMock = new Mock<IRequestReadRepository>();

    private readonly Mock<IAccountLegalEntityReadRepository> _accountLegalEntityReadRepositoryInvalidMock = new Mock<IAccountLegalEntityReadRepository>();

    [SetUp]
    public void SetUp()
    {
        _requestReadRepositoryValidMock.Setup(a => a.RequestExists(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _providersReadRepositoryValidMock.Setup(a => a.ProviderExists(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _accountLegalEntityReadRepositoryValidMock.Setup(a => a.AccountLegalEntityExists(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        _accountLegalEntityReadRepositoryInvalidMock.Setup(a => a.AccountLegalEntityExists(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
    }

    [Test]
    public async Task CreatePermissionRequestCommand_Valid_Command()
    {
        var sut = new CreatePermissionRequestCommandValidator(
            _requestReadRepositoryValidMock.Object,
            _providersReadRepositoryValidMock.Object,
            _accountLegalEntityReadRepositoryValidMock.Object
        );
        var result = await sut.TestValidateAsync(new CreatePermissionRequestCommand { Ukprn = 10000003, AccountLegalEntityId = 1, RequestedBy = Guid.NewGuid().ToString(), Operations = [Operation.CreateCohort] });
        result.ShouldNotHaveValidationErrorFor(query => query.Ukprn);
        result.ShouldNotHaveValidationErrorFor(query => query.AccountLegalEntityId);
        result.ShouldNotHaveValidationErrorFor(query => query.Ukprn);
    }

    [Test]
    public async Task CreatePermissionRequestCommand_Invalid_Ukprn()
    {
        var sut = new CreatePermissionRequestCommandValidator(
            _requestReadRepositoryValidMock.Object,
            _providersReadRepositoryValidMock.Object,
            _accountLegalEntityReadRepositoryValidMock.Object
        );
        var result = await sut.TestValidateAsync(new CreatePermissionRequestCommand { Ukprn = 1000000, AccountLegalEntityId = 1, RequestedBy = Guid.NewGuid().ToString(), Operations = [Operation.CreateCohort] });
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
            .WithErrorMessage(UkprnValidator.UkprnFormatValidationMessage);
    }

    [Test]
    public async Task CreatePermissionRequestCommand_Invalid_AccountLegalEntityId()
    {
        var sut = new CreatePermissionRequestCommandValidator(
            _requestReadRepositoryValidMock.Object,
            _providersReadRepositoryValidMock.Object,
            _accountLegalEntityReadRepositoryInvalidMock.Object
        );
        var result = await sut.TestValidateAsync(new CreatePermissionRequestCommand { Ukprn = 10000002, AccountLegalEntityId = 0, RequestedBy = Guid.NewGuid().ToString(), Operations = [Operation.CreateCohort] });
        result.ShouldHaveValidationErrorFor(q => q.AccountLegalEntityId)
            .WithErrorMessage(AccountLegalEntityValidator.AccountLegalEntityIdValidationMessage);
    }
}
