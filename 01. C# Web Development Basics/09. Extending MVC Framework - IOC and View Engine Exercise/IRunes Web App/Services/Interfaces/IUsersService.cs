namespace IRunes.Services.Interfaces
{
    internal interface IUsersService
    {
        bool UserExists(string username, string password);
    }
}