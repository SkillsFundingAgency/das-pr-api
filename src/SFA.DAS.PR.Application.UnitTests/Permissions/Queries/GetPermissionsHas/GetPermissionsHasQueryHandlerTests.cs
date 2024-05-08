using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Queries.GetPermissionsHas;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Queries.GetPermissionsHas;
public class GetPermissionsHasQueryHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_PermissionsChecked_ReturnsAccountProvidersResult(
        [Frozen] Mock<IPermissionsReadRespository> readRepository,
        GetPermissionHasHandler sut,
        bool response,
        GetPermissionsHasQuery query,
        CancellationToken cancellationToken
    )
    {
        GetPermissionsHasResult getPermissionsHasResult = new GetPermissionsHasResult { HasPermissions = response };

        readRepository.Setup(a =>
            a.GetPermissionsHas(query.Ukprn.GetValueOrDefault(), query.PublicHashedId!, query.Operations!, cancellationToken)
        ).ReturnsAsync(response);

        ValidatedResponse<GetPermissionsHasResult> result =
            await sut.Handle(query, cancellationToken);

        result.Result.Should().BeEquivalentTo(getPermissionsHasResult);
    }
}