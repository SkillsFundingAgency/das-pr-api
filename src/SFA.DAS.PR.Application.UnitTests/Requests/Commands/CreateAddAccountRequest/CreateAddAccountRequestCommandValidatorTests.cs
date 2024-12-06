using FluentValidation.TestHelper;
using Moq;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Application.Requests.Commands.CreateAddAccountRequest;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.UnitTests.Requests.Commands.CreateAddAccountRequest;

public class CreateAddAccountRequestCommandValidatorTests
{
    private readonly Mock<IAccountLegalEntityReadRepository> _accountLegalEntityReadRepositoryValidMock = new Mock<IAccountLegalEntityReadRepository>();
    private readonly Mock<IProviderReadRepository> _providersReadRepositoryValidMock = new Mock<IProviderReadRepository>();
    private readonly Mock<IRequestReadRepository> _requestReadRepositoryValidMock = new Mock<IRequestReadRepository>();

    private readonly Mock<IAccountLegalEntityReadRepository> _accountLegalEntityReadRepositoryInvalidMock = new Mock<IAccountLegalEntityReadRepository>();

    private const string Paye = "222/AAA";

    [SetUp]
    public void SetUp()
    {
        _requestReadRepositoryValidMock.Setup(a => a.RequestExists(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<RequestStatus[]>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _providersReadRepositoryValidMock.Setup(a => a.ProviderExists(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _accountLegalEntityReadRepositoryValidMock.Setup(a => a.AccountLegalEntityExists(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        _accountLegalEntityReadRepositoryInvalidMock.Setup(a => a.AccountLegalEntityExists(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
    }

    [Test]
    public async Task CreateAddAccountRequestCommand_Valid_Command()
    {
        var sut = new CreateAddAccountRequestCommandValidator(
            _requestReadRepositoryValidMock.Object,
            _providersReadRepositoryValidMock.Object,
            _accountLegalEntityReadRepositoryValidMock.Object
        );
        var result = await sut.TestValidateAsync(new CreateAddAccountRequestCommand { Ukprn = 10000003, AccountLegalEntityId = 1, RequestedBy = Guid.NewGuid().ToString(), EmployerContactEmail = "test@email.com", Paye = Paye, Operations = [Operation.CreateCohort] });
        result.ShouldNotHaveValidationErrorFor(query => query.Ukprn);
        result.ShouldNotHaveValidationErrorFor(query => query.AccountLegalEntityId);
        result.ShouldNotHaveValidationErrorFor(query => query.EmployerContactEmail);
        result.ShouldNotHaveValidationErrorFor(query => query.Ukprn);
        result.ShouldNotHaveValidationErrorFor(query => query.Paye);
    }

    [Test]
    public async Task CreateAddAccountRequestCommand_Invalid_Ukprn()
    {
        var sut = new CreateAddAccountRequestCommandValidator(
            _requestReadRepositoryValidMock.Object,
            _providersReadRepositoryValidMock.Object,
            _accountLegalEntityReadRepositoryValidMock.Object
        );
        var result = await sut.TestValidateAsync(new CreateAddAccountRequestCommand { Ukprn = 1000000, AccountLegalEntityId = 1, RequestedBy = Guid.NewGuid().ToString(), EmployerContactEmail = "test@email.com", Paye = Paye, Operations = [Operation.CreateCohort] });
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
            .WithErrorMessage(UkprnValidator.UkprnFormatValidationMessage);
    }

    [Test]
    public async Task CreateAddAccountRequestCommand_Invalid_AccountLegalEntityId()
    {
        var sut = new CreateAddAccountRequestCommandValidator(
            _requestReadRepositoryValidMock.Object,
            _providersReadRepositoryValidMock.Object,
            _accountLegalEntityReadRepositoryInvalidMock.Object
        );
        var result = await sut.TestValidateAsync(new CreateAddAccountRequestCommand { Ukprn = 10000002, AccountLegalEntityId = 0, RequestedBy = Guid.NewGuid().ToString(), EmployerContactEmail = "test@email.com", Paye = Paye, Operations = [Operation.CreateCohort] });
        result.ShouldHaveValidationErrorFor(q => q.AccountLegalEntityId)
            .WithErrorMessage(AccountLegalEntityValidator.AccountLegalEntityIdValidationMessage);
    }

    [Test]
    public async Task CreateAddAccountRequestCommand_Invalid_EmployerContactEmail()
    {
        var sut = new CreateAddAccountRequestCommandValidator(
            _requestReadRepositoryValidMock.Object,
            _providersReadRepositoryValidMock.Object,
            _accountLegalEntityReadRepositoryValidMock.Object
        );
        var result = await sut.TestValidateAsync(new CreateAddAccountRequestCommand { Ukprn = 10000002, AccountLegalEntityId = 1, RequestedBy = Guid.NewGuid().ToString(), EmployerContactEmail = "testemail.com", Paye = Paye, Operations = [Operation.CreateCohort] });
        result.ShouldHaveValidationErrorFor(q => q.EmployerContactEmail)
            .WithErrorMessage(CreateAddAccountRequestCommandValidator.EmployerContactEmailValidationMessage);
    }

    [TestCase("")]
    [TestCase(null)]
    public async Task CreateAddAccountRequestCommand_No_Paye(string? paye)
    {
        var sut = new CreateAddAccountRequestCommandValidator(
            _requestReadRepositoryValidMock.Object,
            _providersReadRepositoryValidMock.Object,
            _accountLegalEntityReadRepositoryValidMock.Object
        );
        var result = await sut.TestValidateAsync(new CreateAddAccountRequestCommand { Ukprn = 10000002, AccountLegalEntityId = 1, RequestedBy = Guid.NewGuid().ToString(), EmployerContactEmail = "test@email.com", Paye = paye, Operations = [Operation.CreateCohort] });
        result.ShouldHaveValidationErrorFor(q => q.Paye)
            .WithErrorMessage(CreateAddAccountRequestCommandValidator.NoPayeMessage);
    }

    [TestCase("1/1")]
    [TestCase("11/1")]
    [TestCase("11A/1")]
    [TestCase("A11/1")]
    [TestCase("1AA/1")]
    [TestCase("11A/1")]
    [TestCase("A1A1")]
    [TestCase("1")]
    [TestCase("12")]
    [TestCase("123/")]
    [TestCase("11A/1234567")]
    [TestCase("1A1/1234567")]
    [TestCase("A11/12345678")]
    [TestCase("AAA")]
    [TestCase("AAA/")]
    [TestCase("AAA/1")]
    [TestCase("AAA/12")]
    [TestCase("A1A/1234567")]
    [TestCase("A11/12345678")]
    [TestCase("A11/123456789")]
    [TestCase("A11/1234567890")]
    public async Task CreateAddAccountRequestCommand_Invalid_Paye(string? paye)
    {
        var sut = new CreateAddAccountRequestCommandValidator(
            _requestReadRepositoryValidMock.Object,
            _providersReadRepositoryValidMock.Object,
            _accountLegalEntityReadRepositoryValidMock.Object
        );
        var result = await sut.TestValidateAsync(new CreateAddAccountRequestCommand { Ukprn = 10000002, AccountLegalEntityId = 1, RequestedBy = Guid.NewGuid().ToString(), EmployerContactEmail = "test@email.com", Paye = paye, Operations = [Operation.CreateCohort] });
        result.ShouldHaveValidationErrorFor(q => q.Paye)
            .WithErrorMessage(CreateAddAccountRequestCommandValidator.InvalidPayeErrorMessage);
    }
}
