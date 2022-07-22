using May25.API.Core.Contracts.Services;
using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Resources;
using May25.API.Core.Services;
using May25.Tests.Mocks;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace May25.Tests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task UserService_GetByIdAsync_UserExists()
        {
            // Arrange
            var userId = 1;

            var expectedUser = new User
            {
                Id = userId,
                Email = "admin1@may25.com",
                Roles = new List<UserRoles> { new UserRoles { UserId = userId, RoleId = 1 } }
            };

            var expectedUserDTO = new UserDTO
            {
                Id = userId,
                Email = "admin1@may25.com",
                Roles = new string[] { "admin" }.ToList()
            };

            var mockUserRepository = new MockUserRepository().MockGetAsync(expectedUser);
            var mockUnitOfWork = new MockUnitOfWork().MockRepository(x => x.Users, mockUserRepository.Object);
            var mockMapper = new MockMapper().MockMap<User, UserDTO>(expectedUserDTO);
            var mockHashService = new Mock<IHashService>();
            var mockClaimsPrincipal = new MockClaimsPrincipal().MockHasClaim(true);
            var mockRatingService = new MockRatingService();
            var mockEmailService = new MockEmailService();
            var mockHtmlService = new MockHtmlService();

            var userService = new UserService(mockUnitOfWork.Object, mockMapper.Object,
                mockHashService.Object, mockClaimsPrincipal.Object, mockRatingService.Object,
                mockEmailService.Object, mockHtmlService.Object);

            // Act
            var result = await userService.GetByIdAsync(userId);

            // Assert
            Assert.Equal(expectedUserDTO, result);
        }
    }
}
