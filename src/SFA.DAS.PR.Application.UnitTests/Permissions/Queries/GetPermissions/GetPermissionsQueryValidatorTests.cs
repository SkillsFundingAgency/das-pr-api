using FluentValidation.TestHelper;
using SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Queries.GetPermissions;
public class GetPermissionsQueryValidatorTests
{
    [Test]
    public async Task Validate_AccountHashedId_Valid_Query()
    {
        var sut = new GetPermissionsQueryValidator();
        var result = await sut.TestValidateAsync(new GetPermissionsQuery("hash"));
        result.ShouldNotHaveValidationErrorFor(query => query.AccountHashedId);
    }

    [Test]
    public async Task Validate_AccountHashedId_Returns_ErrorMessage()
    {
        var sut = new GetPermissionsQueryValidator();
        var result = await sut.TestValidateAsync(new GetPermissionsQuery(string.Empty));
        result.ShouldHaveValidationErrorFor(q => q.AccountHashedId)
                    .WithErrorMessage(GetPermissionsQueryValidator.AccountHashedIdValidationMessage);
    }
}
