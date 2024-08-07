﻿using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Queries.GetPermissions;
public class GetPermissionsQueryHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_PermissionsChecked_Returns_GetPermissionsQueryResult(
        [Frozen] Mock<IAccountProviderLegalEntitiesReadRepository> readRepository,
        GetPermissionsQueryHandler sut,
        AccountProviderLegalEntity accountProviderLegalEntity,
        List<Operation> response,
        GetPermissionsQuery query,
        CancellationToken cancellationToken
    )
    {
        GetPermissionsQueryResult getPermissionQueryResult = new GetPermissionsQueryResult
        {
            Operations = response,
            AccountLegalEntityName = accountProviderLegalEntity.AccountLegalEntity.Name,
            ProviderName = accountProviderLegalEntity.AccountProvider.Provider.Name
        };

        readRepository.Setup(a =>
            a.GetAccountProviderLegalEntity(query.Ukprn.GetValueOrDefault(), query.accountLegalEntityId.GetValueOrDefault(), cancellationToken)
        ).ReturnsAsync(accountProviderLegalEntity);

        ValidatedResponse<GetPermissionsQueryResult?> result = await sut.Handle(query, cancellationToken);

        result.Result.Should().BeEquivalentTo(getPermissionQueryResult);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_PermissionsChecked_Returns_Null_ValidResponse(
        [Frozen] Mock<IAccountProviderLegalEntitiesReadRepository> readRepository,
        GetPermissionsQueryHandler sut,
        GetPermissionsQuery query,
        CancellationToken cancellationToken
    )
    {
        readRepository.Setup(a =>
            a.GetAccountProviderLegalEntity(query.Ukprn.GetValueOrDefault(), query.accountLegalEntityId.GetValueOrDefault(), cancellationToken)
        ).ReturnsAsync((AccountProviderLegalEntity?)null);

        ValidatedResponse<GetPermissionsQueryResult?> result = await sut.Handle(query, cancellationToken);

        result.Result.Should().BeNull();
        result.IsValidResponse.Should().BeTrue();
    }
}
