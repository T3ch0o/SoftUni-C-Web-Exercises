namespace Eventures.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Eventures.Data;
    using Eventures.Models;
    using Eventures.Services.Interfaces;

    using Microsoft.AspNetCore.Identity;

    public class UserService : IUserService
    {
        private readonly EventuresDbContext _db;

        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(EventuresDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IQueryable<ApplicationUser> GetAll(string currentUser)
        {

            return _db.Users.Where(u => u.UserName != currentUser);
        }

        public async Task ChangeUserRole(string userData)
        {
            string[] userInfo = userData.Split(" ");

            ApplicationUser user = _db.Users.FirstOrDefault(u => u.Id == userInfo[0]);

            if (userInfo[1] != "Admin")
            {
                await _userManager.AddToRoleAsync(user, "Administrator");
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(user, "Administrator");
            }
        }
    }
}
