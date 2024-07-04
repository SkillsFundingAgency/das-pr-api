using FluentValidation.TestHelper;
using Moq;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Application.Permissions.Commands.RemovePermissions;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Commands.RemovePermissions;
public class RemovePermissionsCommandValidatorTests
{
    private readonly Mock<IAccountLegalEntityReadRepository> _accountLegalEntityReadRepositoryMock = new Mock<IAccountLegalEntityReadRepository>();
    private readonly Mock<IProvidersReadRepository> _providersReadRepositoryMock = new Mock<IProvidersReadRepository>();
    [Test]
    public async Task Validate_UserRef_Valid_Command()
    {
        var sut = new RemovePermissionsCommandValidator(_accountLegalEntityReadRepositoryMock.Object, _providersReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(new RemovePermissionsCommand { Ukprn = 10000003, AccountLegalEntityId = 1, UserRef = Guid.NewGuid() });
        result.ShouldNotHaveValidationErrorFor(query => query.UserRef);
    }

    [Test]
    public async Task Validate_Ukprn_Returns_InvalidUkprn_ErrorMessage()
    {
        var sut = new RemovePermissionsCommandValidator(_accountLegalEntityReadRepositoryMock.Object, _providersReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(new RemovePermissionsCommand { Ukprn = 10000003, AccountLegalEntityId = 1, UserRef = Guid.NewGuid() });
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
            .WithErrorMessage(ProviderValidator.ProviderExistValidationMessage);
    }

    [Test]
    public async Task Validate_AccountLegalEntityId_Returns_Invalid_ErrorMessage()
    {
        var sut = new RemovePermissionsCommandValidator(_accountLegalEntityReadRepositoryMock.Object, _providersReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(new RemovePermissionsCommand { Ukprn = 10000003, AccountLegalEntityId = 1, UserRef = Guid.NewGuid() });
        result.ShouldHaveValidationErrorFor(q => q.AccountLegalEntityId)
            .WithErrorMessage(AccountLegalEntityValidator.AccountLegalEntityExistValidationMessage);
    }
}
