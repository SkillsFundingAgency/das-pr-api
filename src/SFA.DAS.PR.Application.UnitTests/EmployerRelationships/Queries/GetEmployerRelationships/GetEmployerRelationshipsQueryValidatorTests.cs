using FluentValidation.TestHelper;
using SFA.DAS.PR.Application.Permissions.Queries.GetEmployerRelationships;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Queries.GetAllPermissionsForAccount;
public class GetEmployerRelationshipsQueryValidatorTests
{
    [Test]
    public async Task ValidateAccountHashedId_Valid()
    {
        var sut = new GetEmployerRelationshipsQueryValidator();
        var result = await sut.TestValidateAsync(new GetEmployerRelationshipsQuery(1));
        result.ShouldNotHaveValidationErrorFor(query => query.AccountId);
    }

    [Test]
    public async Task ValidateAccountHashedId_Empty_Invalid()
    {
        var sut = new GetEmployerRelationshipsQueryValidator();
        var result = await sut.TestValidateAsync(new GetEmployerRelationshipsQuery(0));
        result.ShouldHaveValidationErrorFor(q => q.AccountId)
                    .WithErrorMessage(GetEmployerRelationshipsQueryValidator.AccountHashedIdValidationMessage);
    }
}
