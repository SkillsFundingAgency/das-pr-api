using FluentValidation.TestHelper;
using Moq;
using SFA.DAS.PR.Application.ProviderRelationships.Queries.GetProviderRelationships;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.UnitTests.ProviderRelationships.Queries.GetProviderRelationships;

public class GetProviderRelationshipsQueryValidatorTests
{
    [TestCase(null, false)]
    [TestCase(0, false)]
    [TestCase(1234567, false)]
    [TestCase(10000001, false)]
    [TestCase(10012002, true)]
    public async Task Validates_Ukprn(long? ukprn, bool isExpectedToBeValid)
    {
        Mock<IProviderReadRepository> mockRepo = new();
        mockRepo.Setup(r => r.ProviderExists(10012002, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        mockRepo.Setup(r => r.ProviderExists(10000001, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        GetProviderRelationshipsQuery target = new() { Ukprn = ukprn };

        GetProviderRelationshipsQueryValidator sut = new(mockRepo.Object);

        var result = await sut.TestValidateAsync(target);

        if (isExpectedToBeValid)
        {
            result.ShouldNotHaveValidationErrorFor(r => r.Ukprn);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(r => r.Ukprn);
        }
    }
}
