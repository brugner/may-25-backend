using AutoMapper;
using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Resources;

namespace May25.API.Core.MappingProfiles
{
    public class NotificationMappingProfile : Profile
    {
        public NotificationMappingProfile()
        {
            CreateMap<NotificationForCreationDTO, Notification>();
            CreateMap<Notification, NotificationDTO>();
            CreateMap<NotificationToken, NotificationTokenDTO>();
        }
    }
}
