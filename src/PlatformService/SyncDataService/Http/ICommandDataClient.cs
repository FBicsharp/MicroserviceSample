using PlatformService.Dtos;

namespace PlatformService.SyncDataService.Http
{
    public interface ICommandDataClient
    {
        Task SendPlatformToCommandAsync(PlatformReadDto platform);      

    }

    
}