using FluentValidation.TestHelper;
using Moq;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Application.Permissions.Commands.PostPermissions;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

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
}
