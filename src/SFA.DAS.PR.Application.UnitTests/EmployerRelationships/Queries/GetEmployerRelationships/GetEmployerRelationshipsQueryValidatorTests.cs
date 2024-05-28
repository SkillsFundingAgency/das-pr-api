using FluentValidation.TestHelper;
using SFA.DAS.PR.Application.Common.Validators;
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

    [Test]
    public async Task ValidateUkprn_Valid()
    {
        var sut = new GetEmployerRelationshipsQueryValidator();
        var result = await sut.TestValidateAsync(new GetEmployerRelationshipsQuery("hash", 10000003));
        result.ShouldNotHaveValidationErrorFor(query => query.Ukprn);
    }

    [Test]
    public async Task ValidateUkprn_Invalid()
    {
        var sut = new GetEmployerRelationshipsQueryValidator();
        var result = await sut.TestValidateAsync(new GetEmployerRelationshipsQuery("hash", 1001, "hash"));
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
                    .WithErrorMessage(UkprnFormatValidator.UkprnFormatValidationMessage);
    }

    [Test]
    public async Task ValidateConditionaProperties_Missing_PublicHashedId_Invalid()
    {
        var sut = new GetEmployerRelationshipsQueryValidator();
        var result = await sut.TestValidateAsync(new GetEmployerRelationshipsQuery("hash", 10000001, null));
        result.ShouldHaveValidationErrorFor(q => q.AccountlegalentityPublicHashedId)
                    .WithErrorMessage(GetEmployerRelationshipsQueryValidator.ConditionalParamsValidationMessage);
    }

    [Test]
    public async Task ValidateConditionaProperties_Missing_Ukprn_Invalid()
    {
        var sut = new GetEmployerRelationshipsQueryValidator();
        var result = await sut.TestValidateAsync(new GetEmployerRelationshipsQuery("hash", null, "hash"));
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
                    .WithErrorMessage(GetEmployerRelationshipsQueryValidator.ConditionalParamsValidationMessage);
    }

    [Test]
    public async Task ValidateConditionaProperties_Valid_Parameters()
    {
        var sut = new GetEmployerRelationshipsQueryValidator();
        var result = await sut.TestValidateAsync(new GetEmployerRelationshipsQuery("hash", 10000003, "hash"));
        result.ShouldNotHaveValidationErrorFor(query => query.Ukprn);
        result.ShouldNotHaveValidationErrorFor(query => query.AccountlegalentityPublicHashedId);
    }
}
