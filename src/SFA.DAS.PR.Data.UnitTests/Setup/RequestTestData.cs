using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Setup;

public static class RequestTestData
{
    public static Request CreateRequest(Guid id)
    {
        return new()
        {
            Id = id,
            RequestType = "RequestType",
            Ukprn = 10000003,
            ProviderUserFullName = "ProviderUserFullName",
            RequestedBy = "RequestedBy",
            RequestedDate = DateTime.Today,
            AccountLegalEntityId = 3,
            EmployerUserRef = Guid.NewGuid(),
            EmployerOrganisationName = "EmployerOrganisationName",
            EmployerContactFirstName = "EmployerContactFirstName",
            EmployerContactLastName = "EmployerContactLastName",
            EmployerContactEmail = "EmployerContactEmail",
            EmployerPAYE = "PAYE",
            EmployerAORN = "AORN",
            Status = "New",
            UpdatedDate = DateTime.Today
        };
    }
}
