using AutoFixture.NUnit3;
using FluentValidation.TestHelper;
using Moq;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Application.Permissions.Commands.PostPermissions;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Commands.PostPermissions;

public class PostPermissionsCommandValidatorTest
{
    private readonly Mock<IAccountLegalEntityReadRepository> _accountLegalEntityReadRepositoryMock = new Mock<IAccountLegalEntityReadRepository>();

    private Mock<IProviderReadRepository> _providerReadRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _providerReadRepositoryMock = new Mock<IProviderReadRepository>();
        _providerReadRepositoryMock.Setup(a => a.ProviderExists(It.IsAny<long>(), CancellationToken.None)).ReturnsAsync(true);
    }

    [InlineAutoData("5d143e25-79a9-4f45-996b-0df709fca713", true)]
    [InlineAutoData("00000000-0000-0000-0000-000000000000", false)]
    public async Task Validate_UserRef_Valid_Command(string userRef, bool isExpectedToBeValid, PostPermissionsCommand command)
    {
        command.UserRef = Guid.Parse(userRef);
        var sut = new PostPermissionsCommandValidator(_accountLegalEntityReadRepositoryMock.Object, _providerReadRepositoryMock.Object);

        var result = await sut.TestValidateAsync(command);

        if (isExpectedToBeValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.UserRef);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.UserRef).WithErrorMessage(PostPermissionsCommandValidator.UserRefValidationMessage);
        }
    }

    [Test]
    [AutoData]
    public async Task Validate_Operations_Valid_Command(PostPermissionsCommand command)
    {
        var sut = new PostPermissionsCommandValidator(_accountLegalEntityReadRepositoryMock.Object, _providerReadRepositoryMock.Object);
        command.Operations = new List<Operation> { Operation.CreateCohort };

        var result = await sut.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(query => query.Operations);
    }

    [Test]
    [AutoData]
    public async Task Validate_Operations_Returns_InvalidCombination_ErrorMessage(PostPermissionsCommand command)
    {
        command.Operations = new List<Operation> { Operation.CreateCohort, Operation.CreateCohort };
        var sut = new PostPermissionsCommandValidator(_accountLegalEntityReadRepositoryMock.Object, _providerReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(q => q.Operations).WithErrorMessage(OperationsValidator.OperationsCombinationValidationMessage);
    }
}
