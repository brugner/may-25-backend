using Moq;
using System.Security.Claims;

namespace May25.Tests.Mocks
{
    class MockClaimsPrincipal : Mock<ClaimsPrincipal>
    {
        public MockClaimsPrincipal MockHasClaim(bool result)
        {
            Setup(m => m.HasClaim(It.IsAny<string>(), It.IsAny<string>())).Returns(result);

            return this;
        }
    }
}
