using CommandsService.AsyncDataService;
using CommandsService.Data;
using CommandsService.EventProcessing;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var DevelopmentMode = builder.Environment.IsDevelopment() ;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// if (DevelopmentMode)
// {
    builder.Services.AddDbContext<AppDbContext>(opt=>
        opt.UseInMemoryDatabase("InMemDb"));    
    Console.WriteLine("--> Using InMemoryDb");
// }
// else
// {
//     builder.Services.AddDbContext<AppDbContext>(opt=>
//         opt.UseSqlServer(configuration.GetConnectionString("CommandsConnection")));
//     Console.WriteLine("--> Using UseSqlServerDb");
// } 

builder.Services.AddScoped<ICommandRepo, CommandRepo>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddHostedService<MessageBusSubscriber>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


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

app.Run();
