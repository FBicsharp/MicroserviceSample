using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory=scopeFactory;
            _mapper=mapper;
        }
        public void ProcessEvent(string messsage)
        {
            var eventType = DetermineEvent(messsage);
            switch (eventType)
            {
                case EnumEventType.PlatformPublisched:
                    AddPlatform(messsage);
                    break;                
                
                default:
                    break;
            }
            
        }
        EnumEventType DetermineEvent(string notificationMesssage)
        {
            System.Console.WriteLine("-->Determinig Event"   );
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMesssage);
            switch (eventType.Event)
            {
                case "PlatformPublished":
                    System.Console.WriteLine("-->PlatformPublished Event detected"   );
                    return EnumEventType.PlatformPublisched;                
                default:
                    System.Console.WriteLine("-->Unknow Event detected"   );
                    return EnumEventType.Undetermined;
            }            
        }
        private void AddPlatform(string platformPublishedMessage)
        {
                using var scope = _scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);
                try
                {
                    var plat = _mapper.Map<Platform>(platformPublishedDto);
                    if (!repo.ExternalPlatformExists(plat.ExternalId) )
                    {
                        repo.CreatePlatform(plat);
                        repo.SaveChanges();
                        System.Console.WriteLine("-->Platform added ");
                        return;
                    }
                    System.Console.WriteLine("-->Platform already exists");
                    
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine("Could not add platform "+ex.Message);
                    
                }
        }

    }

    enum EnumEventType
    {
        PlatformPublisched,
        Undetermined,
    }
    

}