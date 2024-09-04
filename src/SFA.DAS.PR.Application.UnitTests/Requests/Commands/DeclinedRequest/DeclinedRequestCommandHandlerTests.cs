using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Requests.Commands.CreatePermissionRequest;
using SFA.DAS.PR.Application.Requests.Commands.DeclinedRequest;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Requests.Commands.DeclinedRequest;

public sealed class DeclinedRequestCommandHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_DeclinedRequestCommand_Successful(
        Request request,
        DeclinedRequestCommand command
    )
    {
        request.Id = command.RequestId;

        ValidatedResponse<Unit> result = null!;

        Request? updatedRequest = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(Handle_DeclinedRequestCommand_Successful)}")
        )
        {
            await context.Requests.AddAsync(request, CancellationToken.None);
            await context.SaveChangesAsync(CancellationToken.None);

            RequestWriteRepository requestWriteRepository = new(context);

            DeclinedRequestCommandHandler sut = new(
               context,
               requestWriteRepository
            );

            result = await sut.Handle(command, CancellationToken.None);

            updatedRequest = await context.Requests.FirstOrDefaultAsync(a => a.Id == request.Id, CancellationToken.None);
        }

        result.IsValidResponse.Should().BeTrue();

        Assert.Multiple(() =>
        {
            Assert.That(updatedRequest, Is.Not.Null, "Request must be not be null.");
            Assert.That(updatedRequest!.Status, Is.EqualTo(RequestStatus.Declined), $"Request status must be {nameof(RequestStatus.Declined)}.");
            Assert.That(updatedRequest!.ActionedBy, Is.EqualTo(command.ActionedBy), $"Request actioned by must be {command.ActionedBy}.");
        });
    }
}
