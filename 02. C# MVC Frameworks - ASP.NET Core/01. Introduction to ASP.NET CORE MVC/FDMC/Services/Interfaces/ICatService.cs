namespace FDMC.Services.Interfaces
{
    using System.Linq;

    using FDMC.Models;

    public interface ICatService
    {
        IQueryable<Cat> GetAllCats();
    }
}
