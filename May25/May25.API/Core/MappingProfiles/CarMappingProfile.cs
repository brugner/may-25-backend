using AutoMapper;
using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Resources;

namespace May25.API.Core.MappingProfiles
{
    public class CarMappingProfile : Profile
    {
        public CarMappingProfile()
        {
            CreateMap<Car, CarDTO>()
                .ForMember(dest => dest.Make, opt => opt.MapFrom(src => src.Make.Name))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model.Name));

            CreateMap<CarForCreationDTO, Car>();

            CreateMap<CarForUpdateDTO, Car>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
