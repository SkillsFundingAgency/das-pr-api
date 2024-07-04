using AutoFixture;

namespace SFA.DAS.PR.Data.UnitTests.Helpers;

public static class TestHelpers
{
    public static Fixture CreateFixture()
    {
        Fixture fixture = new();
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
            .ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        return fixture;
    }
}
