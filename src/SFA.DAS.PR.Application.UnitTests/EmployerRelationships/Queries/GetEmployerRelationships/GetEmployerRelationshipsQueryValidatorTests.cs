using FluentValidation.TestHelper;
using Moq;
using SFA.DAS.PR.Application.Permissions.Queries.GetEmployerRelationships;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Queries.GetAllPermissionsForAccount;
public class GetEmployerRelationshipsQueryValidatorTests
{
    [Test]
    [MoqAutoData]
    public async Task ValidateAccountHashedId_Valid(
        Mock<IEmployerRelationshipsReadRepository> employerRelationshipsReadRepository, long accountId)
    {
        employerRelationshipsReadRepository
            .Setup(a =>
                a.AccountIdExists(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var sut = new GetEmployerRelationshipsQueryValidator(employerRelationshipsReadRepository.Object);
        var result = await sut.TestValidateAsync(new GetEmployerRelationshipsQuery(accountId));
        result.ShouldNotHaveValidationErrorFor(query => query.AccountId);
    }

    [Test]
    [MoqAutoData]
    public async Task ValidateAccountHashedId_Empty_Invalid(
        Mock<IEmployerRelationshipsReadRepository> employerRelationshipsReadRepository)
    {
        long accountId = 0;

        employerRelationshipsReadRepository
            .Setup(a =>
                a.AccountIdExists(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var sut = new GetEmployerRelationshipsQueryValidator(employerRelationshipsReadRepository.Object);
        var result = await sut.TestValidateAsync(new GetEmployerRelationshipsQuery(accountId));
        result.ShouldHaveValidationErrorFor(q => q.AccountId)
                    .WithErrorMessage(GetEmployerRelationshipsQueryValidator.AccountHashedIdValidationMessage);
    }

    [Test]
    [MoqAutoData]
    public async Task ValidateAccountId_NotFound_Invalid(
        Mock<IEmployerRelationshipsReadRepository> employerRelationshipsReadRepository, long accountId)
    {
        employerRelationshipsReadRepository
            .Setup(a =>
                a.AccountIdExists(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var sut = new GetEmployerRelationshipsQueryValidator(employerRelationshipsReadRepository.Object);
        var result = await sut.TestValidateAsync(new GetEmployerRelationshipsQuery(accountId));
        result.ShouldHaveValidationErrorFor(q => q.AccountId)
            .WithErrorMessage(GetEmployerRelationshipsQueryValidator.AccountIdDoesNotExistValidationMessage);
    }
}
