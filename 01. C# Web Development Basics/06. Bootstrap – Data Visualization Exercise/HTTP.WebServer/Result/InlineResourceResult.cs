namespace HTTP.WebServer.Result
{
    using System.Text;

    using SIS.HTTP.Enums;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Responses;

    public class InlineResourceResult : HttpResponse
    {
        private const string DefaultErrorHeading = "<h1>Error occured, see details</h1>";

        public InlineResourceResult(byte[] content, HttpResponseStatusCode responseStatusCode) : base(responseStatusCode)
        {
            Headers.Add(new HttpHeader(HttpHeader.ContentLength, "text/html"));
            Headers.Add(new HttpHeader(HttpHeader.ContentDisposition, "inline"));
            Content = content;
        }
    }
}