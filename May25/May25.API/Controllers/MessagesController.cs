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
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService.ThrowIfNull(nameof(messageService));
        }

        /// <summary>
        /// Returns the chat between the specified members of the specified trip.
        /// </summary>
        /// <param name="tripId">Trip Id.</param>
        /// <param name="user1">Driver Id.</param>
        /// <param name="user2">Passenger Id.</param>
        /// <returns></returns>
        [HttpGet("trip/{tripId}/{user1}/{user2}")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessages(int tripId, int user1, int user2)
        {
            var messages = await _messageService.GetMessagesAsync(tripId, user1, user2);

            return Ok(messages);
        }

        /// <summary>
        /// Sends a message from the authenticated user to the specified user in the specified trip.
        /// </summary>
        /// <param name="messageForCreation"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<MessageDTO>> CreateMessage([FromBody] MessageForCreationDTO messageForCreation)
        {
            var message = await _messageService.CreateAsync(messageForCreation);

            return Ok(message);
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
