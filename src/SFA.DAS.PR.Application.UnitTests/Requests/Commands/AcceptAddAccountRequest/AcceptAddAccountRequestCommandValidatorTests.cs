using FluentValidation.TestHelper;
using Moq;
using SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;
using SFA.DAS.PR.Application.Requests.Commands.AcceptAddAccountRequest;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Requests.Commands.AcceptAddAccountRequest;

public sealed class AcceptAddAccountRequestCommandValidatorTests
{
    private readonly Mock<IRequestReadRepository> _requestReadRepositoryValidMock = new Mock<IRequestReadRepository>();
    private readonly Mock<IRequestReadRepository> _requestReadRepositoryInvalidMock = new Mock<IRequestReadRepository>();

    [SetUp]
    public void SetUp()
    {
        _requestReadRepositoryValidMock.Setup(a => a.RequestExists(
                It.IsAny<long>(),
                It.IsAny<long>(),
                It.IsAny<RequestStatus[]>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(true);

        _requestReadRepositoryInvalidMock.Setup(a => a.RequestExists(
                It.IsAny<long>(),
                It.IsAny<long>(),
                It.IsAny<RequestStatus[]>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(false);
    }

    [Test]
    [MoqAutoData]
    public async Task AcceptAddAccountRequestCommandValidator_AcceptAddAccountRequestCommand_Valid_Command(
        AcceptAddAccountRequestCommand command
    )
    {
        var sut = new AcceptAddAccountRequestCommandValidator(_requestReadRepositoryValidMock.Object);
        var result = await sut.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(query => query.RequestId);
    }

    [Test]
    [MoqAutoData]
    public async Task AcceptAddAccountRequestCommandValidator_AcceptAddAccountRequestCommand_Invalid_Command(
        AcceptAddAccountRequestCommand command
    )
    {
        var sut = new AcceptAddAccountRequestCommandValidator(_requestReadRepositoryInvalidMock.Object);
        var result = await sut.TestValidateAsync(command);
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    [MoqAutoData]
    public async Task AcceptAddAccountRequestCommandValidator_AcceptAddAccountRequestCommand_ActionedBy_Invalid(
        AcceptAddAccountRequestCommand command
    )
    {
        command.ActionedBy = string.Empty;
        var sut = new AcceptAddAccountRequestCommandValidator(_requestReadRepositoryInvalidMock.Object);
        var result = await sut.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(a => a.ActionedBy)
            .WithErrorMessage(AcceptAddAccountRequestCommandValidator.ActionedByValidationMessage);
    }
}
