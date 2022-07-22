using AutoMapper;
using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Resources;
using System.Linq;

namespace May25.API.Core.MappingProfiles
{
    public class MessageMappingProfile : Profile
    {
        public MessageMappingProfile()
        {
            CreateMap<Message, MessageDTO>()
                .ForMember(dest => dest.FromUserName, opt => opt.MapFrom(src => src.FromUser.FirstName + " " + src.FromUser.LastName.First()))
                .ForMember(dest => dest.ToUserName, opt => opt.MapFrom(src => src.ToUser.FirstName + " " + src.ToUser.LastName.First()));

            CreateMap<MessageForCreationDTO, Message>();
        }
    }
}
