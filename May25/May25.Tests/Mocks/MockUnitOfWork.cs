using May25.API.Core.Contracts.UnitsOfWork;
using Moq;
using System;
using System.Linq.Expressions;

namespace May25.Tests.Mocks
{
    class MockUnitOfWork : Mock<IUnitOfWork>
    {
        public MockUnitOfWork MockRepository<TRepository>(Expression<Func<IUnitOfWork, TRepository>> repository, TRepository result)
        {
            Setup(repository).Returns(result);

            return this;
        }
    }
}
