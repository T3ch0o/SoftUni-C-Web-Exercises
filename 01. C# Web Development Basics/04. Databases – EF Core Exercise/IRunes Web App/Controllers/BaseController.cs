namespace IRunes.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;

    using CakeWebApp.Services;
    using CakeWebApp.Services.Interfaces;

    using HTTP.WebServer.Result;

    using IRunes.Data;

    using SIS.HTTP.Cookies;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Interfaces;
    using SIS.HTTP.Responses.Interfaces;
    using SIS.WebServer.Result;

    internal abstract class BaseController
    {
        private const string RootDirectoryRelativePath = "../../../";

        private const string ViewsFolderName = "Views";

        private const string FileExtension = ".html";

        private string ControllerName => GetType().Name.Replace("Controller", string.Empty);

        protected IRunesDbContext Db { get; } = new IRunesDbContext();

        protected readonly IUserCookieService UserCookieService = new UserCookieService();

        protected IDictionary<string, string> ViewBag { get; set; } = new Dictionary<string, string>();

        protected IHttpResponse View([CallerMemberName] string viewName = "")
        {
            if (viewName.Contains("Get") || viewName.Contains("Post"))
            {
                viewName = viewName
                           .Replace("Get", string.Empty)
                           .Replace("Post", string.Empty);
            }

            string filePath = $"{RootDirectoryRelativePath}{ViewsFolderName}/{ControllerName}/{viewName}{FileExtension}";

            if (!File.Exists(filePath))
            {
                return new BadRequestResult($"Views {viewName} not found", HttpResponseStatusCode.NotFound);
            }

            string fileContent = File.ReadAllText(filePath);

            foreach (string viewBagKey in ViewBag.Keys)
            {
                string dataPlaceholder = $"{{{{{viewBagKey}}}}}";

                if (fileContent.Contains(dataPlaceholder))
                {
                    fileContent = fileContent.Replace(dataPlaceholder, ViewBag[viewBagKey]);
                }
            }

            return new HtmlResult(fileContent, HttpResponseStatusCode.OK);
        }

        protected IHttpResponse Redirect(string path) => new RedirectResult(path);

        protected IHttpResponse BadRequestError(string error) => new BadRequestResult(error, HttpResponseStatusCode.BadRequest);

        protected bool IsAuthenticated(IHttpRequest request)
        {
            return request.Session.ContainsParameter("username");
        }

        protected void SignInUser(string username, IHttpRequest request, IHttpResponse response)
        {
            request.Session.AddParameter("username", username);
            response.Cookies.Add(new HttpCookie("IRunes_auth", UserCookieService.GetUserCookie(username)));
        }
    }
}