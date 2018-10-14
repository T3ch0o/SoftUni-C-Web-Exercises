namespace SIS.WebServer.Result
{
    using SIS.HTTP.Enums;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Responses;

    public class RedirectResult : HttpResponse
    {
        public RedirectResult(string location) : base(HttpResponseStatusCode.Redirect)
        {
            Headers.Add(new HttpHeader("Location", location));
        }
    }
}