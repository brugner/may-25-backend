using May25.API.Core.Models.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageDTO>> GetMessagesAsync(int tripId, int user1, int user2);
        Task<MessageDTO> CreateAsync(MessageForCreationDTO messageForCreation);
    }
}
