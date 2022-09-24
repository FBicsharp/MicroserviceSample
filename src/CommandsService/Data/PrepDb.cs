using Microsoft.EntityFrameworkCore;
using CommandsService.Models;
using CommandsService.SyncDataService.Grpc;

namespace CommandsService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isDevelopment)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var commandrepo = serviceScope.ServiceProvider.GetService<ICommandRepo>();
            var grpcClient = serviceScope.ServiceProvider.GetService<IGrpcPlatformDataClient>();
            var platformItems = grpcClient.ReturnAllPlatforms();

            SeedData(commandrepo, platformItems);
        }


        private static void SeedData(ICommandRepo commandrepo, IEnumerable<Platform> platforms)
        {
            System.Console.WriteLine("-->Seeding data");
            foreach (var platform in platforms)
            {

                if (!commandrepo.ExternalPlatformExists(platform.ExternalId))
                {
                    commandrepo.CreatePlatform(platform);
                }
            }
            commandrepo.SaveChanges();

        }

    }
}