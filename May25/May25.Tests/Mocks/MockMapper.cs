using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace May25.Tests.Mocks
{
    class MockMapper : Mock<IMapper>
    {
        public MockMapper MockMap<TSource, TDestination>(TDestination result)
        {
            Setup(x => x.Map<TDestination>(It.IsAny<TSource>())).Returns(result);

            return this;
        }
    }
}
