using FluentValidation.TestHelper;
using SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;

namespace SFA.DAS.PR.Application.UnitTests.AccountProviders.Queries.GetAccountProviders;

public class GetAccountProvidersQueryValidatorTests
{
    [Test]
    public async Task Validate_AccountHashedId_Returns_ErrorMessage()
    {
        var sut = new GetAccountProvidersQueryValidator();
        var result = await sut.TestValidateAsync(new GetAccountProvidersQuery(0));
        result.ShouldHaveValidationErrorFor(q => q.AccountId)
                    .WithErrorMessage(GetAccountProvidersQueryValidator.AccountProvidersIdValidationMessage);
    }

    [Test]
    public async Task Validate_AccountHashedId_Valid_Query()
    {
        var sut = new GetAccountProvidersQueryValidator();
        var result = await sut.TestValidateAsync(new GetAccountProvidersQuery(1));

        result.ShouldNotHaveValidationErrorFor(query => query.AccountId);
    }
}
