using FluentValidation.TestHelper;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Application.EmployerRelationships.Queries.GetProviderEmployerRelationship;

namespace SFA.DAS.PR.Application.UnitTests.EmployerRelationships.Queries.GetProviderEmployerRelationship;

public class GetProviderEmployerRelationshipQueryValidatorTests
{
    [Test]
    public async Task Validate_Ukprn_Valid_Command()
    {
        var sut = new GetProviderEmployerRelationshipQueryValidator();
        var result = await sut.TestValidateAsync(new GetProviderEmployerRelationshipQuery(10000003, 1));
        result.ShouldNotHaveValidationErrorFor(query => query.Ukprn);
    }

    [Test]
    public async Task Validate_Ukprn_Returns_ErrorMessage()
    {
        var sut = new GetProviderEmployerRelationshipQueryValidator();
        var result = await sut.TestValidateAsync(new GetProviderEmployerRelationshipQuery(null, 1));
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
                    .WithErrorMessage(GetProviderEmployerRelationshipQueryValidator.UkprnValidationMessage);
    }

    [Test]
    public async Task Validate_Ukprn_Returns_FormatErrorMessage()
    {
        var sut = new GetProviderEmployerRelationshipQueryValidator();
        var result = await sut.TestValidateAsync(new GetProviderEmployerRelationshipQuery(10003, 1));
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
                    .WithErrorMessage(UkprnFormatValidator.UkprnFormatValidationMessage);
    }

    [Test]
    public async Task Validate_AccountLegalEntityId_Valid_Command()
    {
        var sut = new GetProviderEmployerRelationshipQueryValidator();
        var result = await sut.TestValidateAsync(new GetProviderEmployerRelationshipQuery(10000003, 1));
        result.ShouldNotHaveValidationErrorFor(query => query.AccountLegalEntityId);
    }

    [Test]
    public async Task Validate_AccountLegalEntityId_Returns_ErrorMessage()
    {
        var sut = new GetProviderEmployerRelationshipQueryValidator();
        var result = await sut.TestValidateAsync(new GetProviderEmployerRelationshipQuery(10000003, null));
        result.ShouldHaveValidationErrorFor(q => q.AccountLegalEntityId)
                    .WithErrorMessage(GetProviderEmployerRelationshipQueryValidator.AccountLegalEntityIdValidationMessage);
    }
}