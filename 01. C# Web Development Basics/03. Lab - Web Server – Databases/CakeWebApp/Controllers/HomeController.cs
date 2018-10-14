namespace CakeWebApp.Controllers
{
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Interfaces;
    using SIS.HTTP.Responses.Interfaces;
    using SIS.WebServer.Result;

    internal class HomeController : BaseController
    {
        public IHttpResponse Index(IHttpRequest request)
        {
            return View("Index");
        }

        public IHttpResponse Asd(IHttpRequest request)
        {
            return new HtmlResult($"<h1>{GetUsername(request)}</h1>", HttpResponseStatusCode.OK);
        }
    }
}