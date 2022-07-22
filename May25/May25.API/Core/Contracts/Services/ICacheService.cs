using May25.API.Core.Models.Resources;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Services
{
    public interface ICacheService
    {
        Task<CleanGoogleApiCacheResultDTO> CleanGoogleMapsApiCacheAsync();
    }
}
