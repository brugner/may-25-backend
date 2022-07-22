using AutoMapper;
using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Resources;
using System.Linq;

namespace May25.API.Core.MappingProfiles
{
    public class TripMappingProfile : Profile
    {
        public TripMappingProfile()
        {
            CreateMap<Trip, TripDTO>()
                .ForMember(dest => dest.Passengers, opt => opt.MapFrom(src => src.Passengers.Select(x => x.Passenger)));

            CreateMap<TripForCreationDTO, Trip>();

            CreateMap<Trip, TripSummaryDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(x => x.Origin.FormattedAddress))
                .ForMember(dest => dest.Destination, opt => opt.MapFrom(x => x.Destination.FormattedAddress))
                .ForMember(dest => dest.Departure, opt => opt.MapFrom(x => x.Departure));
        }
    }
}
