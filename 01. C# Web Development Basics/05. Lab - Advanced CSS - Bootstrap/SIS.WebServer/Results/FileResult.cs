namespace SIS.WebServer.Results
{
    using SIS.HTTP.Headers;
    using SIS.HTTP.Responses;

    public class FileResult : HttpResponse
    {
        public FileResult(byte[] content)
        {
            Headers.Add(new HttpHeader(HttpHeader.ContentLength, content.Length.ToString()));
            Headers.Add(new HttpHeader(HttpHeader.ContentDisposition, "inline"));
            Content = content;
        }
    }
}