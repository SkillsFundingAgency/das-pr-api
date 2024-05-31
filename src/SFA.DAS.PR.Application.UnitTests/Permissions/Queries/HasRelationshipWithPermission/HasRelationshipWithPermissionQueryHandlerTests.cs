using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Queries.HasRelationshipWithPermission;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Queries.HasRelationshipWithPermission;
public class HasRelationshipWithPermissionQueryHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_HasPermissions_ReturnsTrue(
            [Frozen] Mock<IPermissionsReadRepository> permissionsReadRepository,
            HasRelationshipWithPermissionQueryHandler sut,
            long ukprn,
            Operation operation,
            CancellationToken cancellationToken
        )
    {
        HasRelationshipWithPermissionQuery query = new()
        {
            Ukprn = ukprn,
            Operation = operation
        };

        permissionsReadRepository.Setup(a =>
            a.HasPermissionWithRelationship(ukprn, operation, cancellationToken)
        ).ReturnsAsync(true);

        ValidatedResponse<bool> result = await sut.Handle(query, cancellationToken);

        result.Result.Should().BeTrue();
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_HasPermissions_ReturnsFalse(
            [Frozen] Mock<IPermissionsReadRepository> permissionsReadRepository,
            HasRelationshipWithPermissionQueryHandler sut,
            long ukprn,
            Operation operation,
            CancellationToken cancellationToken
        )
    {
        HasRelationshipWithPermissionQuery query = new()
        {
            Ukprn = ukprn,
            Operation = operation
        };

        permissionsReadRepository.Setup(a =>
            a.HasPermissionWithRelationship(ukprn, operation, cancellationToken)
        ).ReturnsAsync(false);

        ValidatedResponse<bool> result = await sut.Handle(query, cancellationToken);

        result.Result.Should().BeFalse();
    }
}
