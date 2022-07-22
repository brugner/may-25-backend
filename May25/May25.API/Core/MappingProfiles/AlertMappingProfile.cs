using AutoMapper;
using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Resources;

namespace May25.API.Core.MappingProfiles
{
    public class AlertMappingProfile : Profile
    {
        public AlertMappingProfile()
        {
            CreateMap<Alert, AlertDTO>();

            CreateMap<AlertForCreationDTO, Alert>();
        }
    }
}
