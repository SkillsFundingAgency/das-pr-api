using Microsoft.AspNetCore.Authorization;
using SFA.DAS.PR.Api.Authorization;
using System.Reflection;
using System.Security.Claims;

namespace SFA.DAS.PR.Api.UnitTests.Authorization
{
    public class LocalAuthorizationHandlerTests
    {
        [Test]
        public void HandleRequirementAsync_Succeeds()
        {
            LocalAuthorizationHandler handler = new();
            MethodInfo? methodInfo = typeof(LocalAuthorizationHandler).GetMethod("HandleRequirementAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            List<IAuthorizationRequirement> requirements = new();
            var context = new AuthorizationHandlerContext(requirements, new ClaimsPrincipal(), null);
            var result = methodInfo?.Invoke(handler, new object[] { context, new NoneRequirement() });
            Assert.That(context.HasSucceeded, Is.True);
            Assert.That(result, Is.EqualTo(Task.CompletedTask));
        }
    }
}
