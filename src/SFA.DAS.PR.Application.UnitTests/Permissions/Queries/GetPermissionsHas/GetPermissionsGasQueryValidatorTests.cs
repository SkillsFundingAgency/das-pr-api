using FluentValidation.TestHelper;
using SFA.DAS.PR.Application.Permissions.Queries.GetPermissionsHas;
using SFA.DAS.PR.Application.Validators;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Queries.GetPermissionsHas;
public class GetPermissionsGasQueryValidatorTests
{

    [Test]
    public async Task Validate_AccountHashedIdEmpty_Returns_ErrorMessage()
    {
        var sut = new GetPermissionsHasValidator();
        GetPermissionsHasQuery query = GetConstructedGetPermissionsHasQuery();
        query.PublicHashedId = null;

        var result = await sut.TestValidateAsync(query);
        result.ShouldHaveValidationErrorFor(q => q.PublicHashedId)
            .WithErrorMessage(GetPermissionsHasValidator.LegalEntityPublicHashedIdNotSuppliedValidationMessage);
    }

    [Test]
    public async Task Validate_UkprnEmpty_Returns_ErrorMessage()
    {
        var sut = new GetPermissionsHasValidator();
        GetPermissionsHasQuery query = GetConstructedGetPermissionsHasQuery();
        query.Ukprn = null;
        var result = await sut.TestValidateAsync(query);

        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
            .WithErrorMessage(GetPermissionsHasValidator.UkprnNotSuppliedValidationMessage);
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
        var sut = new GetPermissionsHasValidator();
        GetPermissionsHasQuery query = GetConstructedGetPermissionsHasQuery();
        query.Ukprn = ukprn;
        var result = await sut.TestValidateAsync(query);

        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
            .WithErrorMessage(UkprnFormatValidator.UkprnFormatValidationMessage);
    }

    [Test]
    public async Task Validate_OperationsEmpty_Returns_ErrorMessage()
    {
        var sut = new GetPermissionsHasValidator();
        GetPermissionsHasQuery query = GetConstructedGetPermissionsHasQuery();
        query.Operations = null;
        var result = await sut.TestValidateAsync(query);
        result.ShouldHaveValidationErrorFor(q => q.Operations)
            .WithErrorMessage(OperationsValidator.OperationFilterValidationMessage);
    }

    [Test]
    public async Task Validate_Operations_Returns_ErrorMessage()
    {
        var sut = new GetPermissionsHasValidator();
        GetPermissionsHasQuery query = GetConstructedGetPermissionsHasQuery();
        query.Operations = new List<Operation> { (Operation)3 };
        var result = await sut.TestValidateAsync(query);
        result.ShouldHaveValidationErrorFor(q => q.Operations)
            .WithErrorMessage(OperationsValidator.OperationFilterFormatValidationMessage);
    }

    [Test]
    public async Task Validate_Valid_Query()
    {
        var sut = new GetPermissionsHasValidator();
        GetPermissionsHasQuery query = GetConstructedGetPermissionsHasQuery();
        query.Operations = null;
        var result = await sut.TestValidateAsync(query);

        result.ShouldNotHaveAnyValidationErrors();
    }


    private GetPermissionsHasQuery GetConstructedGetPermissionsHasQuery()
    {
        return new GetPermissionsHasQuery
        {
            Ukprn = 12345678,
            PublicHashedId = "hashedId",
            Operations = new List<Operation> { Operation.CreateCohort, Operation.CreateCohort }
        };
    }
}
