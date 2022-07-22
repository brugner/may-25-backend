using May25.API.Core.Options;
using Microsoft.Extensions.Options;
using Moq;

namespace May25.Tests.Mocks
{
    class MockOptions<T> : Mock<IOptions<T>> where T : class, new()
    {
        public MockOptions<T> MockTheseOptions(T result)
        {
            Setup(x => x.Value).Returns(result);
            return this;
        }
    }
}
