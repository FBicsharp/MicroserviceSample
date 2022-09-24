using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PreDb
    {
        public static void PrepPopulation(IApplicationBuilder app,bool isDevelopment)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
            SeedData(context);
            if(!isDevelopment)
                ApplyMigrations(context);
            
        }
        private static void SeedData(AppDbContext context)
        {
            if (!context.Platforms.Any())
            {
                System.Console.WriteLine( "Seeding data");
                context.Platforms.AddRange(
                    new Platform(){Name="Dotnet",Publisher="Microsoft",Cost="Free"},
                    new Platform(){Name="SQL Server",Publisher="Microsoft",Cost="Free"},
                    new Platform(){Name="Kubernetes",Publisher="Cloud Native Computing Foundation",Cost="Free"}
                );
            }
            context.SaveChanges();
        }
        private static void ApplyMigrations(AppDbContext context)
        {
            System.Console.WriteLine("--> attempting to apply migration");
            try
            {
                context.Database.Migrate();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"--> FAIL to apply migration {ex.Message}");
            }
        }
    }
}