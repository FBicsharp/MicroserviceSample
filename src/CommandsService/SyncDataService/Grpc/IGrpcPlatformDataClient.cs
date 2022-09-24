using CommandsService.Models;

namespace CommandsService.SyncDataService.Grpc
{
    public interface IGrpcPlatformDataClient
    {
        IEnumerable<Platform> ReturnAllPlatforms();
    }
}