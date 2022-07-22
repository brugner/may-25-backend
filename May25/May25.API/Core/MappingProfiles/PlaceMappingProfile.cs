using AutoMapper;
using GoogleApi.Entities.Maps.Geocoding.Common;
using GoogleApi.Entities.Places.Common;
using GoogleApi.Entities.Places.Details.Response;
using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Resources;

namespace May25.API.Core.MappingProfiles
{
    public class PlaceMappingProfile : Profile
    {
        public PlaceMappingProfile()
        {
            CreateMap<Place, PlaceDTO>();

            CreateMap<PlaceForCreationDTO, Place>();

            CreateMap<Prediction, PlaceAutocompleteDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PlaceId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<Result, PlaceAutocompleteDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PlaceId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.FormattedAddress));

            CreateMap<DetailsResult, PlaceDetailDTO>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PlaceId))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.FormattedAddress))
               .ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.Geometry.Location.Latitude))
               .ForMember(dest => dest.Lng, opt => opt.MapFrom(src => src.Geometry.Location.Longitude));
        }
    }
}
