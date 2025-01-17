﻿using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Requests.Commands.CreateAddAccountRequest;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Requests.Commands.CreateAddAccountRequest;

public class CreateAddAccountRequestCommandHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_CreateAddAccountRequest_Successful(
        [Frozen] Mock<IAccountLegalEntityReadRepository> accountLegalEntityReadRepository,
        AccountLegalEntity accountLegalEntity,
        CreateAddAccountRequestCommand command
    )
    {
        accountLegalEntityReadRepository.Setup(a =>
            a.GetAccountLegalEntity(
                command.AccountLegalEntityId,
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(accountLegalEntity);

        ValidatedResponse<CreateAddAccountRequestCommandResult> result = null!;


        Request? persistedRequest = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(Handle_CreateAddAccountRequest_Successful)}")
        )
        {
            RequestWriteRepository requestWriteRepository = new(context);

            CreateAddAccountRequestCommandHandler sut = new(
               context,
               requestWriteRepository,
               accountLegalEntityReadRepository.Object
            );

            result = await sut.Handle(command, CancellationToken.None);

            persistedRequest = await context.Requests.FirstOrDefaultAsync(a => a.Id == result.Result!.RequestId, CancellationToken.None);
        }

        result.Result.Should().NotBeNull();

        Assert.Multiple(() =>
        {
            Assert.That(persistedRequest, Is.Not.Null, "Request must be created.");
            Assert.That(result.Result!.RequestId, Is.EqualTo(persistedRequest!.Id), "Result RequestId must match persisted request id.");
            Assert.That(persistedRequest!.PermissionRequests, Has.Exactly(command.Operations.Count).Items, $"Request must have {command.Operations.Count} permission requests created.");
            Assert.That(persistedRequest.EmployerPAYE, Is.EqualTo(command.Paye));
        });
    }
}
