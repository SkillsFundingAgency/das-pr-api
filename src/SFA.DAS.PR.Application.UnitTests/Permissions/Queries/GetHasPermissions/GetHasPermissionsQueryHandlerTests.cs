using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Queries.GetHasPermissions;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Queries.GetHasPermissions;
public class GetHasPermissionsQueryHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_HasPermissions_ReturnsTrue(
        [Frozen] Mock<IPermissionsReadRepository> permissionsReadRepository,
        GetHasPermissionsQueryHandler sut,
        long ukprn,
        long accountLegalEntityId,
        List<Operation> operations,
        CancellationToken cancellationToken
    )
    {
        GetHasPermissionsQuery query = new()
        {
            Ukprn = ukprn,
            AccountLegalEntityId = accountLegalEntityId,
            Operations = operations
        };

        permissionsReadRepository.Setup(a =>
            a.GetOperations(ukprn, accountLegalEntityId, cancellationToken)
        ).ReturnsAsync(operations);

        ValidatedBooleanResult result = await sut.Handle(query, cancellationToken);
        result.Result.Should().BeTrue();
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_HasPermissions_ReturnsFalse(
        [Frozen] Mock<IPermissionsReadRepository> permissionsReadRepository,
        GetHasPermissionsQueryHandler sut,
        long ukprn,
        long accountLegalEntityId,
        CancellationToken cancellationToken
    )
    {
        List<Operation> operationsInQuery = new()
        {
            Operation.CreateCohort,
            Operation.Recruitment
        };

        List<Operation> returnedOperations = new()
        {
            Operation.RecruitmentRequiresReview
        };

        GetHasPermissionsQuery query = new()
        {
            Ukprn = ukprn,
            AccountLegalEntityId = accountLegalEntityId,
            Operations = returnedOperations
        };

        permissionsReadRepository.Setup(a =>
            a.GetOperations(ukprn, accountLegalEntityId, cancellationToken)
        ).ReturnsAsync(operationsInQuery);

        ValidatedBooleanResult result = await sut.Handle(query, cancellationToken);
        result.Result.Should().BeFalse();
    }
}
