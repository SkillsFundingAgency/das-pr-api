using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Queries.GetEmployerRelationships;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Queries.GetEmployerRelationships;
public class GetEmployerRelationshipsQueryHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_GetEmployerRelationships_Returns_Populated_Result(
            [Frozen] Mock<IEmployerRelationshipsReadRepository> employerRelationshipsReadRepository,
            GetEmployerRelationshipsQueryHandler sut,
            Account account,
            CancellationToken cancellationToken
        )
    {
        GetEmployerRelationshipsQuery query = new(account.HashedId);

        employerRelationshipsReadRepository.Setup(a =>
            a.GetRelationships(query.AccountHashedId, cancellationToken, null, string.Empty)
        ).ReturnsAsync(account);

        ValidatedResponse<GetEmployerRelationshipsQueryResult> result = await sut.Handle(query, cancellationToken);

        Assert.That(result.Result.AccountLegalEntities, !Is.Empty);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_GetEmployerRelationships_Returns_Empty_Result(
            [Frozen] Mock<IEmployerRelationshipsReadRepository> employerRelationshipsReadRepository,
            GetEmployerRelationshipsQueryHandler sut,
            string accountHashedId,
            CancellationToken cancellationToken
        )
    {
        GetEmployerRelationshipsQuery query = new(accountHashedId);

        employerRelationshipsReadRepository.Setup(a =>
            a.GetRelationships(query.AccountHashedId, cancellationToken, query.Ukprn, query.AccountlegalentityPublicHashedId)
        ).ReturnsAsync((Account?)null);

        ValidatedResponse<GetEmployerRelationshipsQueryResult> result = await sut.Handle(query, cancellationToken);

        Assert.That(result.Result.AccountLegalEntities, Is.Empty);
    }
}