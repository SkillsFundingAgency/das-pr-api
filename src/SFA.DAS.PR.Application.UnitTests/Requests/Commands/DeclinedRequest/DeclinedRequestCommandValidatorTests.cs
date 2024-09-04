using FluentValidation.TestHelper;
using Moq;
using SFA.DAS.PR.Application.Requests.Commands.DeclinedRequest;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Requests.Commands.DeclinedRequest;

public sealed class DeclinedRequestCommandValidatorTests
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
    public async Task DeclinedRequestCommandValidator_DeclinedRequestCommand_Valid_Command(
        DeclinedRequestCommand command
    )
    {
        var sut = new DeclinedRequestCommandValidator(_requestReadRepositoryValidMock.Object);
        var result = await sut.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(query => query.RequestId);
    }

    [Test]
    [MoqAutoData]
    public async Task DeclinedRequestCommandValidator_DeclinedRequestCommand_Invalid_Command(
        DeclinedRequestCommand command
    )
    {
        var sut = new DeclinedRequestCommandValidator(_requestReadRepositoryInvalidMock.Object);
        var result = await sut.TestValidateAsync(command);
        result.ShouldHaveAnyValidationError();
    }
}
