using May25.API.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Repositories
{
    public interface ICarRepository : IRepository<Car, int>
    {
        Task<IEnumerable<Car>> GetUserCarsAsync(int userId);
    }
}
