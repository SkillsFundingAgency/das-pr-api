using FluentValidation.TestHelper;
using SFA.DAS.PR.Application.AccountProviderLegalEntities.Queries.GetAccountProviderLegalEntities;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Application.UnitTests.AccountProviderLegalEntities.Queries.GetAccountProviderLegalEntities;
public class GetAccountProviderLegalEntitiesQueryValidatorTests
{
    [Test]
    public async Task GetAccountProviderLegalEntitiesQueryValidator_Validate_Ukprn_Returns_ErrorMessage()
    {
        var query = new GetAccountProviderLegalEntitiesQuery()
        {
            Ukprn = 0,
            Operations = new List<Operation>() { Operation.Recruitment }
        };
        var sut = new GetAccountProviderLegalEntitiesQueryValidator();
        var result = await sut.TestValidateAsync(query);
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
                    .WithErrorMessage(GetAccountProviderLegalEntitiesQueryValidator.UkprnFormatValidationMessage);
    }

    [Test]
    public async Task GetAccountProviderLegalEntitiesQueryValidator_Validate_AccountHashId_Returns_ErrorMessage()
    {
        var query = new GetAccountProviderLegalEntitiesQuery()
        {
            Ukprn = null,
            AccountHashedId = "",
            Operations = new List<Operation>() { Operation.Recruitment }
        };
        var sut = new GetAccountProviderLegalEntitiesQueryValidator();
        var result = await sut.TestValidateAsync(query);
        result.ShouldHaveValidationErrorFor(q => q.AccountHashedId)
                    .WithErrorMessage(GetAccountProviderLegalEntitiesQueryValidator.UkprnAccountHashIdValidationMessage);
    }

    [Test]
    public async Task GetAccountProviderLegalEntitiesQueryValidator_Validate_Operations_Returns_ErrorMessage()
    {
        var query = new GetAccountProviderLegalEntitiesQuery()
        {
            Ukprn = null,
            AccountHashedId = "Hash",
            Operations = []
        };
        var sut = new GetAccountProviderLegalEntitiesQueryValidator();
        var result = await sut.TestValidateAsync(query);
        result.ShouldHaveValidationErrorFor(q => q.Operations)
                    .WithErrorMessage(GetAccountProviderLegalEntitiesQueryValidator.OperationFilterValidationMessage);
    }
}
