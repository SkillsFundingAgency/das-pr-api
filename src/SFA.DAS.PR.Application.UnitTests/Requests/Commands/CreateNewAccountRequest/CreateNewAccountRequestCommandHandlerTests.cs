using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Requests.Commands.CreateAddAccountRequest;
using SFA.DAS.PR.Application.Requests.Commands.CreateNewAccountRequest;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Requests.Commands.CreateNewAccountRequest;

public class CreateNewAccountRequestCommandHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_CreateNewAccountRequest_Successful(CreateNewAccountRequestCommand command)
    {
        ValidatedResponse<CreateNewAccountRequestCommandResult> result = null!;

        Request? persistedRequest = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(Handle_CreateNewAccountRequest_Successful)}")
        )
        {
            RequestWriteRepository requestWriteRepository = new(context);

            CreateNewAccountRequestCommandHandler sut = new(
               context,
               requestWriteRepository
            );

            result = await sut.Handle(command, CancellationToken.None);

            persistedRequest = await context.Requests.FirstOrDefaultAsync(a => a.Id == result.Result!.RequestId, CancellationToken.None);
        }

        result.Result.Should().NotBeNull();

        Assert.Multiple(() =>
        {
            Assert.That(persistedRequest, Is.Not.Null, "Request must not be null.");
            Assert.That(result.Result!.RequestId, Is.EqualTo(persistedRequest!.Id), "Result RequestId must match persisted request id.");
            Assert.That(persistedRequest!.PermissionRequests, Has.Exactly(command.Operations.Count).Items, $"Request must have {command.Operations.Count} permission requests created.");
            Assert.That(persistedRequest.EmployerOrganisationName, Is.EqualTo(command.EmployerOrganisationName), $"Requests EmployerOrganisationName ({persistedRequest.EmployerOrganisationName}) must match the commands EmployerOrganisationName ({command.EmployerOrganisationName})");
            Assert.That(persistedRequest.EmployerContactFirstName, Is.EqualTo(command.EmployerContactFirstName), $"Requests EmployerContactFirstName ({persistedRequest.EmployerContactFirstName}) must match the commands EmployerContactFirstName ({command.EmployerContactFirstName})");
            Assert.That(persistedRequest.EmployerContactLastName, Is.EqualTo(command.EmployerContactLastName), $"Requests EmployerContactLastName ({persistedRequest.EmployerContactLastName}) must match the commands EmployerContactLastName ({command.EmployerContactLastName})");
        });
    }
}
