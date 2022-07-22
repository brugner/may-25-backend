using May25.API.Core.Models.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Services
{
    public interface IMakeService
    {
        Task<IEnumerable<MakeDTO>> GetAllAsync();
    }
}
