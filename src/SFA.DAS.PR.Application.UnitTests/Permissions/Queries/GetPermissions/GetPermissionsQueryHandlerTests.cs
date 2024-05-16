using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Queries.GetPermissionsForProviderOnAccount;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Queries.GetPermissions;
public class GetPermissionsQueryHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_PermissionsChecked_ReturnsAccountProvidersResult(
        [Frozen] Mock<IPermissionsReadRepository> readRepository,
        GetPermissionsForProviderOnAccountQueryHandler sut,
        List<Operation> response,
        GetPermissionsForProviderOnAccountQuery query,
        CancellationToken cancellationToken
    )
    {
        GetPermissionsForProviderOnAccountQueryResult getPermissionsHasResult = new GetPermissionsForProviderOnAccountQueryResult { Operations = response };

        readRepository.Setup(a =>
            a.GetOperations(query.Ukprn.GetValueOrDefault(), query.PublicHashedId!, cancellationToken)
        ).ReturnsAsync(response);

        ValidatedResponse<GetPermissionsForProviderOnAccountQueryResult> result =
            await sut.Handle(query, cancellationToken);

        result.Result.Should().BeEquivalentTo(getPermissionsHasResult);
    }
}
