using May25.API.Core.Contracts.Repositories;
using May25.API.Core.Models.Entities;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.Tests.Mocks
{
    class MockMakeRepository : Mock<IMakeRepository>
    {
        public MockMakeRepository MockGetAllAsync(IEnumerable<Make> result)
        {
            Setup(x => x.GetAllAsync()).Returns(Task.FromResult(result));

            return this;
        }
    }
}
