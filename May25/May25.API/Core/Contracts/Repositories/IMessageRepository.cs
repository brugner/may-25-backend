using May25.API.Core.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Repositories
{
    public interface IMessageRepository : IRepository<Message, int>
    {
        Task<IEnumerable<Message>> GetAllAsync(int tripId, int driverId, int passengerId);
    }
}
