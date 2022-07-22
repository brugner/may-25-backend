using AutoMapper;
using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Resources;
using System.Linq;

namespace May25.API.Core.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(x => x.Role.Name)));

            CreateMap<User, CreatedUserDTO>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(x => x.Role.Name)));

            CreateMap<UserForUpdateDTO, User>()
               .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<User, UserPublicProfileDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName.FirstOrDefault() + "."));
        }
    }
}
