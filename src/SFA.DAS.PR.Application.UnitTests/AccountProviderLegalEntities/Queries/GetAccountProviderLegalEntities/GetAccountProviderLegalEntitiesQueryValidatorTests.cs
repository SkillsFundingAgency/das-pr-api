using FluentValidation.TestHelper;
using Moq;
using SFA.DAS.PR.Application.AccountProviderLegalEntities.Queries.GetAccountProviderLegalEntities;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.UnitTests.AccountProviderLegalEntities.Queries.GetAccountProviderLegalEntities;
public class GetAccountProviderLegalEntitiesQueryValidatorTests
{
    [Test]
    public async Task GetAccountProviderLegalEntitiesQueryValidator_Validate_Ukprn_Returns_ErrorMessage()
    {
        var _providerReadRepositoryMock = new Mock<IProviderReadRepository>();
        _providerReadRepositoryMock.Setup(a => a.ProviderExists(It.IsAny<long>(), CancellationToken.None)).ReturnsAsync(false);

        GetAccountProviderLegalEntitiesQuery query = new()
        {
            Ukprn = 0,
            Operations = new List<Operation>() { Operation.Recruitment }
        };
        var sut = new GetAccountProviderLegalEntitiesQueryValidator(_providerReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(query);
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
                    .WithErrorMessage(UkprnValidator.UkprnFormatValidationMessage);
    }

    [Test]
    public async Task GetAccountProviderLegalEntitiesQueryValidator_Validate_AccountHashId_Returns_ErrorMessage()
    {
        var _providerReadRepositoryMock = new Mock<IProviderReadRepository>();
        _providerReadRepositoryMock.Setup(a => a.ProviderExists(It.IsAny<long>(), CancellationToken.None)).ReturnsAsync(true);

        GetAccountProviderLegalEntitiesQuery query = new()
        {
            Ukprn = null,
            AccountHashedId = "",
            Operations = new List<Operation>() { Operation.Recruitment }
        };
        var sut = new GetAccountProviderLegalEntitiesQueryValidator(_providerReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(query);
        result.ShouldHaveValidationErrorFor(q => q.AccountHashedId)
                    .WithErrorMessage(GetAccountProviderLegalEntitiesQueryValidator.UkprnAccountHashIdValidationMessage);
    }

    [Test]
    public async Task GetAccountProviderLegalEntitiesQueryValidator_Validate_Operations_Returns_ErrorMessage()
    {
        var _providerReadRepositoryMock = new Mock<IProviderReadRepository>();
        _providerReadRepositoryMock.Setup(a => a.ProviderExists(It.IsAny<long>(), CancellationToken.None)).ReturnsAsync(true);

        GetAccountProviderLegalEntitiesQuery query = new()
        {
            Ukprn = null,
            AccountHashedId = "Hash",
            Operations = []
        };
        var sut = new GetAccountProviderLegalEntitiesQueryValidator(_providerReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(query);
        result.ShouldHaveValidationErrorFor(q => q.Operations)
                    .WithErrorMessage(OperationsValidator.OperationsFilterValidationMessage);
    }
}