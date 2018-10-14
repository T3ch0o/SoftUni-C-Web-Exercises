namespace SIS.Demo
{
    using SIS.HTTP.Enums;
    using SIS.HTTP.Responses.Interfaces;
    using SIS.WebServer.Result;

    internal class HomeController
    {
        public IHttpResponse Index()
        {
            const string content = "<h1>Hello, World</h1>";

            return new HtmlResult(content, HttpResponseStatusCode.OK);
        }
    }
}