using FluentValidation.TestHelper;
using SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;

namespace SFA.DAS.PR.Application.UnitTests.AccountProviders.Queries.GetAccountProviders
{
    public class GetAccountProvidersQueryValidatorTests
    {
        [Test]
        public async Task Validate_AccountId_Returns_ErrorMessage()
        {
            var sut = new GetAccountProvidersQueryValidator();
            var result = await sut.TestValidateAsync(new GetAccountProvidersQuery(0));
            result.ShouldHaveValidationErrorFor(q => q.AccountId)
                     .WithErrorMessage("Account ID needs to be set.");
        }

        [Test]
        public async Task Validate_AccountId_Valid_Query()
        {
            var sut = new GetAccountProvidersQueryValidator();
            var result = await sut.TestValidateAsync(new GetAccountProvidersQuery(1));

            result.ShouldNotHaveValidationErrorFor(query => query.AccountId);
            Assert.That(result.IsValid, Is.True, "The validation result should be valid.");
            Assert.That(result.Errors, Is.Empty, "There should be no validation errors.");
        }
    }
}
