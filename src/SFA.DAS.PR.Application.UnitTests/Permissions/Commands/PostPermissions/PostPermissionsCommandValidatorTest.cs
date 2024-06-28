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

    [InlineAutoData("5d143e25-79a9-4f45-996b-0df709fca713", true)]
    [InlineAutoData("00000000-0000-0000-0000-000000000000", false)]
    public async Task Validate_UserRef_Valid_Command(string userRef, bool isExpectedToBeValid, PostPermissionsCommand command)
    {
        command.UserRef = Guid.Parse(userRef);
        var sut = new PostPermissionsCommandValidator(_accountLegalEntityReadRepositoryMock.Object);

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
        command.Operations = new List<Operation> { Operation.CreateCohort };
        var sut = new PostPermissionsCommandValidator(_accountLegalEntityReadRepositoryMock.Object);

        var result = await sut.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(query => query.Operations);
    }

    [Test]
    [AutoData]
    public async Task Validate_Operations_Returns_InvalidCombination_ErrorMessage(PostPermissionsCommand command)
    {
        command.Operations = new List<Operation> { Operation.CreateCohort, Operation.CreateCohort };
        var sut = new PostPermissionsCommandValidator(_accountLegalEntityReadRepositoryMock.Object);

        var result = await sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(q => q.Operations).WithErrorMessage(OperationsValidator.OperationsCombinationValidationMessage);
    }

    [TestCase("FirstName", true)]
    [TestCase("", false)]
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
