using CommandsService.Models;

namespace CommandsService.Data
{
    public interface ICommandRepo
    {

        bool SaveChanges();
        #region   PLATFORM
        IEnumerable<Platform> GetAllPlatform();
        void CreatePlatform(Platform plat);
        bool PlatformExists(int platformId);
        #endregion  

        #region   COMMAND
        IEnumerable<Command> GetCommandFroPlatform(int platformId);
        Command GetCommand(int platformId,int commandId);

        void CreateCommand(int platformId,Command command);
        
        #endregion  
    
    }    
    


}