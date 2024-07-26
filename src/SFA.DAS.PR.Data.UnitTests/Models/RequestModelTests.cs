using NUnit.Framework;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Models;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Data.UnitTests.Models;

public class RequestModelTests
{
    [Test]
    [RecursiveMoqAutoData]
    public void RequestModel_ImplicitConversion_Successful()
    {
        Request request = RequestTestData.Create(Guid.NewGuid());

        RequestModel requestModel = request;

        Assert.Multiple(() =>
        {
            Assert.That(requestModel.RequestId, Is.EqualTo(request.Id));
            Assert.That(requestModel.RequestType, Is.EqualTo(request.RequestType.ToString()));
            Assert.That(requestModel.Ukprn, Is.EqualTo(request.Ukprn));
            Assert.That(requestModel.ProviderName, Is.EqualTo(request.Provider.Name));
            Assert.That(requestModel.ProviderName, Is.EqualTo(request.Provider.Name));
            Assert.That(requestModel.RequestedBy, Is.EqualTo(request.RequestedBy));
            Assert.That(requestModel.RequestedDate, Is.EqualTo(request.RequestedDate));
            Assert.That(requestModel.AccountLegalEntityId, Is.EqualTo(request.AccountLegalEntityId));
            Assert.That(requestModel.EmployerOrganisationName, Is.EqualTo(request.EmployerOrganisationName));
            Assert.That(requestModel.EmployerContactFirstName, Is.EqualTo(request.EmployerContactFirstName));
            Assert.That(requestModel.EmployerContactLastName, Is.EqualTo(request.EmployerContactLastName));
            Assert.That(requestModel.EmployerContactEmail, Is.EqualTo(request.EmployerContactEmail));
            Assert.That(requestModel.EmployerPAYE, Is.EqualTo(request.EmployerPAYE));
            Assert.That(requestModel.EmployerAORN, Is.EqualTo(request.EmployerAORN));
            Assert.That(requestModel.UpdatedDate, Is.EqualTo(request.UpdatedDate));
            Assert.That(requestModel.Operations, Is.EqualTo(request.PermissionRequests.Select(a => (Operation)a.Operation).ToArray()));
            Assert.That(requestModel.Status, Is.EqualTo(request.Status.ToString()));
        });
    }
}
