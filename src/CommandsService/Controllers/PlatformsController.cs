using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    
    [Route("api/commandsService/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepo _commandRepo;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepo commandRepo,IMapper mapper)
        {
            _commandRepo=commandRepo;
            _mapper=mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
                System.Console.WriteLine("-->GetPlatforms");
                var platformitem = _commandRepo.GetAllPlatform();
                return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformitem));
        }




        [HttpPost]
        public ActionResult TestinboundConnection()
        {
            System.Console.WriteLine( "Inbound post"  ); 
            return Ok("Inbound post");
        }

    }
}