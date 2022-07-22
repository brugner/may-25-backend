using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Resources;
using May25.API.Core.Services;
using May25.Tests.Mocks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace May25.Tests.Services
{
    public class MakeServiceTests
    {
        [Fact]
        public async Task MakeService_GetAllAsync_MakesExists()
        {
            // Arrange
            var expectedMakes = new List<Make>
            {
                new Make { Id = 1, Name = "Toyota" },
                new Make { Id = 2, Name = "Volkswagen" },
            }.AsEnumerable();

            var expectedMakesDTO = new List<MakeDTO>
            {
                new MakeDTO { Id = 1, Name = "Toyota" },
                new MakeDTO { Id = 2, Name = "Volkswagen" },
            }.AsEnumerable();

            var mockMakeRepository = new MockMakeRepository().MockGetAllAsync(expectedMakes);
            var mockUnitOfWork = new MockUnitOfWork().MockRepository(x => x.Makes, mockMakeRepository.Object);
            var mockMapper = new MockMapper().MockMap<IEnumerable<Make>, IEnumerable<MakeDTO>>(expectedMakesDTO);
            var makeService = new MakeService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await makeService.GetAllAsync();

            // Assert
            Assert.Equal(expectedMakesDTO, result);
        }
    }
}
