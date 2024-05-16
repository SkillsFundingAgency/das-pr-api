using FluentValidation.TestHelper;
using SFA.DAS.PR.Application.Permissions.Queries.GetPermissionsForProviderOnAccount;
using SFA.DAS.PR.Application.Validators;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Queries.GetPermissions;
public class GetPermissionsQueryValidatorTests
{
    [Test]
    public async Task Validate_AccountHashedIdEmpty_Returns_ErrorMessage()
    {
        var sut = new GetPermissionsForProviderOnAccountQueryValidator();
        GetPermissionsForProviderOnAccountQuery query = GetConstructedGetPermissionsQuery();
        query.PublicHashedId = null;

        var result = await sut.TestValidateAsync(query);
        result.ShouldHaveValidationErrorFor(q => q.PublicHashedId)
            .WithErrorMessage(GetPermissionsForProviderOnAccountQueryValidator.LegalEntityPublicHashedIdNotSuppliedValidationMessage);
    }

    [Test]
    public async Task Validate_UkprnEmpty_Returns_ErrorMessage()
    {
        var sut = new GetPermissionsForProviderOnAccountQueryValidator();
        GetPermissionsForProviderOnAccountQuery query = GetConstructedGetPermissionsQuery();
        query.Ukprn = null;
        var result = await sut.TestValidateAsync(query);

        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
            .WithErrorMessage(GetPermissionsForProviderOnAccountQueryValidator.UkprnNotSuppliedValidationMessage);
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
        var sut = new GetPermissionsForProviderOnAccountQueryValidator();
        GetPermissionsForProviderOnAccountQuery query = GetConstructedGetPermissionsQuery();
        query.Ukprn = ukprn;
        var result = await sut.TestValidateAsync(query);

        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
            .WithErrorMessage(UkprnFormatValidator.UkprnFormatValidationMessage);
    }

    [Test]
    public async Task Validate_Valid_Query()
    {
        var sut = new GetPermissionsForProviderOnAccountQueryValidator();
        GetPermissionsForProviderOnAccountQuery query = GetConstructedGetPermissionsQuery();
        var result = await sut.TestValidateAsync(query);

        result.ShouldNotHaveAnyValidationErrors();
    }

    private GetPermissionsForProviderOnAccountQuery GetConstructedGetPermissionsQuery()
    {
        return new GetPermissionsForProviderOnAccountQuery
        {
            Ukprn = 12345678,
            PublicHashedId = "hashedId"
        };
    }
}
