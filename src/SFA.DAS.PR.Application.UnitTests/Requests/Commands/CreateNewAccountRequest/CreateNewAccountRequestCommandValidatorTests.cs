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
            new CreateNewAccountRequestCommand { 
                Ukprn = 10000003, 
                EmployerPAYE = "EmployerPAYE", 
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
                EmployerPAYE = "EmployerPAYE",
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
                EmployerPAYE = "EmployerPAYE",
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
}
