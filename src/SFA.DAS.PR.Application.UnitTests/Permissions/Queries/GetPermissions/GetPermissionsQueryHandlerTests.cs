using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Queries.GetPermissions;
public class GetPermissionsQueryHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_PermissionsChecked_ReturnsAccountProvidersResult(
        [Frozen] Mock<IPermissionsReadRepository> readRepository,
        GetPermissionsQueryHandler sut,
        List<Operation> response,
        GetPermissionsQuery query,
        CancellationToken cancellationToken
    )
    {
        GetPermissionsQueryResult getPermissionsHasResult = new GetPermissionsQueryResult { Operations = response };

        readRepository.Setup(a =>
            a.GetOperations(query.Ukprn.GetValueOrDefault(), query.accountLegalEntityId.GetValueOrDefault(), cancellationToken)
        ).ReturnsAsync(response);

        ValidatedResponse<GetPermissionsQueryResult> result =
            await sut.Handle(query, cancellationToken);

        result.Result.Should().BeEquivalentTo(getPermissionsHasResult);
    }
}
