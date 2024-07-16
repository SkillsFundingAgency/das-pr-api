using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.PR.Application.ProviderRelationships.Queries.GetProviderRelationships;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.PR.Domain.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.ProviderRelationships.Queries.GetProviderRelationships;

public class GetProviderRelationshipsQueryHandlerTests
{
    [Test]
    [RecursiveMoqInlineAutoData(0, 0)]
    [RecursiveMoqInlineAutoData(-1, 0)]
    [RecursiveMoqInlineAutoData(1, 0)]
    public async Task Handler_DefualtsPageNumber(
        int pageNumber,
        int expectedPageNumber,
        [Frozen] Mock<IProviderRelationshipsReadRepository> mockRepo,
        GetProviderRelationshipsQueryHandler sut,
        GetProviderRelationshipsQuery query,
        CancellationToken cancellationToken)
    {
        query.PageNumber = pageNumber;
        await sut.Handle(query, cancellationToken);

        mockRepo.Verify(m => m.GetProviderRelationships(
                It.Is<ProviderRelationshipsQueryFilters>(o => o.PageNumber == expectedPageNumber),
                cancellationToken),
            Times.Once);
    }

    [Test]
    [RecursiveMoqInlineAutoData(0, 15)]
    [RecursiveMoqInlineAutoData(-1, 15)]
    [RecursiveMoqInlineAutoData(1, 1)]
    public async Task Handler_DefualtsPageSize(
        int pageSize,
        int expectedPageSize,
        [Frozen] Mock<IProviderRelationshipsReadRepository> mockRepo,
        GetProviderRelationshipsQueryHandler sut,
        GetProviderRelationshipsQuery query,
        CancellationToken cancellationToken)
    {
        query.PageSize = pageSize;
        await sut.Handle(query, cancellationToken);

        mockRepo.Verify(m => m.GetProviderRelationships(
                It.Is<ProviderRelationshipsQueryFilters>(o => o.PageSize == expectedPageSize),
                cancellationToken),
            Times.Once);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handler_PassesAllTheFiltersToRepository(
        [Frozen] Mock<IProviderRelationshipsReadRepository> mockRepo,
        GetProviderRelationshipsQueryHandler sut,
        GetProviderRelationshipsQuery query,
        CancellationToken cancellationToken)
    {
        query.PageNumber = 1;
        query.PageSize = 10;

        await sut.Handle(query, cancellationToken);

        mockRepo.Verify(m => m.GetProviderRelationships(
                It.Is<ProviderRelationshipsQueryFilters>(o =>
                    o.PageSize == query.PageSize
                    && o.PageNumber == 0
                    && o.HasPendingRequest == query.HasPendingRequest
                    && o.HasCreateCohortPermission == query.HasCreateCohortPermission
                    && o.HasRecruitWithReviewPermission == query.HasRecruitWithReviewPermission
                    && o.EmployerName == query.EmployerName
                    && o.HasRecruitPermission == query.HasRecruitPermission
                    && o.HasNoRecruitPermissions == query.HasNoRecruitPermissions
                    && o.Ukprn == query.Ukprn),
                cancellationToken),
            Times.Once);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handler_ReturnsResults(
        [Frozen] Mock<IProviderRelationshipsReadRepository> mockRepo,
        GetProviderRelationshipsQueryHandler sut,
        GetProviderRelationshipsQuery query,
        List<ProviderRelationship> searchResult,
        int totalCount,
        CancellationToken cancellationToken)
    {
        query.PageNumber = 3;
        query.PageSize = 100;
        mockRepo.Setup(r => r.GetProviderRelationships(It.IsAny<ProviderRelationshipsQueryFilters>(), cancellationToken)).ReturnsAsync((searchResult, totalCount));

        var result = await sut.Handle(query, cancellationToken);

        result.Result!.Employers.Should().HaveCount(searchResult.Count);
        result.Result.TotalCount.Should().Be(totalCount);
        result.Result.PageNumber.Should().Be(query.PageNumber);
        result.Result.PageSize.Should().Be(query.PageSize);
    }
}
