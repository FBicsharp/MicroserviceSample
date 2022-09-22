

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public PlatformsController(IPlatformRepo repository,IMapper mapper,ICommandDataClient commandDataClient)
        {

            _repository= repository;
            _mapper=mapper;
            _commandDataClient = commandDataClient;
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
            try
            {
                await _commandDataClient.SendPlatformToCommandAsync(platformCreated);              
            }
            catch (System.Exception ex)
            {                
                System.Console.WriteLine( ex.Message );
            }

            return CreatedAtRoute(nameof(CreatePlatforms) , new {Id=platformCreated.Id},platformCreated);
        }









    }
}