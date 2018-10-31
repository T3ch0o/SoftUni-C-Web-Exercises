namespace IRunes.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;

    using HTTP.WebServer.Result;

    using IRunes.Data;
    using IRunes.Services;
    using IRunes.Services.Interfaces;

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

        private const string LayoutFileName = "_Layout";

        private const string AuthorizedNavigationBarFileName = "UserLoggedIn";

        private const string UnauthorizedNavigationBarFileName = "UserLoggedOut";

        private bool _isAuthenticated = false;

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

            string layoutView = $"{RootDirectoryRelativePath}{ViewsFolderName}/{LayoutFileName}{FileExtension}";
            string filePath = $"{RootDirectoryRelativePath}{ViewsFolderName}/{ControllerName}/{viewName}{FileExtension}";

            string userNavigationBar = GetNavigationBar();

            if (!File.Exists(filePath))
            {
                return new BadRequestResult($"Views {viewName} not found", HttpResponseStatusCode.NotFound);
            }

            string viewContent = BuildViewContent(filePath);
            string view = File.ReadAllText(layoutView)
                              .Replace("@RenderNavigationBar", userNavigationBar)
                              .Replace("@RenderBody", viewContent);

            return new HtmlResult(view, HttpResponseStatusCode.OK);
        }

        private string GetNavigationBar()
        {
            string navigationBarPath = $"{RootDirectoryRelativePath}{ViewsFolderName}";

            if (_isAuthenticated)
            {
                navigationBarPath += $"/{AuthorizedNavigationBarFileName}{FileExtension}";
            }
            else
            {
                navigationBarPath += $"/{UnauthorizedNavigationBarFileName}{FileExtension}";
            }

            string navigationBarView = File.ReadAllText(navigationBarPath);

            return navigationBarView;
        }

        private string BuildViewContent(string filePath)
        {
            string viewContent = File.ReadAllText(filePath);

            foreach (string viewBagKey in ViewBag.Keys)
            {
                string dataPlaceholder = $"{{{{{viewBagKey}}}}}";

                if (viewContent.Contains(dataPlaceholder))
                {
                    viewContent = viewContent.Replace(dataPlaceholder, ViewBag[viewBagKey]);
                }
            }

            return viewContent;
        }

        protected IHttpResponse Redirect(string path) => new RedirectResult(path);

        protected IHttpResponse BadRequestError(string error) => new BadRequestResult(error, HttpResponseStatusCode.BadRequest);

        protected bool IsAuthenticated(IHttpRequest request)
        {
            _isAuthenticated = request.Session.ContainsParameter("username");
            return _isAuthenticated;
        }

        protected void SignInUser(string username, IHttpRequest request, IHttpResponse response)
        {
            request.Session.AddParameter("username", username);
            response.Cookies.Add(new HttpCookie("IRunes_auth", UserCookieService.GetUserCookie(username)));
        }
    }
}