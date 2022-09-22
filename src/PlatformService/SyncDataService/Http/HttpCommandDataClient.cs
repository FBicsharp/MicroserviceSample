using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.SyncDataService.Http
{    
    public class HttpCommandDataClient :ICommandDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpclient;

        public HttpCommandDataClient(HttpClient httpclient,IConfiguration configuration)
        {
            _configuration = configuration;
            _httpclient=httpclient;
        }

        public async Task SendPlatformToCommandAsync(PlatformReadDto platform)
        {
            var httpcontent = new StringContent(
                JsonSerializer.Serialize(platform)
                ,encoding: Encoding.UTF8
                ,mediaType: "application/json"
            );
            
            //TODO: traslate to a service            
            var response = await _httpclient.PostAsync(_configuration["CommandService"],httpcontent); 

            if (response.IsSuccessStatusCode)
            {   
                System.Console.WriteLine("--> SyncPOST Ok");
                return;
            }
            System.Console.WriteLine("--> SyncPOST NOT Ok");

        }
    }
} 