﻿using AutoFixture;
using FluentValidation.TestHelper;
using Moq;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Application.EmployerRelationships.Queries.GetProviderEmployerRelationship;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.EmployerRelationships.Queries.GetProviderEmployerRelationship;

public class GetProviderEmployerRelationshipQueryValidatorTests
{
    private readonly CancellationToken cancellationToken = CancellationToken.None;
    private readonly Mock<IAccountLegalEntityReadRepository> _accountLegalEntityReadRepositoryMock = new Mock<IAccountLegalEntityReadRepository>();
    private readonly Mock<IProviderReadRepository> _providerReadRepositoryMock = new Mock<IProviderReadRepository>();

    [Test]
    public async Task Validate_Ukprn_Valid_Command()
    {
        Mock<IProviderReadRepository> providerReadRepositoryMock = new Mock<IProviderReadRepository>();

        providerReadRepositoryMock.Setup(a => a.ProviderExists(It.IsAny<long>(), cancellationToken)).ReturnsAsync(true);

        var sut = new GetProviderEmployerRelationshipQueryValidator(providerReadRepositoryMock.Object, _accountLegalEntityReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(new GetProviderEmployerRelationshipQuery(10000003, 1));
        result.ShouldNotHaveValidationErrorFor(query => query.Ukprn);
    }

    [Test]
    public async Task Validate_Ukprn_Returns_ErrorMessage()
    {
        var sut = new GetProviderEmployerRelationshipQueryValidator(_providerReadRepositoryMock.Object, _accountLegalEntityReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(new GetProviderEmployerRelationshipQuery(null, 1));
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
                    .WithErrorMessage(GetProviderEmployerRelationshipQueryValidator.UkprnValidationMessage);
    }

    [Test]
    public async Task Validate_Ukprn_Returns_FormatErrorMessage()
    {
        var sut = new GetProviderEmployerRelationshipQueryValidator(_providerReadRepositoryMock.Object, _accountLegalEntityReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(new GetProviderEmployerRelationshipQuery(10003, 1));
        result.ShouldHaveValidationErrorFor(q => q.Ukprn)
                    .WithErrorMessage(UkprnFormatValidator.UkprnFormatValidationMessage);
    }

    [Test]
    public async Task Validate_AccountLegalEntityId_Valid_Command()
    {
        Mock<IAccountLegalEntityReadRepository> accountLegalEntityReadRepositoryMock = new Mock<IAccountLegalEntityReadRepository>();

        accountLegalEntityReadRepositoryMock.Setup(a => a.AccountLegalEntityExists(It.IsAny<long>(), cancellationToken)).ReturnsAsync(true);

        var sut = new GetProviderEmployerRelationshipQueryValidator(_providerReadRepositoryMock.Object, accountLegalEntityReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(new GetProviderEmployerRelationshipQuery(10000003, 1));
        result.ShouldNotHaveValidationErrorFor(query => query.AccountLegalEntityId);
    }

    [Test]
    public async Task Validate_AccountLegalEntityId_Returns_ErrorMessage()
    {
        var sut = new GetProviderEmployerRelationshipQueryValidator(_providerReadRepositoryMock.Object, _accountLegalEntityReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(new GetProviderEmployerRelationshipQuery(10000003, null));
        result.ShouldHaveValidationErrorFor(q => q.AccountLegalEntityId)
                    .WithErrorMessage(GetProviderEmployerRelationshipQueryValidator.AccountLegalEntityIdValidationMessage);
    }
}