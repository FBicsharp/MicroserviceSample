// using Microsoft.EntityFrameworkCore;
// using PlatformService.Models;

// namespace PlatformService.Data
// {
//     public static class PreDb
//     {
//         public static void PrepPopulation(IApplicationBuilder app,bool isDevelopment)
//         {            
//             using var serviceScope = app.ApplicationServices.CreateScope();
//             var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
//             ValidateConnectionString(context);
            
//             if(!isDevelopment)
//                 ApplyMigrations(context);

//             SeedData(context);
            
//         }

//         private static void ValidateConnectionString(AppDbContext? context)
//         {
//             if (string.IsNullOrEmpty(context.Database.GetConnectionString())) 
//             {
//                 System.Console.WriteLine("-->Connection string not setted");
//             }
//             System.Console.WriteLine("-->Connection string setted");
//         }

//         private static void SeedData(AppDbContext context)
//         {
//             if (!context.Platforms.Any())
//             {
//                 System.Console.WriteLine( "Seeding data");
//                 context.Platforms.AddRange(
//                     new Platform(){Name="Dotnet",Publisher="Microsoft",Cost="Free"},
//                     new Platform(){Name="SQL Server",Publisher="Microsoft",Cost="Free"},
//                     new Platform(){Name="Kubernetes",Publisher="Cloud Native Computing Foundation",Cost="Free"}
//                 );
//             }
//             context.SaveChanges();
//         }
//         private static void ApplyMigrations(AppDbContext context)
//         {
//             System.Console.WriteLine("--> attempting to apply migration");
//             try
//             {
//                 context.Database.Migrate();
//             }
//             catch (System.Exception ex)
//             {
//                 System.Console.WriteLine($"--> FAIL to apply migration {ex.Message}");
//             }
//         }
//     }
// }