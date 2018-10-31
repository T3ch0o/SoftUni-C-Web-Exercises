namespace IRunes.Services
{
    using IRunes.Data;
    using IRunes.Models;
    using IRunes.Services.Interfaces;
    using System.Linq;

    internal class UsersService : IUsersService
    {
        private readonly IRunesDbContext _db;

        private readonly IHashService _hashService;

        public UsersService(IRunesDbContext db, IHashService hashService)
        {
            _db = db;
            _hashService = hashService;
        }

        public bool UserExists(string username, string password)
        {
            string hashedPassword = _hashService.Hash(password);

            bool userExists = _db.Users.Any(u => (u.Email == username || u.Username == username) && u.Password == hashedPassword);

            return userExists;
        }
    }
}