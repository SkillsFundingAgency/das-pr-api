using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.PR.Application.ProviderRelationships.Queries.GetProviderRelationships;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Application.UnitTests.ProviderRelationships.Queries.GetProviderRelationships;

public class ProviderRelationshipModelTests
{
    [Test, AutoData]
    public void Operator_ConvertsFromProviderRelationshipEntity(ProviderRelationship source)
    {
        ProviderRelationshipModel sut = source;

        sut.Should().BeEquivalentTo(source, config => config.ExcludingMissingMembers());
    }
}
