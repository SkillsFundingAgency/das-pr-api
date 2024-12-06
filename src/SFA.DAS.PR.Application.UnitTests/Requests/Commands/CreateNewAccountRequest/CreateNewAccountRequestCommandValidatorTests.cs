using FluentValidation.TestHelper;
using Moq;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Application.Requests.Commands.CreateAddAccountRequest;
using SFA.DAS.PR.Application.Requests.Commands.CreateNewAccountRequest;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.UnitTests.Requests.Commands.CreateNewAccountRequest;

public class CreateNewAccountRequestCommandValidatorTests
{
    private readonly Mock<IProviderReadRepository> _providersReadRepositoryValidMock = new Mock<IProviderReadRepository>();
    private readonly Mock<IRequestReadRepository> _requestReadRepositoryValidMock = new Mock<IRequestReadRepository>();

    private const string Paye = "222/AAA";

    [SetUp]
    public void SetUp()
    {
        _requestReadRepositoryValidMock.Setup(a => a.RequestExists(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<RequestStatus[]>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _providersReadRepositoryValidMock.Setup(a => a.ProviderExists(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
    }

    [Test]
    public async Task CreateNewAccountRequestCommand_Valid_Command()
    {
        var sut = new CreateNewAccountRequestCommandValidator(
            _requestReadRepositoryValidMock.Object,
            _providersReadRepositoryValidMock.Object
        );

        var result = await sut.TestValidateAsync(
            new CreateNewAccountRequestCommand
            {
                Ukprn = 10000003,
                EmployerPAYE = Paye,
                RequestedBy = Guid.NewGuid().ToString(),
                EmployerContactEmail = "test@email.com",
                Operations = [Operation.CreateCohort],
                EmployerOrganisationName = "EmployerOrganisationName",
                EmployerContactFirstName = "EmployerContactFirstName",
                EmployerContactLastName = "EmployerContactLastName",
                EmployerAORN = "EmployerAORN"
            }
        );

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task CreateNewAccountRequestCommand_Invalid_Ukprn()
    {
        var sut = new CreateNewAccountRequestCommandValidator(
            _requestReadRepositoryValidMock.Object,
            _providersReadRepositoryValidMock.Object
        );

        var result = await sut.TestValidateAsync(new CreateNewAccountRequestCommand
        {
            Ukprn = 1000003,
            EmployerPAYE = Paye,
            RequestedBy = Guid.NewGuid().ToString(),
            EmployerContactEmail = "test@email.com",
            Operations = [Operation.CreateCohort],
            EmployerOrganisationName = "EmployerOrganisationName",
            EmployerContactFirstName = "EmployerContactFirstName",
            EmployerContactLastName = "EmployerContactLastName",
            EmployerAORN = "EmployerAORN"
        }
        );

        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
            .WithErrorMessage(UkprnValidator.UkprnFormatValidationMessage);
    }

    [Test]
    public async Task CreateNewAccountRequestCommand_Invalid_EmployerContactEmail()
    {
        var sut = new CreateNewAccountRequestCommandValidator(
            _requestReadRepositoryValidMock.Object,
            _providersReadRepositoryValidMock.Object
        );

        var result = await sut.TestValidateAsync(new CreateNewAccountRequestCommand
        {
            Ukprn = 10000003,
            EmployerPAYE = Paye,
            RequestedBy = Guid.NewGuid().ToString(),
            EmployerContactEmail = "invalid_email",
            Operations = [Operation.CreateCohort],
            EmployerOrganisationName = "EmployerOrganisationName",
            EmployerContactFirstName = "EmployerContactFirstName",
            EmployerContactLastName = "EmployerContactLastName",
            EmployerAORN = "EmployerAORN"
        }
        );

        result.ShouldHaveValidationErrorFor(q => q.EmployerContactEmail)
            .WithErrorMessage(CreateAddAccountRequestCommandValidator.EmployerContactEmailValidationMessage);
    }

    [TestCase("")]
    [TestCase(null)]
    public async Task CreateAddAccountRequestCommand_No_Paye(string? paye)
    {
        var sut = new CreateNewAccountRequestCommandValidator(
            _requestReadRepositoryValidMock.Object,
            _providersReadRepositoryValidMock.Object
        );
        var result = await sut.TestValidateAsync(new CreateNewAccountRequestCommand
        {
            Ukprn = 10000003,
            EmployerPAYE = paye,
            RequestedBy = Guid.NewGuid().ToString(),
            EmployerContactEmail = "test@test.com",
            Operations = [Operation.CreateCohort],
            EmployerOrganisationName = "EmployerOrganisationName",
            EmployerContactFirstName = "EmployerContactFirstName",
            EmployerContactLastName = "EmployerContactLastName",
            EmployerAORN = "EmployerAORN"
        }
        );

        result.ShouldHaveValidationErrorFor(q => q.EmployerPAYE)
            .WithErrorMessage(CreateNewAccountRequestCommandValidator.NoPayeMessage);
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
        var sut = new CreateNewAccountRequestCommandValidator(
            _requestReadRepositoryValidMock.Object,
            _providersReadRepositoryValidMock.Object
        );

        var result = await sut.TestValidateAsync(new CreateNewAccountRequestCommand
        {
            Ukprn = 10000003,
            EmployerPAYE = paye,
            RequestedBy = Guid.NewGuid().ToString(),
            EmployerContactEmail = "test@test.com",
            Operations = [Operation.CreateCohort],
            EmployerOrganisationName = "EmployerOrganisationName",
            EmployerContactFirstName = "EmployerContactFirstName",
            EmployerContactLastName = "EmployerContactLastName",
            EmployerAORN = "EmployerAORN"
        }
        );

        result.ShouldHaveValidationErrorFor(q => q.EmployerPAYE)
            .WithErrorMessage(CreateNewAccountRequestCommandValidator.InvalidPayeErrorMessage);
    }
}
