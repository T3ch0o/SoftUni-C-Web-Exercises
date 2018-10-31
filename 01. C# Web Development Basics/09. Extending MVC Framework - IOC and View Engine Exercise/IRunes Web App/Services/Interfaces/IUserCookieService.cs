namespace IRunes.Services.Interfaces
{
    internal interface IUserCookieService
    {
        string GetUserCookie(string username);

        string GetUserData(string cookieContent);
    }
}