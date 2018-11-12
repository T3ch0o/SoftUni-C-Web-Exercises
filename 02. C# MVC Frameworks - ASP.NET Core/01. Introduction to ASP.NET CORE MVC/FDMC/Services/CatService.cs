namespace FDMC.Services
{
    using System.Linq;

    using FDMC.Data;
    using FDMC.Models;
    using FDMC.Models.ViewModels;
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

        public void AddCat(AddCatViewModel model)
        {
            Cat cat = new Cat
            {
                Name = model.Name,
                Age = model.Age,
                Breed = model.Breed,
                Url = model.Url
            };

            _db.Cats.Add(cat);
            _db.SaveChanges();
        }
    }
}
