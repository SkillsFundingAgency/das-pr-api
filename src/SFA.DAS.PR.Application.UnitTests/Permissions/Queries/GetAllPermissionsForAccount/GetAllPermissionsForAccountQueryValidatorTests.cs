using FluentValidation.TestHelper;
using SFA.DAS.PR.Application.Permissions.Queries.GetAllPermissionsForAccount;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Queries.GetAllPermissionsForAccount;
public class GetAllPermissionsForAccountQueryValidatorTests
{
    [Test]
    public async Task Validate_AccountHashedId_Valid_Query()
    {
        var sut = new GetAllPermissionsForAccountQueryValidator();
        var result = await sut.TestValidateAsync(new GetAllPermissionsForAccountQuery("hash"));
        result.ShouldNotHaveValidationErrorFor(query => query.AccountHashedId);
    }

    [Test]
    public async Task Validate_AccountHashedId_Returns_ErrorMessage()
    {
        var sut = new GetAllPermissionsForAccountQueryValidator();
        var result = await sut.TestValidateAsync(new GetAllPermissionsForAccountQuery(string.Empty));
        result.ShouldHaveValidationErrorFor(q => q.AccountHashedId)
                    .WithErrorMessage(GetAllPermissionsForAccountQueryValidator.AccountHashedIdValidationMessage);
    }
}
