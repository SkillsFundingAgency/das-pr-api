using NUnit.Framework;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Models;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing.AutoFixture;
using FluentAssertions;

namespace SFA.DAS.PR.Data.UnitTests.Models;

public class RequestModelTests
{
    [Test]
    [RecursiveMoqAutoData]
    public void RequestModel_ImplicitConversion_Successful()
    {
        Request request = RequestTestData.Create(Guid.NewGuid());

        RequestModel requestModel = request;

        requestModel.Should().BeEquivalentTo(
            request,
            options => options
                .Excluding(model => model.Id)
                .Excluding(model => model.RequestType)
                .Excluding(model => model.Provider)
                .Excluding(model => model.PermissionRequests)
                .Excluding(model => model.Status)
                .Excluding(model => model.ActionedBy)
                .Excluding(model => model.AccountLegalEntity)
        );

        Assert.Multiple(() =>
        {
            Assert.That(requestModel.RequestType, Is.EqualTo(request.RequestType.ToString()));
            Assert.That(requestModel.ProviderName, Is.EqualTo(request.Provider.Name));
            Assert.That(requestModel.RequestId, Is.EqualTo(request.Id));
            Assert.That(requestModel.Operations, Is.EqualTo(request.PermissionRequests.Select(a => (Operation)a.Operation).ToArray()));
            Assert.That(requestModel.Status, Is.EqualTo(request.Status.ToString()));
        });
    }
}
