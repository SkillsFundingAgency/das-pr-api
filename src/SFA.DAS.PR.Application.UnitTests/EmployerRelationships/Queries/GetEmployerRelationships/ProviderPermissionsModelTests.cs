using FluentAssertions;
using SFA.DAS.PR.Application.EmployerRelationships.Queries.GetProviderRelationships;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing.AutoFixture;
namespace SFA.DAS.PR.Application.UnitTests.EmployerRelationships.Queries.GetEmployerRelationships;
public class ProviderPermissionsModelTests
{
    [Test, MoqAutoData]
    public void Operator_mapsAccountProviderLegalEntity(string providerName, long ukprn, List<Operation> operations)
    {
        var permissions = operations.Select(operation => new Permission { Operation = operation }).ToList();

        AccountProviderLegalEntity source = new()
        {
            AccountProvider = new AccountProvider { Provider = new Provider { Name = providerName }, ProviderUkprn = ukprn },
            Permissions = permissions
        };

        ProviderPermissionsModel model = source;
        model.ProviderName.Should().Be(providerName);
        model.Ukprn.Should().Be(ukprn);
        model.Operations.Should().BeEquivalentTo(operations.ToArray());
    }
}
