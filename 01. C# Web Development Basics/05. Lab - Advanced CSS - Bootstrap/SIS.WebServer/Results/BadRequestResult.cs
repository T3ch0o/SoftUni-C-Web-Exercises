namespace SIS.WebServer.Results
{
    using System;
    using System.Text;

    using SIS.HTTP.Enums;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Responses;

    public class BadRequestResult : HttpResponse
    {
        private const string DefaultErrorHeading = "<h1>Error of type occured, see details</h1>";

        public BadRequestResult(string content, HttpResponseStatusCode responseStatusCode) : base(responseStatusCode)
        {
            content = DefaultErrorHeading + Environment.NewLine + content;
            Headers.Add(new HttpHeader(HttpHeader.ContentType, "text/html"));
            Content = Encoding.UTF8.GetBytes(content);
        }
    }
}