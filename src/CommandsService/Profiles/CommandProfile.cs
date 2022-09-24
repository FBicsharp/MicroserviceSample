using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.Profiles
{
    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
            //source -->target
             CreateMap<Platform, PlatformReadDto>();            
             CreateMap<Command,CommandReadDto>();            
             CreateMap<CommandCreateDto, Command>();            
        }
    }
}