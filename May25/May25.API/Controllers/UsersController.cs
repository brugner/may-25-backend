using May25.API.Core.Constants;
using May25.API.Core.Contracts.Services;
using May25.API.Core.Exceptions;
using May25.API.Core.Extensions;
using May25.API.Core.Models.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace May25.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ClaimsPrincipal _authUser;

        public UsersController(IUserService userService, ClaimsPrincipal authUser)
        {
            _userService = userService.ThrowIfNull(nameof(userService));
            _authUser = authUser;
        }

        /// <summary>
        /// Get a user by id.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            if (_authUser.IsInRole(AppRoles.Admin) || _authUser.HasId(id))
            {
                var user = await _userService.GetByIdAsync(id);

                if (user == null)
                {
                    throw new NotFoundException($"User {id} not found");
                }

                return Ok(user);
            }
            else
            {
                throw new ForbiddenException("Cannot access another user info");
            }
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userForCreation">User data.</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<CreatedUserDTO>> CreateUser([FromBody] UserForCreationDTO userForCreation)
        {
            var user = await _userService.CreateAsync(userForCreation);

            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }

        /// <summary>
        /// Gets a user public info.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <returns></returns>
        [HttpGet("{id}/public")]
        public async Task<ActionResult<UserPublicProfileDTO>> GetPublicProfile(int id)
        {
            var user = await _userService.GetUserPublicProfileAsync(id);

            return Ok(user);
        }

        /// <summary>
        /// Gets a user public info.
        /// </summary>
        /// <returns></returns>
        [HttpGet("me/public")]
        public async Task<ActionResult<UserPublicProfileDTO>> GetPublicProfile()
        {
            int id = _authUser.GetId();
            var user = await _userService.GetUserPublicProfileAsync(id);

            return Ok(user);
        }

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <param name="userForUpdate">User data.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDTO>> Update(int id, [FromBody] UserForUpdateDTO userForUpdate)
        {
            var user = await _userService.UpdateAsync(id, userForUpdate);

            return Ok(user);
        }

        /// <summary>
        /// Confirms the email of the specified user.
        /// </summary>
        /// <param name="email">Email of the user.</param>
        /// <param name="token">Email confirmation token.</param>
        /// <returns></returns>
        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var result = await _userService.ConfirmEmailAsync(new ConfirmEmailParamsDTO(email, token));

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = result.Success ? 200 : 400,
                Content = result.Content
            };
        }

        /// <summary>
        /// Unsubscribe a user from the email list.
        /// </summary>
        /// <param name="email">Email of the user.</param>
        /// <returns></returns>
        [HttpPost("unsubscribe")]
        [AllowAnonymous]
        public async Task<IActionResult> Unsubscribe(string email)
        {
            var result = await _userService.UnsubscribeAsync(email);

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = 200,
                Content = result.Content
            };
        }

        /// <summary>
        /// Change the password of the authenticated user.
        /// </summary>
        /// <param name="changePassword">Password info.</param>
        /// <returns></returns>
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePassword)
        {
            await _userService.ChangePasswordAsync(changePassword);

            return NoContent();
        }

        /// <summary>
        /// Creates a new password reset request.
        /// </summary>
        /// <returns></returns>
        [HttpPost("request-password-reset")]
        [AllowAnonymous]
        public async Task<IActionResult> RequestPasswordReset(string email)
        {
            await _userService.RequestPasswordResetAsync(email);

            return NoContent();
        }

        [HttpGet("password-reset")]
        [AllowAnonymous]
        public IActionResult GetPasswordResetForm(string email, string token)
        {
            var result = _userService.GetPasswordResetForm(email, token);

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = 200,
                Content = result.Content
            };
        }

        [HttpPost("password-reset")]
        [AllowAnonymous]
        public async Task<ActionResult<HtmlContentResultDTO>> ResetPassword(string email, string token, [FromBody] ResetPasswordDTO data)
        {
            await _userService.ResetPasswordAsync(email, token, data);

            return NoContent();
        }

        /// <summary>
        /// Returns the allowed HTTP verbs.
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetHttpOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,PUT");
            return Ok();
        }
    }
}
