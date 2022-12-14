using CommandsService.Models;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context ;

        public CommandRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateCommand(int platformId, Command command)
        {
            if (command== null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            command.PlatformId=platformId; 
            _context.Commands.Add(command);
        }

        public void CreatePlatform(Platform plat)
        {
            if (plat== null)
            {
                throw new ArgumentNullException(nameof(plat));
            }
            _context.Platforms.Add(plat);
        }

        public bool ExternalPlatformExists(int externalPlatformId)        
            =>_context.Platforms.Any(p=>p.ExternalId==externalPlatformId);
        

        public IEnumerable<Platform> GetAllPlatform()
            => _context.Platforms.ToList();

        public Command GetCommand(int platformId, int commandId)
        
            =>_context.Commands.Where(c=>c.PlatformId ==platformId && c.Id==commandId).FirstOrDefault();
        

        public IEnumerable<Command> GetCommandFroPlatform(int platformId)
            =>_context.Commands.Where(c=>c.PlatformId ==platformId).OrderBy(c=> c.Platform.Id);
        

        public bool PlatformExists(int platformId) 
            =>_context.Platforms.Any(p=>p.Id==platformId);

        public bool SaveChanges() 
            => (_context.SaveChanges()>=0);
    }



}