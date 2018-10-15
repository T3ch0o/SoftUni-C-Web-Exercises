namespace SIS.WebServer.Results
{
    using SIS.HTTP.Enums;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Responses;

    public class InlineResourceResult : HttpResponse
    {
        public InlineResourceResult(byte[] content, HttpResponseStatusCode responseStatusCode) : base(responseStatusCode)
        {
            Headers.Add(new HttpHeader(HttpHeader.ContentLength, content.Length.ToString()));
            Headers.Add(new HttpHeader(HttpHeader.ContentDisposition, "inline"));
            Content = content;
        }
    }
}