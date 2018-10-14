namespace HTTP.WebServer.Result
{
    using System.Text;

    using SIS.HTTP.Enums;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Responses;

    public class BadRequestResult : HttpResponse
    {
        private const string DefaultErrorHeading = "<h1>Error occured, see details</h1>";

        public BadRequestResult(string content, HttpResponseStatusCode responseStatusCode) : base(responseStatusCode)
        {
            content = $"{DefaultErrorHeading}\n{content}";

            Headers.Add(new HttpHeader("Content-Type", "text/html"));
            Content = Encoding.UTF8.GetBytes(content);
        }
    }
}