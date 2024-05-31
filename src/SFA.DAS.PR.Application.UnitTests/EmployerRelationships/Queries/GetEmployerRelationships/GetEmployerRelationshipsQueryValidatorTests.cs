using AutoFixture.NUnit3;
using FluentValidation.TestHelper;
using Moq;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Application.Permissions.Queries.GetEmployerRelationships;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.EmployerRelationships.Queries.GetEmployerRelationships;
public class GetEmployerRelationshipsQueryValidatorTests
{
    [Test]
    [MoqAutoData]
    public async Task ValidateAccountHashedId_Valid(
        [Frozen]Mock<IAccountReadRepository> accountReadRepository,
        string accountHashedId
    )
    {
        accountReadRepository.Setup(a => a.AccountExists(accountHashedId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var sut = new GetEmployerRelationshipsQueryValidator(accountReadRepository.Object);
        var result = await sut.TestValidateAsync(new GetEmployerRelationshipsQuery(accountHashedId));
        result.ShouldNotHaveValidationErrorFor(query => query.AccountHashedId);
    }

    [Test]
    [MoqAutoData]
    public async Task ValidateAccountHashedId_Empty_Invalid(
        [Frozen] Mock<IAccountReadRepository> accountReadRepository
    )
    {
        accountReadRepository.Setup(a => a.AccountExists(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var sut = new GetEmployerRelationshipsQueryValidator(accountReadRepository.Object);
        var result = await sut.TestValidateAsync(new GetEmployerRelationshipsQuery(string.Empty));
        result.ShouldHaveValidationErrorFor(q => q.AccountHashedId)
                    .WithErrorMessage(AccountValidator.AccountHashedIdValidationMessage);
    }

    [Test]
    [MoqAutoData]
    public async Task ValidateAccountHashedId_AccountExists_Returns_False(
        [Frozen] Mock<IAccountReadRepository> accountReadRepository
    )
    {
        accountReadRepository.Setup(a => a.AccountExists(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var sut = new GetEmployerRelationshipsQueryValidator(accountReadRepository.Object);
        var result = await sut.TestValidateAsync(new GetEmployerRelationshipsQuery("hash"));
        result.ShouldHaveValidationErrorFor(q => q.AccountHashedId)
                    .WithErrorMessage(AccountValidator.AccountNotExistValidationMessage);
    }

    [Test]
    [MoqAutoData]
    public async Task ValidateUkprn_Valid(
        [Frozen] Mock<IAccountReadRepository> accountReadRepository
    )
    {
        accountReadRepository.Setup(a => a.AccountExists(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var sut = new GetEmployerRelationshipsQueryValidator(accountReadRepository.Object);
        var result = await sut.TestValidateAsync(new GetEmployerRelationshipsQuery("hash", 10000003));
        result.ShouldNotHaveValidationErrorFor(query => query.Ukprn);
    }

    [Test]
    [MoqAutoData]
    public async Task ValidateUkprn_Invalid(
        [Frozen] Mock<IAccountReadRepository> accountReadRepository
    )
    {
        accountReadRepository.Setup(a => a.AccountExists(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var sut = new GetEmployerRelationshipsQueryValidator(accountReadRepository.Object);
        var result = await sut.TestValidateAsync(new GetEmployerRelationshipsQuery("hash", 1001, "hash"));
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
                    .WithErrorMessage(UkprnFormatValidator.UkprnFormatValidationMessage);
    }

    [Test]
    [MoqAutoData]
    public async Task ValidateConditionaProperties_Missing_PublicHashedId_Invalid(
        [Frozen] Mock<IAccountReadRepository> accountReadRepository
    )
    {
        accountReadRepository.Setup(a => a.AccountExists(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var sut = new GetEmployerRelationshipsQueryValidator(accountReadRepository.Object);
        var result = await sut.TestValidateAsync(new GetEmployerRelationshipsQuery("hash", 10000001, null));
        result.ShouldHaveValidationErrorFor(q => q.AccountlegalentityPublicHashedId)
                    .WithErrorMessage(GetEmployerRelationshipsQueryValidator.ConditionalParamsValidationMessage);
    }

    [Test]
    [MoqAutoData]
    public async Task ValidateConditionaProperties_Missing_Ukprn_Invalid(
        [Frozen] Mock<IAccountReadRepository> accountReadRepository
    )
    {
        accountReadRepository.Setup(a => a.AccountExists(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var sut = new GetEmployerRelationshipsQueryValidator(accountReadRepository.Object);
        var result = await sut.TestValidateAsync(new GetEmployerRelationshipsQuery("hash", null, "hash"));
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
                    .WithErrorMessage(GetEmployerRelationshipsQueryValidator.ConditionalParamsValidationMessage);
    }

    [Test]
    [MoqAutoData]
    public async Task ValidateConditionaProperties_Valid_Parameters(
        [Frozen] Mock<IAccountReadRepository> accountReadRepository
    )
    {
        accountReadRepository.Setup(a => a.AccountExists(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var sut = new GetEmployerRelationshipsQueryValidator(accountReadRepository.Object);
        var result = await sut.TestValidateAsync(new GetEmployerRelationshipsQuery("hash", 10000003, "hash"));
        result.ShouldNotHaveValidationErrorFor(query => query.Ukprn);
        result.ShouldNotHaveValidationErrorFor(query => query.AccountlegalentityPublicHashedId);
    }
}
