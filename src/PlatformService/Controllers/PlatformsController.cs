

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataService;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataService.Http;

namespace PlatformSerice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController :ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(IPlatformRepo repository
            ,IMapper mapper
            ,ICommandDataClient commandDataClient
            ,IMessageBusClient messageBusClient
        )
        {

            _repository= repository;
            _mapper=mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient=messageBusClient;
        }

        [HttpGet(Name=nameof(GetPlatforms))]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            var platforms =_repository.GetAllPlatform();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpGet("{id}",Name=nameof(GetPlatformById))]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platform=_repository.GetPlatformById(id);
            if(platform == null)
            {
                return NotFound();
            }  
            return Ok(_mapper.Map<PlatformReadDto>(platform));
        }


        [HttpPost(Name=nameof(CreatePlatforms))]
        public async Task<ActionResult<IEnumerable<PlatformReadDto>>> CreatePlatforms(PlatformCreateDto newplatform)
        {
            var platform = _mapper.Map<Platform>(newplatform);

            _repository.CreatePlatform(platform);
            _repository.SaveChanges();
            var platformCreated = _mapper.Map<PlatformReadDto>(platform);

            try//syncmethod
            {
                await _commandDataClient.SendPlatformToCommandAsync(platformCreated);     
            }
            catch (System.Exception ex)
            {                
                System.Console.WriteLine("Could not send synchronously"+ ex.Message );
            }
            try//asyncmethod
            {
                var platformPublish = _mapper.Map<PlatformPublishDto>(platform);
                platformPublish.Event ="PlatformPublished";
                _messageBusClient.PublishNewPlatform(platformPublish);                         
            }
            catch (System.Exception ex)
            {                
                System.Console.WriteLine("Could not send asynchronously"+ ex.Message );
            }


            return CreatedAtRoute(nameof(CreatePlatforms) , new {Id=platformCreated.Id},platformCreated);
        }









    }
}