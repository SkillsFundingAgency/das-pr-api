using FluentValidation.TestHelper;
using SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;
using SFA.DAS.PR.Application.Validators;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Queries.GetPermissions;
public class GetPermissionsQueryValidatorTests
{
    [Test]
    public async Task Validate_AccountHashedIdEmpty_Returns_ErrorMessage()
    {
        var sut = new GetPermissionsQueryValidator();
        GetPermissionsQuery query = GetConstructedGetPermissionsQuery();
        query.accountLegalEntityId = null;

        var result = await sut.TestValidateAsync(query);
        result.ShouldHaveValidationErrorFor(q => q.accountLegalEntityId)
            .WithErrorMessage(GetPermissionsQueryValidator.LegalEntityIdNotSuppliedValidationMessage);
    }

    [Test]
    public async Task Validate_UkprnEmpty_Returns_ErrorMessage()
    {
        var sut = new GetPermissionsQueryValidator();
        GetPermissionsQuery query = GetConstructedGetPermissionsQuery();
        query.Ukprn = null;
        var result = await sut.TestValidateAsync(query);

        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
            .WithErrorMessage(GetPermissionsQueryValidator.UkprnNotSuppliedValidationMessage);
    }

    [TestCase(1)]
    [TestCase(11)]
    [TestCase(111)]
    [TestCase(1111)]
    [TestCase(11111)]
    [TestCase(111111)]
    [TestCase(1111111)]
    [TestCase(1111111111)]
    [TestCase(211111111)]
    public async Task Validate_UkprnWrongFormat_Returns_ErrorMessage(long ukprn)
    {
        var sut = new GetPermissionsQueryValidator();
        GetPermissionsQuery query = GetConstructedGetPermissionsQuery();
        query.Ukprn = ukprn;
        var result = await sut.TestValidateAsync(query);

        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
            .WithErrorMessage(UkprnFormatValidator.UkprnFormatValidationMessage);
    }

    [Test]
    public async Task Validate_Valid_Query()
    {
        var sut = new GetPermissionsQueryValidator();
        GetPermissionsQuery query = GetConstructedGetPermissionsQuery();
        var result = await sut.TestValidateAsync(query);

        result.ShouldNotHaveAnyValidationErrors();
    }

    private GetPermissionsQuery GetConstructedGetPermissionsQuery()
    {
        return new GetPermissionsQuery
        {
            Ukprn = 12345678,
            accountLegalEntityId = 2
        };
    }
}
