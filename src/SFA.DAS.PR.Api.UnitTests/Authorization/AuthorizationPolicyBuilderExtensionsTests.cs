using Microsoft.AspNetCore.Authorization;
using SFA.DAS.PR.Api.Authorization;

namespace SFA.DAS.PR.Api.UnitTests.Authorization
{
    public class AuthorizationPolicyBuilderExtensionsTests
    {
        [Test]
        public void AllowAnonymousUser_Adds_NoneRequirement()
        {
            AuthorizationPolicyBuilder builder = new();
            var result = builder.AllowAnonymousUser();
            Assert.That(result, Is.Not.Null);
            Assert.That(builder.Requirements, Has.One.InstanceOf<NoneRequirement>());
        }
    }
}
