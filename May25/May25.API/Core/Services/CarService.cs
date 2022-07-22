using AutoMapper;
using May25.API.Core.Contracts.Services;
using May25.API.Core.Contracts.UnitsOfWork;
using May25.API.Core.Exceptions;
using May25.API.Core.Extensions;
using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Resources;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace May25.API.Core.Services
{
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _authUser;

        public CarService(IUnitOfWork unitOfWork, IMapper mapper, ClaimsPrincipal authUser)
        {
            _unitOfWork = unitOfWork.ThrowIfNull(nameof(unitOfWork));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
            _authUser = authUser.ThrowIfNull(nameof(authUser));
        }

        public async Task<CarDTO> CreateAsync(CarForCreationDTO carForCreation)
        {
            var car = _mapper.Map<Car>(carForCreation);
            car.DriverId = _authUser.GetId();
            car.PlateNumber = car.PlateNumber.ToUpper();

            _unitOfWork.Cars.Add(car);
            await _unitOfWork.CompleteAsync();

            return await GetCarAsync(car.Id);
        }

        public async Task DeleteAsync(int carId)
        {
            var car = await _unitOfWork.Cars.GetAsync(carId);

            if (car == null)
            {
                throw new NotFoundException($"Car {carId} not found");
            }

            car.IsDeleted = true;

            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<CarDTO>> GetAllAsync()
        {
            var cars = await _unitOfWork.Cars.GetAllAsync();

            return _mapper.Map<IEnumerable<CarDTO>>(cars);
        }

        public async Task<CarDTO> GetCarAsync(int carId)
        {
            var car = await _unitOfWork.Cars.GetAsync(carId);

            if (car == null)
            {
                throw new NotFoundException($"Car {carId} not found");
            }

            return _mapper.Map<CarDTO>(car);
        }

        public async Task<IEnumerable<CarDTO>> GetUserCarsAsync(int userId)
        {
            var cars = await _unitOfWork.Cars.GetUserCarsAsync(userId);

            return _mapper.Map<IEnumerable<CarDTO>>(cars);
        }

        public async Task<CarDTO> UpdateAsync(int id, CarForUpdateDTO carForUpdate)
        {
            var car = await _unitOfWork.Cars.GetAsync(id);

            if (car == null)
            {
                throw new NotFoundException($"Car {id} not found");
            }

            _mapper.Map(carForUpdate, car);
            car.PlateNumber = car.PlateNumber.ToUpper();

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CarDTO>(car);
        }
    }
}
