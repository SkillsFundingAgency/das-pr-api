using FluentValidation.TestHelper;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Application.Permissions.Queries.HasRelationshipWithPermission;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Queries.HasRelationshipWithPermission;
public class HasRelationshipWithPermissionQueryValidatorTests
{
    [Test]
    public async Task Validate_Ukprn_Valid_Query()
    {
        var sut = new HasRelationshipWithPermissionQueryValidator();
        var result = await sut.TestValidateAsync(new HasRelationshipWithPermissionQuery() { Ukprn = 10000003, Operation = null });
        result.ShouldNotHaveValidationErrorFor(query => query.Ukprn);
    }

    [Test]
    public async Task Validate_Ukprn_Returns_ErrorMessage()
    {
        var sut = new HasRelationshipWithPermissionQueryValidator();
        var result = await sut.TestValidateAsync(new HasRelationshipWithPermissionQuery() { Ukprn = null, Operation = null });
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
                    .WithErrorMessage(HasRelationshipWithPermissionQueryValidator.UkprnValidationMessage);
    }

    [Test]
    public async Task Validate_Ukprn_Returns_InvalidFormat_ErrorMessage()
    {
        var sut = new HasRelationshipWithPermissionQueryValidator();
        var result = await sut.TestValidateAsync(new HasRelationshipWithPermissionQuery() { Ukprn = 10002, Operation = null });
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
                    .WithErrorMessage(UkprnFormatValidator.UkprnFormatValidationMessage);
    }

    [Test]
    public async Task Validate_Operation_Valid_Query()
    {
        var sut = new HasRelationshipWithPermissionQueryValidator();
        var result = await sut.TestValidateAsync(new HasRelationshipWithPermissionQuery() { Ukprn = null, Operation = Operation.Recruitment });

        result.ShouldNotHaveValidationErrorFor(query => query.Operation);
    }

    [Test]
    public async Task Validate_Operation_Returns_ErrorMessage()
    {
        var sut = new HasRelationshipWithPermissionQueryValidator();
        var result = await sut.TestValidateAsync(new HasRelationshipWithPermissionQuery() { Ukprn = null, Operation = null });
        result.ShouldHaveValidationErrorFor(q => q.Operation)
                    .WithErrorMessage(OperationsValidator.OperationFilterValidationMessage);
    }
}
