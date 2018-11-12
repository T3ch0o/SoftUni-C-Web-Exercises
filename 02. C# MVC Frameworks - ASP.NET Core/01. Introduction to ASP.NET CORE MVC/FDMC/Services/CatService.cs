namespace FDMC.Services
{
    using System.Linq;

    using FDMC.Data;
    using FDMC.Models;
    using FDMC.Services.Interfaces;

    public class CatService : ICatService
    {
        private readonly FdmcDbContext _db;

        public CatService(FdmcDbContext db)
        {
            _db = db;
        }

        public IQueryable<Cat> GetAllCats()
        {
            return _db.Cats;
        }
    }
}
