using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformSerice.Profiles
{
    public class PlatformProfile : Profile
    {
        public PlatformProfile()
        {
            //source -->target
            CreateMap<Platform, PlatformReadDto>();            
            CreateMap<PlatformCreateDto, Platform>();            
            CreateMap<Platform, PlatformPublishDto>();            
        }
    }
}