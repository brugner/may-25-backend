using May25.API.Core.Constants;
using May25.API.Core.Contracts.Services;
using May25.API.Core.Extensions;
using May25.API.Core.Models.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Controllers
{
    [ApiController]
    [Authorize(Roles = AppRoles.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICarService _carService;
        private readonly INotificationService _notificationTokenService;

        public AdminController(IUserService userService, ICarService carService, INotificationService notificationService)
        {
            _userService = userService.ThrowIfNull(nameof(userService));
            _carService = carService.ThrowIfNull(nameof(carService));
            _notificationTokenService = notificationService.ThrowIfNull(nameof(notificationService));
        }

        /// <summary>
        /// Get the list of users.
        /// </summary>
        /// <returns></returns>
        [Route("api/users")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();

            return Ok(users);
        }

        /// <summary>
        /// Get the list of cars.
        /// </summary>
        /// <returns></returns>
        [Route("api/cars")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarDTO>>> GetAllCars()
        {
            var cars = await _carService.GetAllAsync();

            return Ok(cars);
        }

        [Route("api/notifications/tokens")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationTokenDTO>>> GetAllNotificationTokens()
        {
            var tokens = await _notificationTokenService.GetAllTokensAsync();

            return Ok(tokens);
        }

        /// <summary>
        /// Returns the allowed HTTP verbs.
        /// </summary>
        /// <returns></returns>
        [Route("api/admin")]
        [HttpOptions]
        public IActionResult GetHttpOptions()
        {
            Response.Headers.Add("Allow", "GET");
            return Ok();
        }
    }
}
// TODO: add pagination to the get alls