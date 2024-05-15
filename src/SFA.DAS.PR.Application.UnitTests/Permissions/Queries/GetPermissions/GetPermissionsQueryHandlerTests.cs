using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Queries.GetPermissions;
public class GetPermissionsQueryHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_GetPermissions_Returns_Populated_Result(
            [Frozen] Mock<IPermissionsReadRepository> permissionsReadRepository,
            GetPermissionsQueryHandler sut,
            Account account,
            CancellationToken cancellationToken
        )
    {
        GetPermissionsQuery query = new(account.HashedId);

        permissionsReadRepository.Setup(a =>
            a.GetPermissions(query.AccountHashedId, cancellationToken)
        ).ReturnsAsync(account);

        ValidatedResponse<GetPermissionsQueryResult> result = await sut.Handle(query, cancellationToken);

        Assert.That(result.Result.LegalEntities, !Is.Empty);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_GetPermissions_Returns_Empty_Result (
            [Frozen] Mock<IPermissionsReadRepository> permissionsReadRepository,
            GetPermissionsQueryHandler sut,
            string accountHashedId,
            CancellationToken cancellationToken
        )
    {
        GetPermissionsQuery query = new(accountHashedId);

        permissionsReadRepository.Setup(a =>
            a.GetPermissions(query.AccountHashedId, cancellationToken)
        ).ReturnsAsync((Account?)null);

        ValidatedResponse<GetPermissionsQueryResult> result = await sut.Handle(query, cancellationToken);

        Assert.That(result.Result.LegalEntities, Is.Empty);
    }
}