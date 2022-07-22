using May25.API.Core.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Repositories
{
    public interface IAlertRepository : IRepository<Alert, int>
    {
        Task<IEnumerable<Alert>> GetAllAsync(int userId);
        Task<Alert> GetAsync(int id, int userId);
        Task<IEnumerable<Alert>> GetAlertsReadyForGoogleCacheCleanUp();
        Task<IEnumerable<Alert>> GetAlertsAsync(Place origin, Place destination);
    }
}
