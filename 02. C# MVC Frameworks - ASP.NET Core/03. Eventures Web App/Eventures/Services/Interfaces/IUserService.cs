namespace Eventures.Services.Interfaces
{
    using System.Linq;
    using System.Threading.Tasks;

    using Eventures.Models;

    public interface IUserService
    {
        IQueryable<ApplicationUser> GetAll(string currentUser);

        Task ChangeUserRole(string userData);
    }
}
