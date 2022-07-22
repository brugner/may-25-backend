using May25.API.Core.Contracts.Repositories;
using May25.API.Core.Models.Entities;
using Moq;
using System.Threading.Tasks;

namespace May25.Tests.Mocks
{
    public class MockUserRepository : Mock<IUserRepository>
    {
        public MockUserRepository MockGetAsync(User result)
        {
            Setup(x => x.GetAsync(It.IsAny<int>())).Returns(new ValueTask<User>(result));

            return this;
        }
    }
}
