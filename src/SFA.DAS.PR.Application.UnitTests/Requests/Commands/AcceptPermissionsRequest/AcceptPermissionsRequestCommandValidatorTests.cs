using FluentValidation.TestHelper;
using Moq;
using SFA.DAS.PR.Application.Requests.Commands.AcceptPermissionsRequest;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Requests.Commands.AcceptPermissionsRequest;

public sealed class AcceptPermissionsRequestCommandValidatorTests
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
    public async Task AcceptPermissionsRequestCommandValidator_AcceptPermissionsRequestCommand_Valid_Command(
        AcceptPermissionsRequestCommand command
    )
    {
        var sut = new AcceptPermissionsRequestCommandValidator(_requestReadRepositoryValidMock.Object);
        var result = await sut.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(query => query.RequestId);
    }

    [Test]
    [MoqAutoData]
    public async Task AcceptPermissionsRequestCommanddValidator_AcceptPermissionsRequestCommand_Invalid_Command(
        AcceptPermissionsRequestCommand command
    )
    {
        var sut = new AcceptPermissionsRequestCommandValidator(_requestReadRepositoryInvalidMock.Object);
        var result = await sut.TestValidateAsync(command);
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    [MoqAutoData]
    public async Task AcceptPermissionsRequestCommandValidator_AcceptPermissionsRequestCommand_ActionedBy_Invalid(
        AcceptPermissionsRequestCommand command
    )
    {
        command.ActionedBy = string.Empty;
        var sut = new AcceptPermissionsRequestCommandValidator(_requestReadRepositoryInvalidMock.Object);
        var result = await sut.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(a => a.ActionedBy)
            .WithErrorMessage(AcceptPermissionsRequestCommandValidator.ActionedByValidationMessage);
    }
}
