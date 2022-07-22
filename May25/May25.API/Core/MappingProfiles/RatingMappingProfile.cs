using AutoMapper;
using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Resources;

namespace May25.API.Core.MappingProfiles
{
    public class RatingMappingProfile : Profile
    {
        public RatingMappingProfile()
        {
            CreateMap<RatingForCreationDTO, Rating>();
            CreateMap<Rating, RatingDTO>();
        }
    }
}
