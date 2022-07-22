using May25.API.Core.Contracts.Services;
using May25.API.Core.Extensions;
using May25.API.Core.Models.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/cars")]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService.ThrowIfNull(nameof(carService));
        }

        /// <summary>
        /// Get the list of cars of the specified user.
        /// </summary>
        /// <returns></returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<CarDTO>>> GetUserCars(int userId)
        {
            var cars = await _carService.GetUserCarsAsync(userId);

            return Ok(cars);
        }

        /// <summary>
        /// Get the car of the specified user.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{carId}", Name = "GetCar")]
        public async Task<ActionResult<CarDTO>> GetCar(int carId)
        {
            var car = await _carService.GetCarAsync(carId);

            return Ok(car);
        }

        /// <summary>
        /// Creates a new car.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CarDTO>> CreateCar([FromBody] CarForCreationDTO carForCreation)
        {
            var car = await _carService.CreateAsync(carForCreation);

            return CreatedAtRoute("GetCar", new { carId = car.Id }, car);
        }

        /// <summary>
        /// Updates a car.
        /// </summary>
        /// <param name="carId">Id of the car.</param>
        /// <param name="carForUpdate">User data.</param>
        /// <returns></returns>
        [HttpPut("{carId}")]
        public async Task<ActionResult<CarDTO>> UpdateCar(int carId, [FromBody] CarForUpdateDTO carForUpdate)
        {
            var car = await _carService.UpdateAsync(carId, carForUpdate);

            return Ok(car);
        }

        /// <summary>
        /// Deletes a car.
        /// </summary>
        /// <param name="carId">Id of the car.</param>
        /// <returns></returns>
        [HttpDelete("{carId}")]
        public async Task<ActionResult> DeleteCar(int carId)
        {
            await _carService.DeleteAsync(carId);

            return NoContent();
        }

        /// <summary>
        /// Returns the allowed HTTP verbs.
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetHttpOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,PUT,DELETE");
            return Ok();
        }
    }
}
