using AutoMapper;
using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Resources;

namespace May25.API.Core.MappingProfiles
{
    public class SeatRequestMappingProfile : Profile
    {
        public SeatRequestMappingProfile()
        {
            CreateMap<SeatRequest, TripSeatRequestDTO>();
            CreateMap<SeatRequest, UserSeatRequestDTO>();
        }
    }
}
