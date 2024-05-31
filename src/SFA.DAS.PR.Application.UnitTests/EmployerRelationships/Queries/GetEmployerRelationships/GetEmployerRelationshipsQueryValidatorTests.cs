using FluentValidation.TestHelper;
using SFA.DAS.PR.Application.Permissions.Queries.GetEmployerRelationships;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Queries.GetAllPermissionsForAccount;
public class GetEmployerRelationshipsQueryValidatorTests
{
    [Test]
    public async Task ValidateAccountHashedId_Valid()
    {
        var sut = new GetEmployerRelationshipsQueryValidator();
        var result = await sut.TestValidateAsync(new GetEmployerRelationshipsQuery("hash"));
        result.ShouldNotHaveValidationErrorFor(query => query.AccountHashedId);
    }

    [Test]
    public async Task ValidateAccountHashedId_Empty_Invalid()
    {
        var sut = new GetEmployerRelationshipsQueryValidator();
        var result = await sut.TestValidateAsync(new GetEmployerRelationshipsQuery(string.Empty));
        result.ShouldHaveValidationErrorFor(q => q.AccountHashedId)
                    .WithErrorMessage(GetEmployerRelationshipsQueryValidator.AccountHashedIdValidationMessage);
    }
}
