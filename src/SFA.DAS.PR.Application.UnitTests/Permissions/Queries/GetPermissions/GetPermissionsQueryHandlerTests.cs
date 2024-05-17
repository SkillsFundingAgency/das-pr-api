using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
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
            a.GetOperations(query.Ukprn.GetValueOrDefault(), query.PublicHashedId!, cancellationToken)
        ).ReturnsAsync(response);

        ValidatedResponse<GetPermissionsQueryResult> result =
            await sut.Handle(query, cancellationToken);

        result.Result.Should().BeEquivalentTo(getPermissionsHasResult);
    }
}
