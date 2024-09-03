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
        GetEmployerRelationshipsQuery query = new(account.Id);

        employerRelationshipsReadRepository.Setup(a =>
            a.GetRelationships(query.AccountId, cancellationToken)
        ).ReturnsAsync(account);

        ValidatedResponse<GetEmployerRelationshipsQueryResult> result = await sut.Handle(query, cancellationToken);

        Assert.That(result.Result!.AccountLegalEntities, !Is.Empty);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_GetEmployerRelationships_Returns_Empty_Result(
            [Frozen] Mock<IEmployerRelationshipsReadRepository> employerRelationshipsReadRepository,
            GetEmployerRelationshipsQueryHandler sut,
            long accountId,
            CancellationToken cancellationToken
        )
    {
        GetEmployerRelationshipsQuery query = new(accountId);

        employerRelationshipsReadRepository.Setup(a =>
            a.GetRelationships(query.AccountId, cancellationToken)
        ).ReturnsAsync((Account?)null);

        ValidatedResponse<GetEmployerRelationshipsQueryResult> result = await sut.Handle(query, cancellationToken);

        Assert.That(result.Result!.AccountLegalEntities, Is.Empty);
    }
}