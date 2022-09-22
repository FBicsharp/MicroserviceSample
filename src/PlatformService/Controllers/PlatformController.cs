

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformSerice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformController :ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;

        public PlatformController(IPlatformRepo repository,IMapper mapper)
        {
            _repository= repository;
            _mapper=mapper;
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
        public ActionResult<IEnumerable<PlatformReadDto>> CreatePlatforms(PlatformCreateDto newplatform)
        {
            var platform = _mapper.Map<Platform>(newplatform);

            _repository.CreatePlatform(platform);
            _repository.SaveChanges();
            var platformRead = _mapper.Map<PlatformReadDto>(platform);

            return CreatedAtRoute(nameof(CreatePlatforms) , new {Id=platformRead.Id},platformRead);
        }









    }
}