using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    
    [Route("api/commandsService/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController()
        {
            
        }




        [HttpPost]
        public ActionResult TestinboundConnection()
        {
            System.Console.WriteLine( "Inbound post"  ); 
            return Ok("Inbound post");
        }


    }
}