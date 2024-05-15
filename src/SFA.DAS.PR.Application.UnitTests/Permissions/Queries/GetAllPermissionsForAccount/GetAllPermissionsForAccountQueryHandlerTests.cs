using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Queries.GetAllPermissionsForAccount;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Queries.GetAllPermissionsForAccount;
public class GetAllPermissionsForAccountQueryHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_GetPermissions_Returns_Populated_Result(
            [Frozen] Mock<IPermissionsReadRepository> permissionsReadRepository,
            GetAllPermissionsForAccountQueryHandler sut,
            Account account,
            CancellationToken cancellationToken
        )
    {
        GetAllPermissionsForAccountQuery query = new(account.HashedId);

        permissionsReadRepository.Setup(a =>
            a.GetPermissions(query.AccountHashedId, cancellationToken)
        ).ReturnsAsync(account);

        ValidatedResponse<GetAllPermissionsForAccountQueryResult> result = await sut.Handle(query, cancellationToken);

        Assert.That(result.Result.LegalEntities, !Is.Empty);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_GetPermissions_Returns_Empty_Result (
            [Frozen] Mock<IPermissionsReadRepository> permissionsReadRepository,
            GetAllPermissionsForAccountQueryHandler sut,
            string accountHashedId,
            CancellationToken cancellationToken
        )
    {
        GetAllPermissionsForAccountQuery query = new(accountHashedId);

        permissionsReadRepository.Setup(a =>
            a.GetPermissions(query.AccountHashedId, cancellationToken)
        ).ReturnsAsync((Account?)null);

        ValidatedResponse<GetAllPermissionsForAccountQueryResult> result = await sut.Handle(query, cancellationToken);

        Assert.That(result.Result.LegalEntities, Is.Empty);
    }
}