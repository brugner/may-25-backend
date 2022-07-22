using May25.API.Core.Contracts.Services;
using May25.API.Core.Extensions;
using May25.API.Core.Models.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace May25.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService.ThrowIfNull(nameof(authService));
        }

        /// <summary>
        /// Authenticates a user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("user")]
        public async Task<ActionResult<UserAuthenticationResultDTO>> AuthenticateUser([FromBody] UserForAuthenticationDTO user)
        {
            var result = await _authService.AuthenticateAsync(user);

            return Ok(result);
        }

        /// <summary>
        /// Authenticates a user through Google.
        /// </summary>
        /// <returns></returns>
        [HttpPost("user/google")]
        public async Task<ActionResult<UserAuthenticationResultDTO>> AuthenticateUserGoogle(string token)
        {
            var result = await _authService.AuthenticateGoogleAsync(token);

            return Ok(result);
        }

        /// <summary>
        /// Authenticates an API client.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        [HttpPost("client")]
        public async Task<ActionResult<ClientAuthenticationResultDTO>> AuthenticateClient([FromBody] ClientForAuthenticationDTO client)
        {
            var result = await _authService.AuthenticateAsync(client);

            return Ok(result);
        }

        /// <summary>
        /// Returns the allowed HTTP verbs.
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetHttpOptions()
        {
            Response.Headers.Add("Allow", "POST");
            return Ok();
        }
    }
}
