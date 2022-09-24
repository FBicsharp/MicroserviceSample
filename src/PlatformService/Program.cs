using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataService;
using PlatformService.Data;
using PlatformService.SyncDataService.Grpc;
using PlatformService.SyncDataService.Http;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var DevelopmentMode = builder.Environment.IsDevelopment() ;
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


if (DevelopmentMode)
{
    builder.Services.AddDbContext<AppDbContext>(opt=>
        opt.UseInMemoryDatabase("InMemDb"));    
    Console.WriteLine("--> Using InMemoryDb");
}
else
{
    builder.Services.AddDbContext<AppDbContext>(opt=>
        opt.UseSqlServer(configuration.GetConnectionString("PlatformsConnection")));
    Console.WriteLine("--> Using UseSqlServerDb");
} 

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient,MessageBusClient>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddGrpc();


var app = builder.Build();

if (DevelopmentMode || bool.Parse(configuration["EnableSwaggerUi"] ))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (bool.Parse(configuration["HttpsRedirection"]))
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<GrpcPlatformService>();
app.MapGet("/protos/platforms.proto", ()=>   
{
    var path = "Protos/platforms.proto";
    var content = System.IO.File.Exists(path)? System.IO.File.ReadAllText("Protos/platforms.proto"): string.Empty ;
    return content;
});

PreDb.PrepPopulation(app,DevelopmentMode);

app.Run();
