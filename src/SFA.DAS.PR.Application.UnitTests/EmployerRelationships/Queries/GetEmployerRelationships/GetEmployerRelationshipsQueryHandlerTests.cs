using AutoFixture.NUnit4;
using Moq;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Queries.GetEmployerRelationships;
using SFA.DAS.PR.Domain.Common;
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
            long accountId,
            long ukprn,
            CancellationToken cancellationToken
        )
    {
        var account = new Account
        {
            Id = accountId,
            Name = "Test Account",
            AccountLegalEntities = new List<AccountLegalEntity>()
            {
                new AccountLegalEntity
                {
                    Id = 1,
                    PublicHashedId = "ABC123",
                    AccountId = accountId,
                    Name = "Test Legal Entity",
                    AccountProviderLegalEntities = new List<AccountProviderLegalEntity>
                    {
                        new AccountProviderLegalEntity
                        {
                            Id = 1,
                            AccountLegalEntityId = 1,
                            AccountProviderId = 1,
                            Created = DateTime.UtcNow,
                            AccountProvider = new AccountProvider
                            {
                                Id = 1,
                                AccountId = accountId,
                                ProviderUkprn = ukprn,
                                Provider = new Provider
                                {
                                    Ukprn = ukprn,
                                    Name = "Test Provider",
                                    Status = null
                                },
                            }
                        }
                    }
                }
            },
        };

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

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_GetEmployerRelationships_ProviderIsRemoved_Returns_Populated_Result(
            [Frozen] Mock<IEmployerRelationshipsReadRepository> employerRelationshipsReadRepository,
            GetEmployerRelationshipsQueryHandler sut,
            long accountId,
            long ukprn,
            CancellationToken cancellationToken
        )
    {
        var account = new Account
        {
            Id = accountId,
            Name = "Test Account",
            AccountLegalEntities = new List<AccountLegalEntity>()
            {
                new AccountLegalEntity
                {
                    Id = 1,
                    PublicHashedId = "ABC123",
                    AccountId = accountId,
                    Name = "Test Legal Entity",
                    AccountProviderLegalEntities = new List<AccountProviderLegalEntity>
                    {
                        new AccountProviderLegalEntity
                        {
                            Id = 1,
                            AccountLegalEntityId = 1,
                            AccountProviderId = 1,
                            Created = DateTime.UtcNow,
                            AccountProvider = new AccountProvider
                            {
                                Id = 1,
                                AccountId = accountId,
                                ProviderUkprn = ukprn,
                                Provider = new Provider
                                {
                                    Ukprn = ukprn,
                                    Name = "Test Provider",
                                    Status = ProviderStatus.Removed
                                },
                            }
                        }
                    }
                }
            },
        };

        GetEmployerRelationshipsQuery query = new(account.Id);

        employerRelationshipsReadRepository.Setup(a =>
            a.GetRelationships(query.AccountId, cancellationToken)
        ).ReturnsAsync(account);

        ValidatedResponse<GetEmployerRelationshipsQueryResult> result = await sut.Handle(query, cancellationToken);

        Assert.That(result.Result!.AccountLegalEntities, Is.Empty);
    }
}