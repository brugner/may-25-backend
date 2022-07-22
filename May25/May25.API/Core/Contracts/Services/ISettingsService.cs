using May25.API.Core.Models.Resources;

namespace May25.API.Core.Contracts.Services
{
    public interface ISettingsService
    {
        SettingsDTO GetAll();
    }
}
