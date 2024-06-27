using FluentValidation.TestHelper;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Application.Permissions.Queries.GetHasPermissions;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Queries.GetHasPermissions;
public class GetHasPermissionsQueryValidatorTests
{
    [Test]
    public async Task Validate_Ukprn_Valid_Query()
    {
        var sut = new GetHasPermissionsQueryValidator();
        var result = await sut.TestValidateAsync(new GetHasPermissionsQuery { Ukprn = 10000003, AccountLegalEntityId = 1, Operations = new List<Operation> { Operation.CreateCohort } });
        result.ShouldNotHaveValidationErrorFor(query => query.Ukprn);
    }

    [Test]
    public async Task Validate_Ukprn_Returns_ErrorMessage()
    {
        var sut = new GetHasPermissionsQueryValidator();
        var result = await sut.TestValidateAsync(new GetHasPermissionsQuery { Ukprn = null, AccountLegalEntityId = 1, Operations = new List<Operation> { Operation.CreateCohort } });
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
                    .WithErrorMessage(GetHasPermissionsQueryValidator.UkprnNotSuppliedValidationMessage);
    }

    [Test]
    public async Task Validate_Ukprn_Returns_InvalidFormat_ErrorMessage()
    {
        var sut = new GetHasPermissionsQueryValidator();
        var result = await sut.TestValidateAsync(new GetHasPermissionsQuery { Ukprn = 10002, AccountLegalEntityId = 1, Operations = new List<Operation> { Operation.CreateCohort } });
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
                    .WithErrorMessage(UkprnFormatValidator.UkprnFormatValidationMessage);
    }

    [Test]
    public async Task Validate_Operation_Valid_Query()
    {
        var sut = new GetHasPermissionsQueryValidator();
        var result = await sut.TestValidateAsync(new GetHasPermissionsQuery { Ukprn = 12345678, AccountLegalEntityId = 1, Operations = new List<Operation> { Operation.Recruitment } });

        result.ShouldNotHaveValidationErrorFor(query => query.Operations);
    }

    [Test]
    public async Task Validate_Operation_Returns_ErrorMessage()
    {
        var sut = new GetHasPermissionsQueryValidator();
        var result = await sut.TestValidateAsync(new GetHasPermissionsQuery { Ukprn = 12345678, AccountLegalEntityId = 1, Operations = new List<Operation>() });
        result.ShouldHaveValidationErrorFor(q => q.Operations)
                    .WithErrorMessage(OperationsValidator.OperationsFilterValidationMessage);
    }

    [Test]
    public async Task Validate_AccountLegalEntityId_Valid_Query()
    {
        var sut = new GetHasPermissionsQueryValidator();
        var result = await sut.TestValidateAsync(new GetHasPermissionsQuery { Ukprn = 12345678, AccountLegalEntityId = 1, Operations = new List<Operation> { Operation.Recruitment } });

        result.ShouldNotHaveValidationErrorFor(query => query.AccountLegalEntityId);
    }

    [Test]
    public async Task Validate_AccountLegalEntityId_Returns_ErrorMessage()
    {
        var sut = new GetHasPermissionsQueryValidator();
        var result = await sut.TestValidateAsync(new GetHasPermissionsQuery { Ukprn = 12345678, AccountLegalEntityId = null, Operations = new List<Operation> { Operation.Recruitment } });
        result.ShouldHaveValidationErrorFor(q => q.AccountLegalEntityId)
            .WithErrorMessage(GetHasPermissionsQueryValidator.AccountLegalEntityIdNotSuppliedValidationMessage);
    }
}
