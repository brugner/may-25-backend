using May25.API.Core.Contracts.Services;
using May25.API.Core.Extensions;
using May25.API.Core.Models.Resources;
using May25.API.Core.Options;
using Microsoft.Extensions.Options;

namespace May25.API.Core.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly AppOptions _appOptions;

        public SettingsService(IOptions<AppOptions> appOptions)
        {
            _appOptions = appOptions.Value.ThrowIfNull(nameof(appOptions));
        }

        public SettingsDTO GetAll()
        {
            return new SettingsDTO
            {
                FuelPrice = _appOptions.FuelPrice
            };
        }
    }
}
