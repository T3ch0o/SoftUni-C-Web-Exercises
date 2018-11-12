namespace FDMC.Services.Interfaces
{
    using System.Linq;

    using FDMC.Models;
    using FDMC.Models.ViewModels;

    public interface ICatService
    {
        IQueryable<Cat> GetAllCats();

        void AddCat(AddCatViewModel model);
    }
}
