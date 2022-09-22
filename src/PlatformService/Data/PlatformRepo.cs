using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _context ;

        public PlatformRepo(AppDbContext context)
        {
            _context = context;
        }


        public void CreatePlatform(Platform plat)
        {
            if (plat==null)
            {
                throw new ArgumentNullException(nameof(plat));
            }  
             _context.Platforms.Add(plat);
        }

        public IEnumerable<Platform> GetAllPlatform() => _context.Platforms.ToList();

        public Platform GetPlatformById(int id) 
        {
            //TODO implementing PlatformEmpty to avoid retunr null
            return _context.Platforms.FirstOrDefault(x=>x.Id == id) ;
        }

        public bool SaveChanges() => (_context.SaveChanges()>=0);
    }



}