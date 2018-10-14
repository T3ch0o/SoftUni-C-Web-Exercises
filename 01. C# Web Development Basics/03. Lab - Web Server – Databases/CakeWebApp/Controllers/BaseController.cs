namespace CakeWebApp.Controllers
{
    using System.IO;
    using System.Net;

    using CakeWebApp.Data;
    using CakeWebApp.Services;
    using CakeWebApp.Services.Interfaces;

    using SIS.HTTP.Cookies;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Interfaces;
    using SIS.HTTP.Responses.Interfaces;
    using SIS.WebServer.Result;

    internal abstract class BaseController
    {
        protected CakesDbContext Db { get; set; } = new CakesDbContext();

        protected IUserCookieService UserCookieService = new UserCookieService();

        protected string GetUsername(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie(".auth-cakes"))
            {
                return null;
            }

            string cookieContent = request.Cookies.GetCookie(".auth-cakes").Value;
            string username = UserCookieService.GetUserData(cookieContent);

            return username;
        }

        protected IHttpResponse View(string viewName)
        {
            string content = File.ReadAllText("Views/" + viewName + ".html");

            return new HtmlResult(content, HttpResponseStatusCode.OK);
        }

        protected IHttpResponse BadRequestError(string errorMessage)
        {
            return new HtmlResult($"<h1>{errorMessage}</h1>", HttpResponseStatusCode.BadRequest);
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            return new HtmlResult($"<h1>{errorMessage}</h1>", HttpResponseStatusCode.InternalServerError);
        }
    }
}