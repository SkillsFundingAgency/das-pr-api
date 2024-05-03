using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.PR.Api.Controllers;
using SFA.DAS.PR.Application.AccountProviderLegalEntities.Queries.GetAccountProviderLegalEntities;
using SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Api.UnitTests.Controllers;
public class AccountProviderLegalEntitiesControllerTests
{
    [Test]
    [MoqAutoData]
    public async Task GetAccountProviderLegalEntities_InvokesQueryHandler(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] AccountProviderLegalEntitiesController sut,
       string accountHashedId,
       List<Operation> operations,
       CancellationToken cancellationToken
    )
    {
        GetAccountProviderLegalEntitiesQuery query = new()
        {
            AccountHashedId = accountHashedId,
            Ukprn = null,
            Operations = operations
        };

        await sut.Get(query, cancellationToken);

        mediatorMock.Verify(m =>
           m.Send(
               It.Is<GetAccountProviderLegalEntitiesQuery>(q =>
                   q.AccountHashedId == accountHashedId &&
                   q.Ukprn == null &&
                   q.Operations != null &&
                   q.Operations.All(op => op == Operation.CreateCohort || op == Operation.Recruitment || op == Operation.RecruitmentRequiresReview)
               ),
               cancellationToken
           )
       );
    }

    [Test]
    [MoqAutoData]
    public async Task GetAccountProviderLegalEntities_HandlerReturnsNullResult_ReturnsNotFoundResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AccountProviderLegalEntitiesController sut,
        string accountHashedId,
        List<Operation> operations,
        CancellationToken cancellationToken
    )
    {
        var notFoundResponse = ValidatedResponse<GetAccountProviderLegalEntitiesQueryResult>.EmptySuccessResponse();

        GetAccountProviderLegalEntitiesQuery query = new()
        {
            AccountHashedId = accountHashedId,
            Ukprn = null,
            Operations = operations
        };

        mediatorMock.Setup(m => m.Send(query, cancellationToken)).ReturnsAsync(notFoundResponse);

        var result = await sut.Get(query, cancellationToken);
        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task GetAccountProviderLegalEntities_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AccountProviderLegalEntitiesController sut,
        string accountHashedId,
        List<Operation> operations,
        CancellationToken cancellationToken,
        GetAccountProviderLegalEntitiesQueryResult getAccountProviderLegalEntitiesQueryResult
    )
    {
        var response = new ValidatedResponse<GetAccountProviderLegalEntitiesQueryResult>(getAccountProviderLegalEntitiesQueryResult);

        GetAccountProviderLegalEntitiesQuery query = new()
        {
            AccountHashedId = accountHashedId,
            Ukprn = null,
            Operations = operations
        };

        mediatorMock.Setup(m =>
            m.Send(query, cancellationToken)
        ).ReturnsAsync(response);

        var result = await sut.Get(query, cancellationToken);

        result.Should().BeOfType<OkObjectResult>();
        var responseData = (result as OkObjectResult)?.Value as GetAccountProviderLegalEntitiesQueryResult;
        responseData?.AccountProviderLegalEntities.Count.Should().Be(getAccountProviderLegalEntitiesQueryResult.AccountProviderLegalEntities.Count);
    }
}