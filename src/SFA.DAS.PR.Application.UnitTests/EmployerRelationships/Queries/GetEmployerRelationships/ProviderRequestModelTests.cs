using SFA.DAS.PR.Application.EmployerRelationships.Queries.GetProviderRelationships;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Data.UnitTests.Models;

public sealed class ProviderRequestModelTests
{
    [Test]
    [RecursiveMoqAutoData]
    public void ProviderRequestModel_ImplicitConversion_Successful()
    {
        Request request = RequestTestData.Create(Guid.NewGuid());

        ProviderRequestModel providerRequestModel = request;

        Assert.Multiple(() =>
        {
            Assert.That(providerRequestModel.RequestId, Is.EqualTo(request.Id));
            Assert.That(providerRequestModel.Ukprn, Is.EqualTo(request.Ukprn));
            Assert.That(providerRequestModel.Operations.Count(), Is.EqualTo(request.PermissionRequests.Count));
            Assert.That(providerRequestModel.RequestType, Is.EqualTo(request.RequestType));
        });
    }
}
