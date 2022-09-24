using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    
    [Route("api/commandsService/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _commandRepo;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo commandRepo,IMapper mapper)
        {
            _commandRepo=commandRepo;
            _mapper=mapper;
        }



        [HttpGet(Name =nameof(GetCommandsForPlatform))]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {            
            System.Console.WriteLine("-->GetCommandsForPlatform");
            if (!_commandRepo.PlatformExists(platformId))
                return NotFound();
            
            var commanditems = _commandRepo.GetCommandFroPlatform(platformId);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commanditems));
        }

        [HttpGet("{commandId}",Name =nameof(GetCommandForPlatform))]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId,int commandId)
        {            
            System.Console.WriteLine("-->GetCommandForPlatform");
            System.Console.WriteLine($"-->{platformId}/{commandId}");
            if (!_commandRepo.PlatformExists(platformId))
                return NotFound();
            
            var commanditem = _commandRepo.GetCommand(platformId,commandId);
            if (commanditem == null)
                return NotFound();
                
            return Ok(_mapper.Map<CommandReadDto>(commanditem));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(int platformId,CommandCreateDto commandCreateDto)
        {            
            System.Console.WriteLine("-->CreateCommand");
            
            if (!_commandRepo.PlatformExists(platformId))
                return NotFound();
            
            var newcommand = _mapper.Map<Command>(commandCreateDto);
            _commandRepo.CreateCommand(platformId,newcommand);
            if (!_commandRepo.SaveChanges())
            {
               return BadRequest("Command not created");
            }

            var newcommandReadDto = _mapper.Map<CommandReadDto>(newcommand);           
                
            return CreatedAtRoute(
                nameof(GetCommandForPlatform),
                new{platformId=platformId,commandId=newcommandReadDto.Id},newcommandReadDto
            );
        }
    }
} 