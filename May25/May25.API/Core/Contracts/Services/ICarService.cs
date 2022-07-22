using May25.API.Core.Models.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Services
{
    public interface ICarService
    {
        Task<IEnumerable<CarDTO>> GetAllAsync();
        Task<IEnumerable<CarDTO>> GetUserCarsAsync(int userId);
        Task<CarDTO> CreateAsync(CarForCreationDTO carForCreation);
        Task<CarDTO> GetCarAsync(int carId);
        Task<CarDTO> UpdateAsync(int carId, CarForUpdateDTO carForUpdate);
        Task DeleteAsync(int carId);
    }
}
