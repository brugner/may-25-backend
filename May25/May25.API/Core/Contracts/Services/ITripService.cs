using May25.API.Core.Models.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.Services
{
    public interface ITripService
    {
        Task<TripDTO> CreateAsync(TripForCreationDTO tripForCreation);
        Task<TripDTO> GetTripAsync(int tripId);
        Task<MyTripsDTO> GetMyTripsAsync();
        Task<IEnumerable<TripSummaryDTO>> SearchAsync(TripsSearchParamsDTO searchParams);
        Task CancelSeatAsync(int tripId);
        Task CancelTripAsync(int tripId);
        Task<IEnumerable<TripDTO>> GetAvailableTripsAsync();
    }
}
