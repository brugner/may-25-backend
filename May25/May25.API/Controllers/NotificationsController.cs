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
    [Authorize]
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationTokensService)
        {
            _notificationService = notificationTokensService.ThrowIfNull(nameof(notificationTokensService));
        }

        /// <summary>
        /// Returns all the notifications for the authenticated user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationDTO>>> GetAllNotifications()
        {
            var notifications = await _notificationService.GetAllAsync();

            return Ok(notifications);
        }

        /// <summary>
        /// Returns the unread notifications for the authenticated user.
        /// </summary>
        /// <returns></returns>
        [HttpGet("unread")]
        public async Task<ActionResult<IEnumerable<NotificationDTO>>> GetUnreadNotifications()
        {
            var notifications = await _notificationService.GetUnreadAsync();

            return Ok(notifications);
        }

        /// <summary>
        /// Marks the specified notification as read.
        /// </summary>
        /// <returns></returns>
        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _notificationService.MarkAsReadAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Register the notification token of the authenticated user.
        /// </summary>
        /// <param name="notificationToken"></param>
        /// <returns></returns>
        [HttpPost("token")]
        public async Task<IActionResult> RegisterToken(NotificationTokenForCreationDTO notificationToken)
        {
            await _notificationService.AddTokenAsync(notificationToken);

            return NoContent();
        }


        [HttpPost("send-trip-completed")]
        [Authorize(Roles = AppRoles.ApiClient)]
        public async Task<ActionResult<SendTripCompletedNotificationsResultDTO>> SendTripCompleted()
        {
            var result = await _notificationService.SendTripCompletedNotificationsAsync();

            return Ok(result);
        }

        /// <summary>
        /// Returns the allowed HTTP verbs.
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetHttpOptions()
        {
            Response.Headers.Add("Allow", "GET,POST");
            return Ok();
        }
    }
}
