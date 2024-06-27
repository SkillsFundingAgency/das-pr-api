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

    [Test]
    public async Task Validate_UserRef_Valid_Command()
    {
        var sut = new PostPermissionsCommandValidator(_accountLegalEntityReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(new PostPermissionsCommand { Ukprn = 10000003, AccountLegalEntityId = 1, Operations = new List<Operation> { Operation.CreateCohort }, UserRef = Guid.NewGuid() });
        result.ShouldNotHaveValidationErrorFor(query => query.UserRef);
    }

    [Test]
    public async Task Validate_Operations_Valid_Command()
    {
        var sut = new PostPermissionsCommandValidator(_accountLegalEntityReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(new PostPermissionsCommand { Ukprn = 10000003, AccountLegalEntityId = 1, Operations = new List<Operation> { Operation.CreateCohort }, UserRef = Guid.NewGuid() });
        result.ShouldNotHaveValidationErrorFor(query => query.Operations);
    }

    [Test]
    public async Task Validate_Operations_Returns_InvalidCombination_ErrorMessage()
    {
        var sut = new PostPermissionsCommandValidator(_accountLegalEntityReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(new PostPermissionsCommand { Ukprn = 10000003, AccountLegalEntityId = 1, Operations = new List<Operation> { Operation.CreateCohort, Operation.CreateCohort }, UserRef = Guid.NewGuid() });
        result.ShouldHaveValidationErrorFor(q => q.Operations)
                    .WithErrorMessage(OperationsValidator.OperationsCombinationValidationMessage);
    }

    [TestCase("FirstName", true)]
    [TestCase("", false)]
    [TestCase(null, false)]
    public async Task Validate_UserFirstName(string firstName, bool isExpectedToBeValid)
    {
        PostPermissionsCommandValidator sut = new(Mock.Of<IAccountLegalEntityReadRepository>());
        var result = await sut.TestValidateAsync(new PostPermissionsCommand { UserFirstName = firstName });

        if (isExpectedToBeValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.UserFirstName);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.UserFirstName).WithErrorMessage(PostPermissionsCommandValidator.UserFirstNameValidationMessage);
        }
    }

    [TestCase("LastName", true)]
    [TestCase("", false)]
    [TestCase(null, false)]
    public async Task Validate_UserLastName(string lastName, bool isExpectedToBeValid)
    {
        PostPermissionsCommandValidator sut = new(Mock.Of<IAccountLegalEntityReadRepository>());
        var result = await sut.TestValidateAsync(new PostPermissionsCommand { UserLastName = lastName });

        if (isExpectedToBeValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.UserLastName);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.UserLastName).WithErrorMessage(PostPermissionsCommandValidator.UserLastNameValidationMessage);
        }
    }

    [TestCase("abc@gmail.com", true)]
    [TestCase("", false)]
    [TestCase(null, false)]
    public async Task Validate_UserEmail(string email, bool isExpectedToBeValid)
    {
        PostPermissionsCommandValidator sut = new(Mock.Of<IAccountLegalEntityReadRepository>());
        var result = await sut.TestValidateAsync(new PostPermissionsCommand { UserEmail = email });

        if (isExpectedToBeValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.UserEmail);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.UserEmail).WithErrorMessage(PostPermissionsCommandValidator.UserEmailValidationMessage);
        }
    }
}
