namespace SIS.WebServer.Result
{
    using System.Text;

    using SIS.HTTP.Enums;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Responses;

    public class UnauthorizedResult : HttpResponse
    {
        private const string DefaultErrorHeading = "<h1>You have no permission to acces this functionality</h1>";

        public UnauthorizedResult() : base(HttpResponseStatusCode.Unauthorized)
        {
            Headers.Add(new HttpHeader(HttpHeader.ContentType, "text/html"));
            Content = Encoding.UTF8.GetBytes(DefaultErrorHeading);
        }
    }
}