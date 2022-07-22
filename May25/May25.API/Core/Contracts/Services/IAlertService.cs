using May25.API.Core.Models.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Services
{
    public interface IAlertService
    {
        Task<IEnumerable<AlertDTO>> GetMyAlerts();
        Task<AlertDTO> GetAlertAsync(int id);
        Task<AlertDTO> CreateAsync(AlertForCreationDTO alertForCreation);
        Task DeleteAlertAsync(int id);
    }
}
