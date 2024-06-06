using FluentValidation.TestHelper;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Application.Permissions.Commands.PostPermissions;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Commands.PostPermissions;

public class PostPermissionsCommandValidatorTest
{
    [Test]
    public async Task Validate_Ukprn_Valid_Command()
    {
        var sut = new PostPermissionsCommandValidator();
        var result = await sut.TestValidateAsync(new PostPermissionsCommand { Ukprn = 10000003, AccountLegalEntityId = 1, Operations = new List<Operation> { Operation.CreateCohort }, UserRef = Guid.NewGuid() });
        result.ShouldNotHaveValidationErrorFor(query => query.Ukprn);
    }

    [Test]
    public async Task Validate_Ukprn_Returns_ErrorMessage()
    {
        var sut = new PostPermissionsCommandValidator();
        var result = await sut.TestValidateAsync(new PostPermissionsCommand { Ukprn = null, AccountLegalEntityId = 1, Operations = new List<Operation> { Operation.CreateCohort }, UserRef = Guid.NewGuid() });
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
                    .WithErrorMessage(PostPermissionsCommandValidator.UkprnValidationMessage);
    }

    [Test]
    public async Task Validate_Ukprn_Returns_FormatErrorMessage()
    {
        var sut = new PostPermissionsCommandValidator();
        var result = await sut.TestValidateAsync(new PostPermissionsCommand { Ukprn = 10003, AccountLegalEntityId = 1, Operations = new List<Operation> { Operation.CreateCohort }, UserRef = Guid.NewGuid() });
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
                    .WithErrorMessage(UkprnFormatValidator.UkprnFormatValidationMessage);
    }

    [Test]
    public async Task Validate_AccountLegalEntityId_Valid_Command()
    {
        var sut = new PostPermissionsCommandValidator();
        var result = await sut.TestValidateAsync(new PostPermissionsCommand { Ukprn = 10000003, AccountLegalEntityId = 1, Operations = new List<Operation> { Operation.CreateCohort }, UserRef = Guid.NewGuid() });
        result.ShouldNotHaveValidationErrorFor(query => query.AccountLegalEntityId);
    }

    [Test]
    public async Task Validate_AccountLegalEntityId_Returns_ErrorMessage()
    {
        var sut = new PostPermissionsCommandValidator();
        var result = await sut.TestValidateAsync(new PostPermissionsCommand { Ukprn = 10000003, AccountLegalEntityId = 0, Operations = new List<Operation> { Operation.CreateCohort }, UserRef = Guid.NewGuid() });
        result.ShouldHaveValidationErrorFor(q => q.AccountLegalEntityId)
                    .WithErrorMessage(PostPermissionsCommandValidator.AccountLegalEntityIdValidationMessage);
    }

    [Test]
    public async Task Validate_UserRef_Valid_Command()
    {
        var sut = new PostPermissionsCommandValidator();
        var result = await sut.TestValidateAsync(new PostPermissionsCommand { Ukprn = 10000003, AccountLegalEntityId = 1, Operations = new List<Operation> { Operation.CreateCohort }, UserRef = Guid.NewGuid() });
        result.ShouldNotHaveValidationErrorFor(query => query.UserRef);
    }

    [Test]
    public async Task Validate_Operations_Valid_Command()
    {
        var sut = new PostPermissionsCommandValidator();
        var result = await sut.TestValidateAsync(new PostPermissionsCommand { Ukprn = 10000003, AccountLegalEntityId = 1, Operations = new List<Operation> { Operation.CreateCohort }, UserRef = Guid.NewGuid() });
        result.ShouldNotHaveValidationErrorFor(query => query.Operations);
    }

    [Test]
    public async Task Validate_Operations_Returns_InvalidCombination_ErrorMessage()
    {
        var sut = new PostPermissionsCommandValidator();
        var result = await sut.TestValidateAsync(new PostPermissionsCommand { Ukprn = 10000003, AccountLegalEntityId = 1, Operations = new List<Operation> { Operation.CreateCohort, Operation.CreateCohort }, UserRef = Guid.NewGuid() });
        result.ShouldHaveValidationErrorFor(q => q.Operations)
                    .WithErrorMessage(OperationsValidator.OperationsCombinationValidationMessage);
    }
}